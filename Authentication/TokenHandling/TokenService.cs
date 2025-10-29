public interface ITokenService
{
    /// <summary>
    /// Obtém o token de autenticação com um tempo limite personalizado para a requisição.
    /// </summary>
    /// <param name="timeout">Tempo máximo de espera para a requisição.</param>
    /// <returns>
    /// Retorna o token de autenticação como uma string.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada caso ocorra um erro ao obter o token ou caso o tempo limite seja excedido.
    /// </exception>
    string ObterAutenticador(TimeSpan? timeout = null);
}

public class TokenService : ITokenService
{
    private readonly IApiClient _apiClient;
    private readonly AppSettings _appSettings;

    // Injeção de dependência
    public TokenService(IApiClient apiClient, AppSettings appSettings)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
    }

    /// <summary>
    /// Prepara a URL completa e o conteúdo JSON a ser enviado em uma requisição HTTP.
    /// </summary>
    /// <returns>
    /// Uma tupla contendo:
    /// <list type="bullet">
    ///   <item><description><c>url</c>: a URL final do endpoint da API.</description></item>
    ///   <item><description><c>content</c>: o conteúdo serializado no formato <see cref="StringContent"/> com codificação UTF-8.</description></item>
    /// </list>
    /// </returns>
    private (string baseUrlToken, string content) BuildRequestData()
    {
        var autenticacaoRequest = new AutenticacaoRequest
        {
            usuario = _appSettings.Usuario,
            senha = _appSettings.Senha
        };

        string baseUrlToken = _appSettings.Uri + "/v1/Autenticacao";
        var content = autenticacaoRequest.FormatToJson();

        return (baseUrlToken, content);
    }

    public string ObterAutenticador(TimeSpan? timeout = null)
    {
        try
        {
            (string baseUrlToken, string content) = BuildRequestData();
            var token = _apiClient.PostAsync(baseUrlToken, content, timeOut).Result.Deserialize<TokenResponse>().Token;

            return token;
        }
        catch (WebException ex)
        {
            throw new InvalidOperationException("Erro ao obter token de autenticação", ex);
        }
    }
}
