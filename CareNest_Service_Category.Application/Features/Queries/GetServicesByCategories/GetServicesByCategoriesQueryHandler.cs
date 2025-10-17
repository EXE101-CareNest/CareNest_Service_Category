using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;
using CareNest_Service_Category.Application.Interfaces.Services;

namespace CareNest_Service_Category.Application.Features.Queries.GetServicesByCategories
{
    public class GetServicesByCategoriesQueryHandler : IQueryHandler<GetServicesByCategoriesQuery, List<ServiceByCategoryResponse>>
    {
        private readonly IService _service;

        public GetServicesByCategoriesQueryHandler(IService service)
        {
            _service = service;
        }

        public async Task<List<ServiceByCategoryResponse>> HandleAsync(GetServicesByCategoriesQuery query)
        {
            ResponseResult<List<ServiceByCategoryResponse>> resp = await _service.GetServicesByCategoryIds(query.CategoryIds);
            if (!resp.IsSuccess || resp.Data?.Data == null)
            {
                return new List<ServiceByCategoryResponse>();
            }
            return resp.Data.Data;
        }
    }
}


