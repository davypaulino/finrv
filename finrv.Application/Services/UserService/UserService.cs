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

    public async Task<Pagination<TotalUserPositionResponseDto, UserTotalPositionsResponseDto>>
        GetUserPositionsByIdAsync(Guid userId, UserPositionsRequestDto request)
    {
        logger.LogInformation("Starting | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(GetUserPositionsByIdAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        var positionsForUserQuery = context.Position
            .Where(p => p.UserId == userId && p.PositionSize > 0)
            .Include(p => p.Asset)
            .AsNoTracking();
        
        var assetIdsInUserPositions = await positionsForUserQuery
            .Select(p => p.AssetId)
            .Distinct()
            .ToListAsync();
        
        var latestQuotations = await context.Quotation
            .Where(q => assetIdsInUserPositions.Contains(q.AssetId))
            .AsNoTracking()
            .GroupBy(q => q.AssetId)
            .Select(group => group.OrderByDescending(q => q.UpdatedAt ?? q.CreatedAt).First())
            .ToDictionaryAsync(
                q => q.AssetId,
                q => new
                {
                    QuotationPrice = q.Price,
                    QuotationTimestamp = q.UpdatedAt ?? q.CreatedAt,
                });
        
        var positionsInMemory = await positionsForUserQuery.ToListAsync();

        var records = positionsInMemory
            .Select(position =>
            {
                latestQuotations.TryGetValue(position.AssetId, out var quotation);
                decimal marketValue = quotation is not null ? position.PositionSize * quotation.QuotationPrice : 0m;
                decimal positionCostValue = position.PositionSize * position.AveragePrice;

                return new UserTotalPositionsResponseDto
                {
                    Ticker = position.Asset.Ticker,
                    Name = position.Asset.Name,
                    PositionSize = position.PositionSize,
                    PositionCostValue = positionCostValue,
                    AverageCostValue = position.AveragePrice,
                    MarketValue = marketValue,
                    ProfitAndLoss = marketValue - positionCostValue,
                    LastUpdate = position.UpdatedAt ?? position.CreatedAt ?? DateTime.Now
                };
            })
            .ToList();

        records = OrderPositionsByFilter(records, request);
        
        var totalItems = records.Count;
        long totalPositionSizeSum = records.Sum(f => (long)f.PositionSize);
        decimal totalPositionCostValue = records.Sum(f => f.PositionCostValue);
        decimal totalMarketValue = records.Sum(f => f.MarketValue);
        decimal totalProfitAndLoss = records.Sum(f => f.ProfitAndLoss);

        decimal totalAverageCostValue = 0m;
        if (totalPositionSizeSum > 0)
        {
            totalAverageCostValue = Math.Round(totalPositionCostValue / totalPositionSizeSum, 2, MidpointRounding.AwayFromZero);
        }

        var totalDetails = new TotalUserPositionResponseDto(
            TotalPositionSize: (uint)totalPositionSizeSum,
            TotalPositionCost: totalPositionCostValue,
            TotalAveragePrice: totalAverageCostValue,
            TotalProfitAndLoss: totalProfitAndLoss,
            TotalMarketValue: totalMarketValue
        );
        
        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(GetUserPositionsByIdAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        return new Pagination<TotalUserPositionResponseDto, UserTotalPositionsResponseDto>(
            records,
            request.Page, request.PageSize, (uint)totalItems,
            totalDetails);
    }
    
    private static List<UserTotalPositionsResponseDto> OrderPositionsByFilter(
        List<UserTotalPositionsResponseDto> records,
       UserPositionsRequestDto request) => (records, request, request.Filter) switch
    {
        (_, _, EPositionFilters.PositionSize) => 
            request.OrderBy == EOrderBy.Desc ?
                records.OrderByDescending(o => o.PositionSize).ToList()
                : records.OrderBy(o => o.PositionSize).ToList(),
        (_, _, EPositionFilters.PositionValue) =>
            request.OrderBy == EOrderBy.Desc ?
                records.OrderByDescending(o => o.PositionCostValue).ToList()
                : records.OrderBy(o => o.PositionCostValue).ToList(),
        (_, _, _) =>
            request.OrderBy == EOrderBy.Desc ?
                records.OrderByDescending(o => o.Ticker).ToList()
                : records.OrderBy(o => o.Ticker).ToList(),
    };
    
}