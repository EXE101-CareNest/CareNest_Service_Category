using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;

namespace CareNest_Service_Category.Application.Features.Commands.Update
{
    public class UpdateCommand : ICommand<ServiceCategory>
    {
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Tên loại dịch vụ 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Id dịch vụ
        /// </summary>
        public string? Service_Id { get; set; }

    }
}
