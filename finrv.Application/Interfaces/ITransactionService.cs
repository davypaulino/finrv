using finrv.Application.Services.TransactionService.Dtos;

namespace finrv.Application.Interfaces;

public interface ITransactionService
{
    Task<BrokerageFeesEarningsResponseDto> GetBrokerageFeesEarningsAsync();
}