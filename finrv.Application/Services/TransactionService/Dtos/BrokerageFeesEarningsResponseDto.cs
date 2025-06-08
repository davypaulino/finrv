namespace finrv.Application.Services.TransactionService.Dtos;

public record BrokerageFeesEarningsResponseDto(
    long NumberOfTransactions,
    decimal TotalFessEarnings);