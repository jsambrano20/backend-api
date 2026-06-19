using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data, string message = "") =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = true, Message = message });

    protected IActionResult Ok(string message) =>
            base.Ok(new ApiResponse { Message = message, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data, string message = "") =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true, Message = message });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            OkPaginated(pagedList, pagedList.CurrentPage, pagedList.TotalPages, pagedList.TotalCount);

    protected IActionResult OkPaginated<T>(IEnumerable<T> data, int currentPage, int totalPages, int totalCount) =>
            base.Ok(new PaginatedResponse<T>
            {
                Data = data,
                CurrentPage = currentPage,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Success = true
            });
}
