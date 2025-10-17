using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;

namespace CareNest_Service_Category.Application.Features.Queries.GetByShopId
{
    public class GetByShopIdQuery : IQuery<List<GetAllPaging.ServiceResponse>>
    {
        public string ShopId { get; set; } = string.Empty;
    }
}


