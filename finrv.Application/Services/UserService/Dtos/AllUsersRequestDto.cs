using System.ComponentModel.DataAnnotations;
using finrv.Application.Enums;

namespace finrv.Application.Services.UserService.Dtos;

public record AllUsersRequestDto(
    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que 0.")]
    uint Page,
    [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
    uint PageSize,
    EOrderBy OrderBy = EOrderBy.Asc);