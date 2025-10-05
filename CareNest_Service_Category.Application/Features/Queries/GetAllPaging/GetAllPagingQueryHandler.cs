using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;
using CareNest_Service_Category.Application.Interfaces.Services;
using CareNest_Service_Category.Application.Interfaces.UOW;

namespace CareNest_Service_Category.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQueryHandler : IQueryHandler<GetAllPagingQuery, PageResult<ServiceResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopService _service;

        public GetAllPagingQueryHandler(IUnitOfWork unitOfWork, IShopService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<PageResult<ServiceResponse>> HandleAsync(GetAllPagingQuery query)
        {
            var selector = ObjectMapperExtensions.CreateMapExpression<ServiceCategory, ServiceResponse>();

            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);
            
            // Thêm điều kiện lọc theo ShopId nếu có
            System.Linq.Expressions.Expression<Func<ServiceCategory, bool>>? predicate = null;
            if (!string.IsNullOrWhiteSpace(query.ShopId))
            {
                predicate = s => s.ShopId == query.ShopId;
            }

            IEnumerable<ServiceResponse> serviceResponses = await _unitOfWork.GetRepository<ServiceCategory>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: selector,
                pageSize: query.PageSize,
                pageIndex: query.Index);

            // Lấy thông tin shop name cho mỗi service category
            List<ServiceResponse> result = new List<ServiceResponse>();
            foreach (var item in serviceResponses)
            {
                if (!string.IsNullOrEmpty(item.ShopId))
                {
                    try
                    {
                        var shopResponse = await _service.GetShopById(item.ShopId);
                        if (shopResponse?.Data?.Data != null)
                        {
                            item.ShopName = shopResponse.Data.Data.Name;
                        }
                    }
                    catch (Exception)
                    {
                        // Xử lý trường hợp không lấy được thông tin shop
                        // Chỉ bỏ qua và không cập nhật ShopName
                    }
                }
                result.Add(item);
            }

            return new PageResult<ServiceResponse>(result, 1, query.Index, query.PageSize);
        }


        private Func<IQueryable<ServiceCategory>, IOrderedQueryable<ServiceCategory>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => ascending ? q.OrderBy(a => a.Name) : q.OrderByDescending(a => a.Name),
                "updateat" => q => ascending ? q.OrderBy(a => a.UpdatedAt) : q.OrderByDescending(a => a.UpdatedAt),
                _ => q => q.OrderBy(a => a.CreatedAt) // fallback nếu không có sortColumn
            };
        }
    }
}
