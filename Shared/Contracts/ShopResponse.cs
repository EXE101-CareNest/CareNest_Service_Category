using System.Text.Json.Serialization;
using Shared.Helper;

namespace Shared.Contracts
{
    public class ShopResponse
    {
        public string? Id { get; set; }
        public string? OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public int? Status { get; set; }  

        public string? ImgUrl { get; set; }
        public string? WorkingDays { get; set; }
    }
}
