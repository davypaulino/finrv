using finrv.ApiService.Application;
using finrv.Application.Interfaces;
using finrv.Application.Services.UserService.Dtos;
using finrv.Domain.Entities;
using finrv.Infra;
using finrv.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace finrv.Application.Services.UserService;

public class UserService(
    InvestimentDbContext context,
    ILogger<UserService> logger,
    RequestInfo requestInfo) : IUserService
{
    private const string CLASS_NAME = nameof(UserService);
    
    public async Task<Pagination<object, AllUsersResponseDto>> AllUsersAsync(AllUsersRequestDto request)
    {
        logger.LogInformation("Starting | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(AllUsersAsync), requestInfo.CorrelationId, requestInfo.ClientType);
        
        var query = context.User.AsQueryable();

        var totalItems = await query.CountAsync();
        
        var users = await query
            .Skip((int)((request.Page - 1) * request.PageSize))
            .Take((int)request.PageSize)
            .Select(u => new AllUsersResponseDto(u.Id.ToString(), u.Name, u.Email))
            .ToListAsync();
        
        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(AllUsersAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        return new Pagination<object, AllUsersResponseDto>(users, request.Page, request.PageSize, (uint)totalItems, null);
    }

    public async Task<Pagination<object, AssetsAveragePriceResponseDto>> AveragePriceOfAssetsByUserAsync(
        Guid userId, AssetsAveragePriceRequestDto query)
    {
        logger.LogInformation("Starting | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(AveragePriceOfAssetsByUserAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        var hasTickersFilter = query.Tickers?.FirstOrDefault();
        
        var q = context.Position.AsQueryable()
            .Where(p => p.UserId == userId);
        
        var totalItems = await q.CountAsync();
        
        if (!String.IsNullOrEmpty(hasTickersFilter))
            q = q.Where(p => query.Tickers!.Contains(p.Asset.Ticker));
        
        var result = await q
            .OrderBy(p => p.Asset.Ticker)
            .Skip((int)((query.Page - 1) * query.PageSize))
            .Take((int)query.PageSize)
            .Select(p => new AssetsAveragePriceResponseDto(
                p.Asset.Ticker,
                p.AveragePrice,
                p.UpdatedAt ?? p.CreatedAt))
            .ToListAsync();

        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(AveragePriceOfAssetsByUserAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        return new Pagination<object, AssetsAveragePriceResponseDto>(result, query.Page, query.PageSize, (uint)totalItems, null);
    }
    
}