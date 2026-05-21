namespace MunicipiosIbge.Api.Common.Responses;

/// <summary>
/// Resposta padrão para erros tratados pela API.
/// </summary>
/// <param name="Success">Sempre falso quando a requisição falha.</param>
/// <param name="Message">Mensagem amigável explicando o motivo da falha.</param>
public sealed record ErrorResponse(bool Success, string Message);
