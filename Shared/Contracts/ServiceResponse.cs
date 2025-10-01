namespace Shared.Contracts
{
    public class ServiceResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Id của dịch vụ cung cấp dịch vụ
        /// </summary>
        public string? ShopId { get; set; }
        /// <summary>
        /// Mô tả dịch vụ
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Đường dẫn hình ảnh dịch vụ
        /// </summary>
        public string? ImgUrl { get; set; }
        /// <summary>
        /// trạng thái dịch vụ (1: hoạt động, 0: không hoạt động)
        /// </summary>
        public int Status { get; set; }
    }
}
