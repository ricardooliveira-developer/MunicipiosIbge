namespace MunicipiosIbge.Api.Common.Responses;

/// <summary>
/// Envelope padrão usado nas respostas de sucesso da API.
/// </summary>
/// <typeparam name="T">Tipo do objeto retornado no campo data.</typeparam>
/// <param name="Success">Indica se a operação foi concluída com sucesso.</param>
/// <param name="Data">Dados retornados pela operação.</param>
/// <param name="Message">Mensagem opcional de contexto.</param>
public sealed record ApiResponse<T>(bool Success, T? Data, string? Message = null)
{
    public static ApiResponse<T> Ok(T data, string? message = null)
    {
        return new ApiResponse<T>(true, data, message);
    }
}
