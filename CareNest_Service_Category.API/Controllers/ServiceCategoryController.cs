using CareNest.Domain.Entitites;
using CareNest_Service_Category.API.Extensions;
using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Features.Commands.Create;
using CareNest_Service_Category.Application.Features.Commands.Delete;
using CareNest_Service_Category.Application.Features.Commands.Update;
using CareNest_Service_Category.Application.Features.Queries.GetAllPaging;
using CareNest_Service_Category.Application.Features.Queries.GetById;
using CareNest_Service_Category.Application.Interfaces.CQRS;
using CareNest_Service_Category.Domain.Commons.Constant;
using Microsoft.AspNetCore.Mvc;


namespace CareNest_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public ServiceCategoryController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Hiển thị toàn bộ danh sách loại dịch vụ hiện có trong hệ thống với phân trang và sắp xếp
        /// </summary>
        /// <param name="pageIndex">trang hiện tại</param>
        /// <param name="pageSize">Số lượng phần tử trong trang</param>
        /// <param name="sortColumn">cột muốn sort: name, updateat,ownerid</param>
        /// <param name="sortDirection">cách sort asc or desc</param>
        /// <param name="shopId">Lọc theo ShopId</param>
        /// <returns>Danh sách dịch vụ</returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? shopId = null)
        {
            var query = new GetAllPagingQuery()
            {
                Index = pageIndex,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                ShopId = shopId
            };
            var result = await _dispatcher.DispatchQueryAsync<GetAllPagingQuery, PageResult<ServiceResponse>>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// Hiển thị chi tiết loại dịch vụ theo id
        /// </summary>
        /// <param name="id">Id dịch vụ</param>
        /// <returns>chi tiết dịch vụ</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetByIdQuery() { Id = id };
            ServiceCategory result = await _dispatcher.DispatchQueryAsync<GetByIdQuery, ServiceCategory>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// tạo mới loại dịch vụ
        /// </summary>
        /// <param name="command">thông tin dịch vụ</param>
        /// <returns>thông tin dịch vụ mới tạo xog</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommand command)
        {
            ServiceCategory result = await _dispatcher.DispatchAsync<CreateCommand, ServiceCategory>(command);

            return this.OkResponse(result, MessageConstant.SuccessCreate);
        }

        /// <summary>
        /// Cập nhật thông tin dịch vụ
        /// </summary>
        /// <param name="id">Id dịch vụ</param>
        /// <param name="request">các thông tin cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRequest request)
        {

            var command = new UpdateCommand()
            {
                Id = id,
                Name = request.Name,
                ShopId = request.ShopId
            };
            ServiceCategory result = await _dispatcher.DispatchAsync<UpdateCommand, ServiceCategory>(command);

            return this.OkResponse(result, MessageConstant.SuccessUpdate);
        }

        /// <summary>
        /// xoá loại dịch vụ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _dispatcher.DispatchAsync(new DeleteCommand { Id = id });
            return this.OkResponse(MessageConstant.SuccessDelete);
        }
    }
}
