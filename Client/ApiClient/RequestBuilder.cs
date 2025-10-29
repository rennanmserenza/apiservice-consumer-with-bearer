using apiservice_consumer_with_bearer.Client.Authorization;
using apiservice_consumer_with_bearer.Helpers;

namespace apiservice_consumer_with_bearer.Client.ApiClient;

/// <summary>
/// Interface responsável pela construção de requisições HTTP, incluindo configuração de método, URL, conteúdo e cabeçalhos.
/// </summary>
/// <remarks>
/// A interface <see cref="IRequestBuilder"/> define o contrato para qualquer classe que implemente a construção de requisições HTTP.
/// Ela exige um método para construir uma requisição HTTP com base no método, caminho, conteúdo e cabeçalho de autorização.
/// </remarks>
public interface IRequestBuilder
{
    /// <summary>
    /// Constrói e configura uma requisição HTTP com o método, caminho, conteúdo e cabeçalhos necessários.
    /// </summary>
    /// <param name="method">O método HTTP a ser utilizado na requisição (ex.: GET, POST, PUT, DELETE).</param>
    /// <param name="path">O caminho da URL do endpoint da requisição.</param>
    /// <param name="content">O conteúdo a ser enviado no corpo da requisição, se aplicável (pode ser nulo ou vazio).</param>
    /// <param name="token">O token de autenticação a ser usado no cabeçalho de autorização. Se nulo ou vazio, o cabeçalho não será adicionado.</param>
    /// <returns>Uma instância de <see cref="HttpRequestMessage"/> configurada com os parâmetros fornecidos.</returns>
    HttpRequestMessage BuildRequest(HttpMethod method, string path, string? content, string? token);
}

/// <summary>
/// Classe responsável por construir requisições HTTP, incluindo cabeçalhos de autorização e corpo da requisição.
/// </summary>
/// <remarks>
/// A classe <see cref="RequestBuilder"/> encapsula a lógica de construção de requisições HTTP, configurando o método HTTP, o caminho da URL,
/// o conteúdo da requisição (se houver) e o cabeçalho de autorização com o token fornecido.
/// </remarks>
/// <remarks>
/// Constrói uma instância do <see cref="RequestBuilder"/>.
/// </remarks>
/// <param name="authorizationManager">Instância do <see cref="IAuthorizationManager"/> usada para adicionar cabeçalhos de autorização à requisição.</param>
public class RequestBuilder(IAuthorizationManager authorizationManager) : IRequestBuilder
{
    public HttpRequestMessage BuildRequest(HttpMethod method, string path, string? content, string? token)
    {
        var request = new HttpRequestMessage(method, path);

        if (!string.IsNullOrWhiteSpace(content))
            request.Content = StringHelper.GerarStringContentUTF8(content);

        // Adiciona o cabeçalho de autorização
        authorizationManager.AddAuthorizationHeader(request, token);

        return request;
    }
}