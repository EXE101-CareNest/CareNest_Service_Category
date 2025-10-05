using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;

namespace CareNest_Service_Category.Application.Features.Commands.Create
{
    public class CreateCommand : ICommand<ServiceCategory>
    {
        /// <summary>
        /// Tên loại dịch vụ 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Id dịch vụ
        /// </summary>
        public string? ShopId { get; set; }

    }
}
