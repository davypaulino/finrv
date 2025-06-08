using System.ComponentModel.DataAnnotations;
using finrv.ApiService.Application;

namespace finrv.Application.Services.UserService.Dtos;

public record AssetsAveragePriceRequestDto(
    string[]? Tickers = null) : PaginationQuery;