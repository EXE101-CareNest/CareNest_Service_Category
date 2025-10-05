
namespace CareNest_Service_Category.Application.Features.Commands.Update
{
    public class UpdateRequest
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
