using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;

namespace CareNest_Service_Category.Application.Features.Queries.GetServicesByCategories
{
    public class GetServicesByCategoriesQuery : IQuery<List<ServiceByCategoryResponse>>
    {
        public List<string> CategoryIds { get; set; } = new List<string>();
    }
}


