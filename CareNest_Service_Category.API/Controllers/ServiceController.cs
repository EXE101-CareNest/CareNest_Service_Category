using CareNest_Service_Category.API.Extensions;
using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Features.Queries.GetAllPaging;
using CareNest_Service_Category.Application.Features.Queries.GetServicesByCategories;
using CareNest_Service_Category.Application.Interfaces.CQRS;
using CareNest_Service_Category.Domain.Commons.Constant;
using Microsoft.AspNetCore.Mvc;

namespace CareNest_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public ServiceController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Lấy danh sách service theo danh sách categoryId
        /// </summary>
        /// <param name="categoryIds">Danh sách categoryId</param>
        /// <returns>Danh sách service</returns>
        [HttpPost("by-categories")]
        public async Task<IActionResult> GetServicesByCategories([FromBody] List<string> categoryIds)
        {
            var query = new GetServicesByCategoriesQuery { CategoryIds = categoryIds };
            List<ServiceByCategoryResponse> result = await _dispatcher.DispatchQueryAsync<GetServicesByCategoriesQuery, List<ServiceByCategoryResponse>>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }
    }
}


