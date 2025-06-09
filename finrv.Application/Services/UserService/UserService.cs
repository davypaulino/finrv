using finrv.ApiService.Application;
using finrv.Application.Enums;
using finrv.Application.Interfaces;
using finrv.Application.Services.PositionService.Dtos;
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
        
        query = request.OrderBy == EOrderBy.Desc ?
            query.OrderByDescending(q => q.Name)
            : query.OrderBy(q => q.Name);
        
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
        
        q = query.OrderBy == EOrderBy.Desc ? 
            q.OrderByDescending(p => p.Asset.Ticker)
            : q.OrderBy(p => p.Asset.Ticker);
        
        var result = await q
            .Skip((int)((query.Page - 1) * query.PageSize))
            .Take((int)query.PageSize)
            .Select(p => new AssetsAveragePriceResponseDto(
                p.Asset.Ticker,
                p.AveragePrice,
                p.UpdatedAt ?? p.CreatedAt ?? DateTime.Now))
            .ToListAsync();

        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(AveragePriceOfAssetsByUserAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        return new Pagination<object, AssetsAveragePriceResponseDto>(result, query.Page, query.PageSize, (uint)totalItems, null);
    }

    public async Task<Pagination<TotalUserPositionResponseDto, UserPositionsResponseDto>>
        GetUserPositionsByIdAsync(Guid userId, UsersPositionsRequestDto request)
    {
        await Task.CompletedTask;
        var records = new List<UserPositionsResponseDto>();
        return new Pagination<TotalUserPositionResponseDto, UserPositionsResponseDto>(
            records,
            request.Page, request.PageSize, 0,
            new TotalUserPositionResponseDto(
                0m, 0, 0m, 0m, 0, 0));
    }
    
}