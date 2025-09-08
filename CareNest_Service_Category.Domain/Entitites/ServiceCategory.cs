using CareNest_Service_Category.Domain.Commons;

namespace CareNest.Domain.Entitites
{
    public class ServiceCategory: BaseEntity
    {
        /// <summary>
        /// Tên dịch vụ 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Id dịch vụ
        /// </summary>
        public string? Service_Id { get; set; }
    }
}
