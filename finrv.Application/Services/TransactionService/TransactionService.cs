using finrv.Application.Interfaces;
using finrv.Application.Services.TransactionService.Dtos;
using finrv.Infra;
using finrv.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace finrv.Application.Services.TransactionService;

public class TransactionService(
    ILogger<TransactionService> logger,
    InvestimentDbContext context,
    RequestInfo requestInfo
    ) : ITransactionService
{
    private const string CLASS_NAME = nameof(TransactionService);
    
    public async Task<BrokerageFeesEarningsResponseDto> GetBrokerageFeesEarningsAsync()
    {
        logger.LogInformation("Starting | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(GetBrokerageFeesEarningsAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        var totalEarnings = await context.Transaction
            .GroupBy(t => 1)
            .Select(t => new BrokerageFeesEarningsResponseDto(
                t.LongCount(),
                t.Sum(v => ((v.UnitPrice * v.PositionSize) * v.BrokerageValue) / 100)
            ))
            .FirstOrDefaultAsync();
        
        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(GetBrokerageFeesEarningsAsync), requestInfo.CorrelationId, requestInfo.ClientType);

        return totalEarnings ?? new BrokerageFeesEarningsResponseDto(0, 0m);
    }
}