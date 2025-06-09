using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using finrv.Application.Enums;

namespace finrv.Application.Services.UserService.Dtos;

public record UserPositionsRequestDto(
    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que 0.")]
    [DefaultValue(1)]
    uint Page = 1,
    [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
    [DefaultValue(10)]
    uint PageSize = 10,
    [DefaultValue(EOrderBy.Asc)]
    [Description("Options: 0 - ASC | 1 - DESC")]
    EOrderBy OrderBy = EOrderBy.Asc,
    [DefaultValue(EPositionFilters.BrokerageFees)]
    [Description("Options: 0 - None | 1 - Brokerage | 2 - PositionSize | 3 - PositionValue")]
    EPositionFilters Filter = EPositionFilters.None);