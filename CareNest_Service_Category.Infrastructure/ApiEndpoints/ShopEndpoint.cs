namespace CareNest_Service_Category.Infrastructure.ApiEndpoints
{
    public class ShopEndpoint
    {
        public static string GetById(string? id) => $"/api/shop/{id}";
    }
}