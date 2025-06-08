using System.ComponentModel.DataAnnotations;

namespace finrv.ApiService.Application;

public record PaginationQuery(
    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que 0.")]
    uint Page = 1,
    [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
    uint PageSize = 10);