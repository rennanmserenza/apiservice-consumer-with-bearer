namespace apiservice_consumer_with_bearer.Client.ApiClient.Interfaces;

/// <summary>
/// Interface responsável por executar requisições HTTP POST com suporte a envio de conteúdo e autenticação.
/// </summary>
public interface IPostApiClient
{
    /// <summary>
    /// Envia uma requisição HTTP POST para o endpoint especificado, com o corpo da requisição em formato JSON.
    /// </summary>
    /// <param name="path">O endpoint (URL relativa) para o qual a requisição POST será enviada.</param>
    /// <param name="content">O conteúdo a ser enviado no corpo da requisição, no formato <see cref="StringContent"/> (geralmente JSON).</param>
    /// <param name="token">Token de autenticação opcional. Se fornecido, será incluído no cabeçalho da requisição.</param>
    /// <returns>
    /// Um <see cref="Task{TResult}"/> que representa a operação assíncrona. O resultado será o corpo da resposta como uma <see cref="string"/>, ou <c>null</c> em caso de falha.
    /// </returns>
    Task<string?> PostAsync(string path, string? content, TimeSpan? timeout = null, string? token = null);

    Task<HttpResponseMessage?> PostRawAsync(string path, string? content, TimeSpan? timeout = null, string? token = null);
}
