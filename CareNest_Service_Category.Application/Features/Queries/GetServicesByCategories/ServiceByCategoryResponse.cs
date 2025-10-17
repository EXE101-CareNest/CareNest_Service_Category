namespace CareNest_Service_Category.Application.Features.Queries.GetServicesByCategories
{
    public class ServiceByCategoryResponse
    {
        public string? Name { get; set; }
        public string? ServiceCategoryId { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public bool Status { get; set; }
        public string? Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}


