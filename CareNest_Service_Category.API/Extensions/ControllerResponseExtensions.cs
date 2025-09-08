using CareNest_Service_Category.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CareNest_Service_Category.API.Extensions
{
    public static class ControllerResponseExtensions
    {
        public static IActionResult OkResponse<T>(this ControllerBase controller, T data, string? message = null)
        {
            return controller.Ok(ApiResponse<T>.SuccessResponse(data, message));
        }

        public static IActionResult ErrorResponse<T>(this ControllerBase controller, string message)
        {
            return controller.BadRequest(ApiResponse<T>.Failure(message));
        }
    }
}
