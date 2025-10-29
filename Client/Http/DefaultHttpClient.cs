namespace apiservice_consumer_with_bearer.Client.Http;

/// <summary>
/// Define o contrato para enviar requisições HTTP assíncronas.
/// </summary>
/// <remarks>
/// A interface <see cref="IHttpClient"/> permite enviar requisições HTTP assíncronas, seja com ou sem um token de cancelamento.
/// Esta interface facilita a substituição do cliente HTTP em diferentes cenários, como testes ou implementações alternativas.
/// </remarks>
public interface IHttpClient
{
    /// <summary>
    /// Envia uma requisição HTTP assíncrona e retorna a resposta.
    /// </summary>
    /// <param name="request">A requisição HTTP que será enviada.</param>
    /// <returns>Uma tarefa representando a operação assíncrona, com um objeto <see cref="HttpResponseMessage"/> que contém a resposta.</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

    /// <summary>
    /// Envia uma requisição HTTP assíncrona e retorna a resposta, com suporte a cancelamento.
    /// </summary>
    /// <param name="request">A requisição HTTP que será enviada.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a requisição.</param>
    /// <returns>Uma tarefa representando a operação assíncrona, com um objeto <see cref="HttpResponseMessage"/> que contém a resposta.</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}

/// <summary>
/// Implementação padrão do cliente HTTP, que encapsula um objeto <see cref="HttpClient"/> para enviar requisições HTTP.
/// </summary>
/// <remarks>
/// A classe <see cref="DefaultHttpClient"/> utiliza a implementação padrão do <see cref="HttpClient"/> para enviar requisições HTTP.
/// Ela expõe métodos assíncronos para enviar requisições, com suporte a cancelamento via <see cref="CancellationToken"/>.
/// Esta classe pode ser facilmente substituída em testes ou em cenários onde seja necessário customizar o comportamento das requisições HTTP.
/// </remarks>
/// <remarks>
/// Constrói uma instância do cliente HTTP utilizando um <see cref="HttpClient"/> existente.
/// </remarks>
/// <param name="httpClient">Instância de <see cref="HttpClient"/> para enviar as requisições.</param>
public class DefaultHttpClient(HttpClient httpClient) : IHttpClient
{
    /// <summary>
    /// Envia uma requisição HTTP assíncrona e retorna a resposta.
    /// </summary>
    /// <param name="request">A requisição HTTP a ser enviada.</param>
    /// <returns>Uma tarefa representando a operação assíncrona, com a resposta da requisição.</returns>
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        => httpClient.SendAsync(request);

    /// <summary>
    /// Envia uma requisição HTTP assíncrona e retorna a resposta, com suporte ao token de cancelamento.
    /// </summary>
    /// <param name="request">A requisição HTTP a ser enviada.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a requisição.</param>
    /// <returns>Uma tarefa representando a operação assíncrona, com a resposta da requisição.</returns>
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        => httpClient.SendAsync(request, cancellationToken);
}
