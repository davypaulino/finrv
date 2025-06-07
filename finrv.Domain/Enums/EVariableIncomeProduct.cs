namespace finrv.Domain.Enums;

public enum EVariableIncomeProduct
{
    /// <summary>
    /// Representa ações de empresas negociadas em bolsa.
    /// </summary>
    Acoes = 1,

    /// <summary>
    /// Fundos de investimento focados no setor imobiliário.
    /// </summary>
    FundosImobiliarios, // FIIs

    /// <summary>
    /// Fundos de índice negociados em bolsa, que replicam o desempenho de um índice.
    /// </summary>
    ETFs, // Exchange Traded Funds

    /// <summary>
    /// Recibos de ações de empresas estrangeiras negociados na bolsa brasileira.
    /// </summary>
    BDRs, // Brazilian Depositary Receipts

    /// <summary>
    /// Instrumentos derivativos que dão o direito de comprar ou vender um ativo no futuro.
    /// </summary>
    Opcoes,

    /// <summary>
    /// Contratos para comprar ou vender um ativo em uma data futura a um preço predeterminado.
    /// </summary>
    ContratosFuturos,

    /// <summary>
    /// Ativos digitais descentralizados baseados em criptografia.
    /// </summary>
    Criptomoedas,

    /// <summary>
    /// Fundos de investimento que aplicam a maior parte de seu patrimônio em ações.
    /// </summary>
    FundosDeAcoes,

    /// <summary>
    /// Fundos de investimento que podem aplicar em diversas classes de ativos, incluindo renda variável.
    /// </summary>
    FundosMultimercado, // Com exposição relevante a RV

    /// <summary>
    /// Produtos estruturados que combinam elementos de renda fixa e renda variável.
    /// </summary>
    COEs, // Certificados de Operações Estruturadas

    /// <summary>
    /// Negociação de ativos com liquidação imediata ou em curtíssimo prazo.
    /// </summary>
    MercadoAVista,

    /// <summary>
    /// Negociação de ativos com liquidação em uma data futura pré-acordada.
    /// </summary>
    MercadoATermo,

    /// <summary>
    /// Ofertas iniciais de ações de uma empresa no mercado de capitais.
    /// </summary>
    OfertasPublicasIniciais, // IPOs

    /// <summary>
    /// Novas ofertas de ações de empresas que já possuem capital aberto.
    /// </summary>
    OfertasSubsequentes // Follow-ons
}