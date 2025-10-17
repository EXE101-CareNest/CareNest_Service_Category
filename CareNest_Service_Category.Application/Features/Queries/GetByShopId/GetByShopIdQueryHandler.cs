using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;
using CareNest_Service_Category.Application.Interfaces.UOW;

namespace CareNest_Service_Category.Application.Features.Queries.GetByShopId
{
    public class GetByShopIdQueryHandler : IQueryHandler<GetByShopIdQuery, List<GetAllPaging.ServiceResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByShopIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllPaging.ServiceResponse>> HandleAsync(GetByShopIdQuery query)
        {
            var selector = ObjectMapperExtensions.CreateMapExpression<ServiceCategory, GetAllPaging.ServiceResponse>();

            IEnumerable<GetAllPaging.ServiceResponse> categories = await _unitOfWork
                .GetRepository<ServiceCategory>()
                .FindAsync(predicate: c => c.ShopId == query.ShopId,
                           orderBy: q => q.OrderBy(c => c.Name),
                           selector: selector);

            return categories.ToList();
        }
    }
}


