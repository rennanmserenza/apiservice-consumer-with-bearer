using apiservice_consumer_with_bearer.Authentication.TokenHandling;
using apiservice_consumer_with_bearer.Client.ApiClient.Interfaces;
using apiservice_consumer_with_bearer.Config;
using apiservice_consumer_with_bearer.Extensions;

namespace apiservice_consumer_with_bearer.BaseApiServices;

/// <summary>
/// Interface que define operações de requisições HTTP com desserialização automática da resposta para um tipo especificado.
/// </summary>
public interface IBaseDeserializedApiService : IBaseApiService
{
    /// <summary>
    /// Envia uma requisição HTTP GET para o endpoint configurado e desserializa a resposta no tipo <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Tipo no qual a resposta será desserializada.</typeparam>
    /// <param name="urlConfigKey">Chave de configuração utilizada para resolver a URL da API.</param>
    /// <returns>Instância do tipo <typeparamref name="T"/> contendo os dados da resposta, ou uma instância vazia em caso de falha ou resposta nula.</returns>
    Task<T> DeserializedGetAsync<T>(string urlConfigKey) where T : new();

    /// <summary>
    /// Envia uma requisição HTTP POST com o conteúdo fornecido e desserializa a resposta no tipo <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Tipo no qual a resposta será desserializada.</typeparam>
    /// <param name="urlConfigKey">Chave de configuração utilizada para resolver a URL da API.</param>
    /// <param name="payload">Objeto a ser serializado e enviado no corpo da requisição.</param>
    /// <returns>Instância do tipo <typeparamref name="T"/> contendo os dados da resposta, ou uma instância vazia em caso de falha ou resposta nula.</returns>
    Task<T> DeserializedPostAsync<T>(string urlConfigKey, object? payload) where T : new();

    /// <summary>
    /// Envia uma requisição HTTP DELETE com o conteúdo fornecido e desserializa a resposta no tipo <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Tipo no qual a resposta será desserializada.</typeparam>
    /// <param name="urlConfigKey">Chave de configuração utilizada para resolver a URL da API.</param>
    /// <param name="payload">Objeto a ser serializado e enviado no corpo da requisição.</param>
    /// <returns>Instância do tipo <typeparamref name="T"/> contendo os dados da resposta, ou uma instância vazia em caso de falha ou resposta nula.</returns>
    Task<T> DeserializedDeleteAsync<T>(string urlConfigKey, object? payload) where T : new();
}

/// <summary>
/// Implementação base para serviços de API que realiza chamadas HTTP (GET, POST, DELETE)
/// com autenticação automática e desserialização da resposta.
/// </summary>
/// <remarks>
/// Esta classe herda de <see cref="BaseApiService"/> e adiciona funcionalidade para resolver URLs de forma configurável
/// e desserializar automaticamente as respostas JSON para objetos fortemente tipados.
/// </remarks>
public abstract class BaseDeserializedApiService(
    ITokenService tokenService,
    IApiClient apiClient,
    AppSettings appSettings)
    : BaseApiService(tokenService, apiClient)
    , IBaseDeserializedApiService
{
    /// <summary>
    /// URL base da API, definida nas configurações da aplicação.
    /// </summary>
    protected readonly string BaseUrl = appSettings.BaseUri;

    /// <summary>
    /// Método abstrato responsável por compor a URL final com base na chave de configuração fornecida.
    /// </summary>
    /// <param name="apiName">Chave de configuração da API.</param>
    /// <returns>URL completa da API.</returns>
    protected abstract string ObterUrl(string apiName);

    /// <summary>
    /// Prepara a URL completa e o conteúdo JSON a ser enviado em uma requisição HTTP.
    /// </summary>
    /// <param name="apiName">Chave identificadora da API, utilizada para resolver a URL completa.</param>
    /// <param name="payload">Objeto que será serializado em JSON e enviado no corpo da requisição.</param>
    /// <returns>
    /// Uma tupla contendo:
    /// <list type="bullet">
    ///   <item><description><c>url</c>: a URL final do endpoint da API.</description></item>
    ///   <item><description><c>content</c>: o conteúdo serializado no formato <see cref="StringContent"/> com codificação UTF-8.</description></item>
    /// </list>
    /// </returns>
    private (string url, string? content) BuildRequestData(string apiName, object? payload)
    {
        var url = ObterUrl(apiName);
        var content = payload.FormatToJson();

        return (url, content);
    }

    /// <summary>
    /// Desserializa a resposta JSON em uma instância do tipo <typeparamref name="T"/>.
    /// Caso a resposta seja nula ou vazia, retorna uma nova instância padrão de <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Tipo no qual a resposta será desserializada.</typeparam>
    /// <param name="response">String contendo a resposta JSON a ser desserializada.</param>
    /// <returns>Instância do tipo <typeparamref name="T"/> com os dados da resposta, ou uma nova instância vazia se a resposta for nula ou vazia.</returns>
    private static T DeserializeResponseOrDefault<T>(string? response) where T : new()
        => !string.IsNullOrWhiteSpace(response) ? response.FromJson<T>()! : new T();

    public async Task<T> DeserializedGetAsync<T>(string urlConfigKey) where T : new()
    {
        var url = ObterUrl(urlConfigKey);
        var response = await GetAsync(url);

        return DeserializeResponseOrDefault<T>(response);
    }

    public async Task<T> DeserializedPostAsync<T>(string urlConfigKey, object? payload) where T : new()
    {
        var (url, content) = BuildRequestData(urlConfigKey, payload);
        var response = await PostAsync(url, content);

        return DeserializeResponseOrDefault<T>(response);
    }

    public async Task<T> DeserializedDeleteAsync<T>(string urlConfigKey, object? payload) where T : new()
    {
        var (url, content) = BuildRequestData(urlConfigKey, payload);
        var response = await DeleteAsync(url, content);

        return DeserializeResponseOrDefault<T>(response);
    }
}