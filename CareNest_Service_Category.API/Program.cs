using CareNest.Domain.Entitites;
using CareNest_Service_Category.API.Middleware;
using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Common.Options;
using CareNest_Service_Category.Application.Features.Commands.Create;
using CareNest_Service_Category.Application.Features.Commands.Delete;
using CareNest_Service_Category.Application.Features.Commands.Update;
using CareNest_Service_Category.Application.Features.Queries.GetAllPaging;
using CareNest_Service_Category.Application.Features.Queries.GetById;
using CareNest_Service_Category.Application.Features.Queries.GetByShopId;
using CareNest_Service_Category.Application.Features.Queries.GetServicesByCategories;
using CareNest_Service_Category.Application.Interfaces.CQRS;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;
using CareNest_Service_Category.Application.Interfaces.Services;
using CareNest_Service_Category.Application.Interfaces.UOW;
using CareNest_Service_Category.Application.UseCases;
using CareNest_Service_Category.Domain.Repositories;
using CareNest_Service_Category.Infrastructure.Persistences.Configuration;
using CareNest_Service_Category.Infrastructure.Persistences.Database;
using CareNest_Service_Category.Infrastructure.Persistences.Repository;
using CareNest_Service_Category.Infrastructure.Services;
using CareNest_Service_Category.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Lấy DatabaseSettings từ configuration (ưu tiên lấy env - dùng cho cloud build Koyeb)
var config = builder.Configuration;
DatabaseSettings dbSettings = new DatabaseSettings
{
    Ip = config["DB_HOST"] ?? config["DatabaseSettings:Ip"],
    Port = int.TryParse(config["DB_PORT"], out var port) ? port : (config.GetSection("DatabaseSettings").GetValue<int?>("Port") ?? 5432),
    User = config["DB_USER"] ?? config["DatabaseSettings:User"],
    Password = config["DB_PASSWORD"] ?? config["DatabaseSettings:Password"],
    Database = config["DB_NAME"] ?? config["DatabaseSettings:Database"]
};
dbSettings.Display();
string connectionString = dbSettings.GetConnectionString() ?? "Host=localhost;Port=5432;Database=service-category-dev;Username=exe-carenest-dev;Password=nghi123";


// Đăng ký DbContext với PostgreSQL - chuẩn cloud Koyeb
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(
        connectionString + ";Pooling=true;Maximum Pool Size=5;Minimum Pool Size=0;Timeout=15;",
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
            // npgsqlOptions.CommandTimeout(60); // Nếu muốn có thể enable thêm
        }));

builder.Services.AddTransient<DatabaseSeeder>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Đăng ký service thêm chú thích cho api
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    //ADD JWT BEARER SECURITY DEFINITION
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        //Type = SecuritySchemeType.ApiKey,
        Type = SecuritySchemeType.Http,//ko cần thêm token phía trước
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            },
            new List<string>()
        }
    });
});
// Đăng ký cấu hình APIServiceOption
builder.Services.Configure<APIServiceOption>(
    builder.Configuration.GetSection("APIService")
);
// Đăng ký các repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//command
builder.Services.AddScoped<ICommandHandler<CreateCommand, ServiceCategory>, CreateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCommand, ServiceCategory>, UpdateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteCommand>, DeleteCommandHandler>();
//query
builder.Services.AddScoped<IQueryHandler<GetAllPagingQuery, PageResult<ServiceResponse>>, GetAllPagingQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetByIdQuery, ServiceResponse>, GetByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetByShopIdQuery, List<ServiceResponse>>, GetByShopIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetServicesByCategoriesQuery, List<ServiceByCategoryResponse>>, GetServicesByCategoriesQueryHandler>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// Add CORS policies: AllowAll cho dev, AllowTrusted cho prod
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    // Production: chỉ allow origin cụ thể, không credentials
    options.AddPolicy("AllowTrusted",
        builder =>
        {
            builder
                .WithOrigins("https://your-production-domain.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


//Đăng ký lấy thông tin từ token
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IShopService, ShopService>();


builder.Services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
// => Dùng biến flag thay vì chỉ kiểm tra IsDevelopment()
var swaggerEnabled = app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled");
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Chỉ migrate khi RUN_MIGRATIONS=true (chuẩn cloud)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
    if (!string.IsNullOrWhiteSpace(runMigrations) && runMigrations.Equals("true", StringComparison.OrdinalIgnoreCase))
    {
        context.Database.Migrate();
    }
}
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("AllowTrusted");
}
app.UseAuthorization();
app.MapControllers();

app.Run();