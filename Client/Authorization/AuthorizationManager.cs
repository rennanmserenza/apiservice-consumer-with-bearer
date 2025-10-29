/// <summary>
/// Define o contrato para gerenciar a adição de cabeçalhos de autorização em requisições HTTP.
/// </summary>
/// <remarks>
/// A interface <see cref="IAuthorizationManager"/> garante que qualquer implementação fornecerá a lógica necessária
/// para adicionar cabeçalhos de autorização "Bearer" em requisições HTTP. Isso envolve verificar se o token fornecido
/// é válido antes de delegar a responsabilidade para um manipulador de cabeçalhos de autorização.
/// </remarks>
public interface IAuthorizationManager
{
    /// <summary>
    /// Adiciona o cabeçalho de autorização "Bearer" a uma requisição HTTP.
    /// </summary>
    /// <param name="request">A requisição HTTP à qual o cabeçalho de autorização será adicionado.</param>
    /// <param name="token">O token de autenticação (Bearer) que será adicionado ao cabeçalho de autorização.</param>
    /// <exception cref="ArgumentNullException">Lançado se o token for nulo ou vazio.</exception>
    void AddAuthorizationHeader(HttpRequestMessage request, string token);
}

/// <summary>
/// Classe responsável por gerenciar a adição de cabeçalhos de autorização em requisições HTTP.
/// </summary>
/// <remarks>
/// A classe <see cref="AuthorizationManager"/> valida a presença e a integridade do token de autenticação antes de delegar
/// a responsabilidade para um manipulador específico de cabeçalhos de autorização. Caso o token seja inválido, ela lança
/// uma exceção para garantir que apenas tokens válidos sejam utilizados nas requisições.
/// </remarks>
public class AuthorizationManager : IAuthorizationManager
{
    private readonly IAuthorizationHeaderHandler _authorizationHeaderHandler;

    /// <summary>
    /// Constrói uma instância do gerenciador de cabeçalhos de autorização.
    /// </summary>
    /// <param name="authorizationHeaderHandler">Instância do manipulador de cabeçalhos de autorização.</param>
    public AuthorizationManager(IAuthorizationHeaderHandler authorizationHeaderHandler)
    {
        _authorizationHeaderHandler = authorizationHeaderHandler;
    }

    /// <summary>
    /// Adiciona o cabeçalho de autorização "Bearer" a uma requisição HTTP.
    /// </summary>
    /// <param name="request">A requisição HTTP à qual o cabeçalho de autorização será adicionado.</param>
    /// <param name="token">O token de autenticação (Bearer) que será adicionado ao cabeçalho de autorização.</param>
    /// <exception cref="ArgumentNullException">Lançado se o token for nulo ou vazio.</exception>
    public void AddAuthorizationHeader(HttpRequestMessage request, string token)
    {
        _authorizationHeaderHandler.AddAuthorizationHeader(request, token);
    }
}
