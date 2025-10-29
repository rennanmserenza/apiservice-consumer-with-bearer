/// <summary>
/// Define o contrato para enviar requisições HTTP com suporte a tempo de expiração (timeout).
/// </summary>
/// <remarks>
/// A interface <see cref="IHttpRequestSender"/> define um método para enviar requisições HTTP assíncronas.
/// A implementação dessa interface deve permitir a configuração de um tempo limite (timeout) opcional para a operação.
/// </remarks>
public interface IHttpRequestSender
{
    /// <summary>
    /// Envia uma requisição HTTP assíncrona e retorna a resposta, com suporte a tempo de expiração opcional.
    /// </summary>
    /// <param name="request">A requisição HTTP que será enviada.</param>
    /// <param name="timeout">O tempo de espera (em milissegundos) antes que a requisição seja cancelada. Se nulo, não há limite de tempo.</param>
    /// <returns>Uma tarefa representando a operação assíncrona, com a resposta da requisição.</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, TimeSpan? timeout = null);
}

/// <summary>
/// Implementação padrão do <see cref="IHttpRequestSender"/>, que utiliza um cliente HTTP para enviar requisições.
/// </summary>
/// <remarks>
/// A classe <see cref="DefaultHttpRequestSender"/> é responsável por enviar requisições HTTP assíncronas utilizando o cliente HTTP fornecido.
/// Ela também suporta a definição de um tempo limite (timeout) para as requisições, cancelando a operação se o tempo expirar.
/// </remarks>
public class DefaultHttpRequestSender : IHttpRequestSender
{
    private readonly IHttpClient _httpClient;

    /// <summary>
    /// Constrói uma instância do sender de requisições HTTP utilizando o cliente HTTP fornecido.
    /// </summary>
    /// <param name="httpClient">Instância do <see cref="IHttpClient"/> a ser utilizada para enviar as requisições.</param>
    public DefaultHttpRequestSender(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Envia uma requisição HTTP assíncrona e retorna a resposta, com suporte a tempo limite (timeout) opcional.
    /// </summary>
    /// <param name="request">A requisição HTTP que será enviada.</param>
    /// <param name="timeout">O tempo limite opcional para a requisição. Se nulo, o tempo de expiração não é configurado.</param>
    /// <returns>Uma tarefa representando a operação assíncrona, com a resposta da requisição.</returns>
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, TimeSpan? timeout = null)
    {
        if (timeout.HasValue)
        {
            using (var cts = new CancellationTokenSource(timeout.Value))
            {
                return await _httpClient.SendAsync(request, cts.Token).ConfigureAwait(false);
            }
        }

        return await _httpClient.SendAsync(request).ConfigureAwait(false);
    }
}
