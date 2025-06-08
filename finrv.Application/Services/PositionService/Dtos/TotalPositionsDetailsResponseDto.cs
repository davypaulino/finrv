namespace finrv.Application.Services.PositionService.Dtos;

public record TotalPositionsDetailsResponseDto(
    long NumberOfClients,
    decimal TotalBrokerageFees,
    long TotalPositionSize,
    decimal TotalPositionValue);