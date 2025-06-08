using System.ComponentModel;

namespace finrv.Application.Enums;

public enum EPositionFilters
{
    [Description("Nenhum filtro aplicado")]
    None,
    [Description("Filtra ou ordena por valor total de corretagens pagas.")]
    BrokerageFees,
    [Description("Filtra ou ordena por tamanho da posição (quantidade de ativos).")]
    PositionSize,
    [Description("Filtra ou ordena por valor total da posição.")]
    PositionValue
}