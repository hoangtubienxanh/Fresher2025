using Microsoft.AspNetCore.Mvc;

namespace RookiesWebApp.Models;

public record PaginationRequest(
    [FromQuery(Name = "pageSize")] int PageSize = 3,
    [FromQuery(Name = "pageIndex")] int PageIndex = 0
);