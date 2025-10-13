# Phân Tích Kiến Trúc Call API - CareNest Service Category

## Tổng Quan Kiến Trúc

Dự án CareNest Service Category sử dụng kiến trúc **Clean Architecture** với **CQRS Pattern** và **Repository Pattern**. Đây là một microservice quản lý danh mục dịch vụ trong hệ thống CareNest.

## Cấu Trúc Layer

### 1. API Layer (CareNest_Service_Category.API)
- **Controller**: `ServiceCategoryController` - Entry point cho các API endpoints
- **Middleware**: `GlobalExceptionHandlingMiddleware` - Xử lý exception toàn cục
- **Extensions**: `ControllerResponseExtensions` - Chuẩn hóa response format

### 2. Application Layer (CareNest_Service_Category.Application)
- **CQRS Pattern**: Tách biệt Commands và Queries
- **Use Cases**: `UseCaseDispatcher` - Điều phối các use case
- **Interfaces**: Định nghĩa contracts cho services và repositories

### 3. Domain Layer (CareNest_Service_Category.Domain)
- **Entities**: `ServiceCategory` - Domain model
- **Repositories**: `IGenericRepository` - Repository interface

### 4. Infrastructure Layer (CareNest_Service_Category.Infrastructure)
- **Services**: `APIService`, `Service`, `ShopService` - External API calls
- **Persistence**: Database context và repositories
- **ApiEndpoints**: Định nghĩa endpoints cho external services

## Luồng Call API

### 1. Internal API Flow (Trong cùng microservice)

```
Client Request → Controller → UseCaseDispatcher → Query/Command Handler → Repository → Database
```

**Ví dụ**: GET /api/servicecategory
1. `ServiceCategoryController.GetPaging()` nhận request
2. Tạo `GetAllPagingQuery` object
3. `UseCaseDispatcher.DispatchQueryAsync()` điều phối
4. `GetAllPagingQueryHandler` xử lý business logic
5. Gọi `IUnitOfWork.GetRepository<ServiceCategory>().FindAsync()`
6. Trả về `PageResult<ServiceResponse>`

### 2. External API Flow (Gọi microservice khác)

```
Internal Service → APIService → HttpClient → External Microservice
```

**Ví dụ**: Lấy thông tin shop từ Shop Service
1. `GetAllPagingQueryHandler` cần thông tin shop
2. Gọi `IShopService.GetShopById(shopId)`
3. `ShopService` sử dụng `IAPIService.GetAsync()`
4. `APIService` gửi HTTP request đến Shop microservice
5. Parse response và trả về `ResponseResult<T>`

## Các Thành Phần Chính

### 1. UseCaseDispatcher
```csharp
public class UseCaseDispatcher : IUseCaseDispatcher
{
    // Điều phối Commands
    public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command)
    
    // Điều phối Queries  
    public async Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery query)
}
```

### 2. APIService
```csharp
public class APIService : IAPIService
{
    // GET request
    public async Task<ResponseResult<T>> GetAsync<T>(string serviceType, string endpoint)
    
    // POST request
    public async Task<ResponseResult<T>> PostAsync<T>(string url, object data)
    
    // PUT request
    public async Task<ResponseResult<T>> PutAsync<T>(string url, object data)
    
    // DELETE request
    public async Task<ResponseResult<T>> DeleteAsync<T>(string url)
}
```

### 3. Response Format
```csharp
// API Response wrapper
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}

// Internal service response
public class ResponseResult<T>
{
    public bool IsSuccess { get; set; }
    public ApiResponse<T>? Data { get; set; }
    public string? Message { get; set; }
    public int? ErrorCode { get; set; }
}
```

## Đặc Điểm Kiến Trúc

### 1. CQRS Pattern
- **Commands**: Create, Update, Delete operations
- **Queries**: Read operations (GetAllPaging, GetById)
- Tách biệt rõ ràng giữa read và write operations

### 2. Dependency Injection
- Tất cả dependencies được đăng ký trong `Program.cs`
- Sử dụng interface để loose coupling
- Scoped lifetime cho most services

### 3. Error Handling
- `GlobalExceptionHandlingMiddleware` xử lý tất cả exceptions
- Custom exceptions: `BadRequestException`, `InternalException`
- Standardized error response format

### 4. External Service Integration
- `APIService` với HttpClient để gọi external APIs
- Configuration-based service URLs
- Retry mechanism và error handling

### 5. Database Integration
- Entity Framework Core với PostgreSQL
- Repository pattern với Unit of Work
- Migration support

## API Endpoints

### ServiceCategoryController
- `GET /api/servicecategory` - Lấy danh sách có phân trang
- `GET /api/servicecategory/{id}` - Lấy chi tiết theo ID
- `POST /api/servicecategory` - Tạo mới
- `PUT /api/servicecategory/{id}` - Cập nhật
- `DELETE /api/servicecategory/{id}` - Xóa

## Configuration

### APIServiceOption
```json
{
  "APIService": {
    "BaseUrlService": "https://service-api-url",
    "BaseUrlShop": "https://shop-api-url"
  }
}
```

### DatabaseSettings
```json
{
  "DatabaseSettings": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "service-category-dev",
    "Username": "username",
    "Password": "password"
  }
}
```

## Ưu Điểm Kiến Trúc

1. **Separation of Concerns**: Tách biệt rõ ràng các layer
2. **Testability**: Dễ dàng unit test với dependency injection
3. **Scalability**: CQRS pattern hỗ trợ scaling read/write operations
4. **Maintainability**: Code dễ maintain và extend
5. **Error Handling**: Centralized exception handling
6. **External Integration**: Clean abstraction cho external API calls

## Điểm Cần Cải Thiện

1. **Caching**: Chưa có caching mechanism cho external API calls
2. **Circuit Breaker**: Chưa có circuit breaker pattern cho external services
3. **Logging**: Có thể cần structured logging
4. **Validation**: Có thể cần thêm validation layer
5. **Rate Limiting**: Chưa có rate limiting cho external API calls
