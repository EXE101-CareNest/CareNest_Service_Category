namespace CareNest_Service_Category.Application.Features.Queries.GetAllPaging
{
    public class ServiceResponse
    {
        /// <summary>
        /// Id dịch vụ 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Tên loại dịch vụ 
        /// </summary>
        public string? Name { get; set; }
        public string? ShopId { get; set; }
        public string? ShopName { get; set; }
    }
}
