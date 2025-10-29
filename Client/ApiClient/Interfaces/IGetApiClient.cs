namespace apiservice_consumer_with_bearer.Client.ApiClient.Interfaces;

/// <summary>
/// Interface para realizar requisições HTTP GET.
/// </summary>
public interface IGetApiClient
{
    /// <summary>
    /// Realiza uma requisição HTTP GET para o caminho especificado.
    /// </summary>
    /// <param name="path">O caminho (ou endpoint) da requisição GET.</param>
    /// <param name="token">O token de autorização opcional. Se fornecido, será incluído no cabeçalho de autorização. Caso contrário, a requisição será feita sem um token.</param>
    /// <returns>
    /// Uma <see cref="Task"/> que representa a operação assíncrona. O resultado será o corpo da resposta como uma string, ou <c>null</c> em caso de erro.
    /// </returns>
    Task<string?> GetAsync(string path, string? token = null);
}
