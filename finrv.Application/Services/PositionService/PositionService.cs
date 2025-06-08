using finrv.ApiService.Application;
using finrv.Application.Enums;
using finrv.Application.Interfaces;
using finrv.Application.Services.PositionService.Dtos;
using finrv.Domain.Entities;
using finrv.Domain.Enums;
using finrv.Infra;
using finrv.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZstdSharp.Unsafe;

namespace finrv.Application.Services.PositionService;

public class PositionService(
    ILogger<PositionService> logger,
    InvestimentDbContext context,
    RequestInfo requestInfo) : IPositionService
{
    private const string CLASS_NAME = nameof(PositionService);

    public async Task<Pagination<TotalPositionsDetailsResponseDto, UsersPositionsResponseDto>>
        GetClientsPositionsAsync(UsersPositionsRequestDto request)
    {
        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(GetClientsPositionsAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        var query = context.Transaction
            .Include(t => t.User)
            .AsQueryable();

        var totalDetails = await GetTotalPositionsDetails(query);
        var groupedTransactions = GroupTransactionsByUserId(query);
        var totalItems = await groupedTransactions.CountAsync();
        groupedTransactions = OrderTransactionsByFilter(groupedTransactions, request);
        var records = await groupedTransactions
            .Skip((int)((request.Page - 1) * request.PageSize))
            .Take((int)(request.PageSize))
            .ToListAsync();
            
        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(GetClientsPositionsAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        return new Pagination<TotalPositionsDetailsResponseDto, UsersPositionsResponseDto>(
            records, request.Page, request.PageSize, (uint)Math.Abs(totalItems), totalDetails); 
    }

    private static async Task<TotalPositionsDetailsResponseDto?> GetTotalPositionsDetails(IQueryable<TransactionEntity> query)
    {
        var totalDetails = await query.GroupBy(p => 1)
            .Select(g => new TotalPositionsDetailsResponseDto(
                g.Select(t => t.UserId).Distinct().LongCount(),
                g. Sum(t => ((t.UnitPrice * t.PositionSize) * t.BrokerageValue) / 100),
                g.Sum(t => t.Type == ETransactionType.Buy ? t.PositionSize : 0),
                g.Sum(t => t.Type == ETransactionType.Buy ? t.UnitPrice * t.PositionSize : 0m)
            )).FirstOrDefaultAsync();
        return totalDetails;
    }
    
    private static IQueryable<UsersPositionsResponseDto> GroupTransactionsByUserId(IQueryable<TransactionEntity> query)
    {
        return query.GroupBy(t => new { t.UserId, t.User.Name, t.User.Email })
            .Select(g => new UsersPositionsResponseDto{
                UserId = g.Key.UserId,
                Name = g.Key.Name,
                Email = g.Key.Email,
                TotalPositionSize = g.Sum(t => t.Type == ETransactionType.Buy ? t.PositionSize : 0),
                TotalPositionValue = g.Sum(t => t.Type == ETransactionType.Buy ? t.UnitPrice * t.PositionSize : 0),
                TotalBrokeragePaid = g.Sum(t => ((t.UnitPrice * t.PositionSize) * t.BrokerageValue) / 100)
            });
    }
    
    private static IQueryable<UsersPositionsResponseDto> OrderTransactionsByFilter(
        IQueryable<UsersPositionsResponseDto> query,
        UsersPositionsRequestDto request) => (query, request, request.Filter) switch
    {
        (_, _, EPositionFilters.PositionSize) => 
            request.OrderBy == EOrderBy.Desc ?
                query.OrderByDescending(o => o.TotalPositionSize)
                : query.OrderBy(o => o.TotalPositionSize),
        (_, _, EPositionFilters.PositionValue) =>
            request.OrderBy == EOrderBy.Desc ?
                query.OrderByDescending(o => o.TotalPositionValue)
                : query.OrderBy(o => o.TotalPositionValue),
        (_, _, EPositionFilters.BrokerageFees) =>
            request.OrderBy == EOrderBy.Desc ?
                query.OrderByDescending(o => o.TotalBrokeragePaid)
                : query.OrderByDescending(o => o.TotalBrokeragePaid),
        (_, _, _) =>
            request.OrderBy == EOrderBy.Desc ?
                query.OrderByDescending(o => o.Name)
                : query.OrderBy(o => o.Name),
    };
}