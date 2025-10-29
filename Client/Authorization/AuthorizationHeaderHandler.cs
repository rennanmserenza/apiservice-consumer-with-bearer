/// <summary>
/// Define o contrato para manipulação de cabeçalhos de autorização em requisições HTTP.
/// </summary>
/// <remarks>
/// A interface <see cref="IAuthorizationHeaderHandler"/> permite que implementações forneçam a lógica para adicionar ou remover 
/// o cabeçalho de autorização "Bearer" em requisições HTTP, dependendo do valor do token fornecido.
/// </remarks>
public interface IAuthorizationHeaderHandler
{
    /// <summary>
    /// Adiciona o cabeçalho de autorização "Bearer" a uma requisição HTTP, se o token fornecido não for nulo ou vazio.
    /// </summary>
    /// <param name="request">A requisição HTTP à qual o cabeçalho de autorização será adicionado.</param>
    /// <param name="token">O token de autenticação (Bearer) que será adicionado ao cabeçalho de autorização. Pode ser nulo ou vazio.</param>
    void AddAuthorizationHeader(HttpRequestMessage request, string token);
}

/// <summary>
/// Classe responsável por adicionar cabeçalhos de autorização "Bearer" em requisições HTTP.
/// </summary>
/// <remarks>
/// A classe <see cref="AuthorizationHeaderHandler"/> implementa a lógica necessária para adicionar ou remover o cabeçalho 
/// de autorização "Bearer" em uma requisição HTTP, com base no token fornecido. Caso o token seja válido, ele é adicionado
/// ao cabeçalho. Caso contrário, o cabeçalho de autorização é removido.
/// </remarks>
public class AuthorizationHeaderHandler : IAuthorizationHeaderHandler
{
    public void AddAuthorizationHeader(HttpRequestMessage request, string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            // Adiciona o cabeçalho de autorização com o token Bearer
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            // Se o token não for válido, remove o cabeçalho de autorização
            request.Headers.Authorization = null;
        }
    }
}
