using apiservice_consumer_with_bearer.Client.ApiClient.Interfaces;
using apiservice_consumer_with_bearer.Client.Http;

namespace apiservice_consumer_with_bearer.Client.ApiClient;

/// <summary>
/// Implementação do cliente para realizar requisições HTTP com suporte a GET, POST e DELETE.
/// </summary>
/// <remarks>
/// Esta classe utiliza um <see cref="HttpClient"/> para enviar requisições HTTP, uma política de retry para lidar com falhas temporárias,
/// e um manipulador de cabeçalhos de autorização. Os métodos fornecidos permitem enviar requisições GET, POST e DELETE para um endpoint
/// específico, com suporte opcional a autenticação via token.
/// </remarks>
/// <remarks>
/// Constrói uma instância do cliente para realizar requisições HTTP com suporte a políticas de repetição (retry) e construção de requisições.
/// </remarks>
/// <param name="httpRequestSender">Instância de <see cref="IHttpRequestSender"/> responsável por enviar requisições HTTP.</param>
/// <param name="retryPolicy">Instância de <see cref="IRetryPolicy"/> para gerenciar tentativas de repetição em caso de falhas temporárias nas requisições.</param>
/// <param name="requestBuilder">Instância de <see cref="IRequestBuilder"/> para construir e configurar requisições HTTP, incluindo cabeçalhos de autorização e conteúdo.</param>
/// <remarks>
/// Este construtor inicializa o cliente HTTP com as dependências necessárias para enviar requisições com suporte a retry e
/// construção personalizada das requisições, incluindo autenticação via token quando necessário.
/// </remarks>
public class ApiClient(
    IHttpRequestSender httpRequestSender,
    IRetryPolicy retryPolicy,
    IRequestBuilder requestBuilder)
    : IApiClient
{
    public Task<string?> GetAsync(string path, string? token = null)
        => SendAsync(HttpMethod.Get, path, null, null, token);

    public Task<string?> PostAsync(string path, string? content, TimeSpan? timeout = null, string? token = null)
        => SendAsync(HttpMethod.Post, path, content, timeout, token);

    public Task<HttpResponseMessage?> PostRawAsync(string path, string? content, TimeSpan? timeout = null, string? token = null)
        => SendRawAsync(HttpMethod.Post, path, content, timeout, token);

    public Task<string?> DeleteAsync(string path, string? content, string? token = null)
        => SendAsync(HttpMethod.Delete, path, content, null, token);

    /// <summary>
    /// Envia uma requisição HTTP, com ou sem tempo limite definido.
    /// </summary>
    private async Task<HttpResponseMessage> BuildAndSendRequestAsync
        (HttpMethod method, string path, string? content, string? token, TimeSpan? timeout = null)
    {
        // Utilizando RequestBuilder para construir a requisição
        var request = requestBuilder.BuildRequest(method, path, content, token);

        // Envio da requisição
        return await httpRequestSender.SendAsync(request, timeout).ConfigureAwait(false);
    }

    /// <summary>
    /// Executa uma requisição HTTP aplicando a política de retry configurada.
    /// </summary>
    private async Task<HttpResponseMessage> ExecuteRetryPolicyAsync
        (HttpMethod method, string path, string? content = null, TimeSpan? timeout = null, string? token = null)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            return await BuildAndSendRequestAsync(method, path, content, token, timeout).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Envia a requisição HTTP e retorna o conteúdo da resposta como string.
    /// Retorna <c>null</c> em caso de erro ou falha na comunicação.
    /// </summary>
    private async Task<string?> SendAsync
        (HttpMethod method, string path, string? content = null, TimeSpan? timeout = null, string? token = null)
    {
        try
        {
            var response = await ExecuteRetryPolicyAsync(method, path, content, timeout, token).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Envia a requisição HTTP e retorna o objeto <see cref="HttpResponseMessage"/> completo.
    /// Retorna <c>null</c> em caso de erro ou falha na comunicação.
    /// </summary>
    private async Task<HttpResponseMessage?> SendRawAsync
        (HttpMethod method, string path, string? content = null, TimeSpan? timeout = null, string? token = null)
    {
        try
        {
            return await ExecuteRetryPolicyAsync(method, path, content, timeout, token).ConfigureAwait(false);
        }
        catch
        {
            return null;
        }
    }
}