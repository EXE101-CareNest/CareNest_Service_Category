using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;
using CareNest_Service_Category.Application.Interfaces.UOW;

namespace CareNest_Service_Category.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQueryHandler : IQueryHandler<GetAllPagingQuery, PageResult<ServiceResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            IEnumerable<ServiceResponse> a = await _unitOfWork.GetRepository<ServiceCategory>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: selector,
                pageSize: query.PageSize,
                pageIndex: query.Index);

            return new PageResult<ServiceResponse>(a, 1, query.PageSize, query.Index);
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
