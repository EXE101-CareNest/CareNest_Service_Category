namespace CareNest_Service_Category.Infrastructure.ApiEndpoints
{
    public class ServiceEndpoint
    {
        public static string GetById(string? id) => $"/api/service/{id}";
        public static string GetByCategories() => "/api/service/by-categories";
    }
}
