/// <summary>
/// Interface que define operações básicas de comunicação com APIs via HTTP (GET, POST e DELETE),
/// com suporte ao envio de conteúdo no corpo da requisição.
/// </summary>
public interface IBaseApiService
{
    /// <summary>
    /// Envia uma requisição HTTP GET para o endpoint especificado.
    /// </summary>
    /// <param name="path">O caminho (endpoint) da requisição.</param>
    /// <returns>
    /// Um <see cref="Task{TResult}"/> que representa a operação assíncrona. O resultado será o corpo da resposta como uma <see cref="string"/>, ou <c>null</c> em caso de falha.
    /// </returns>
    Task<string> GetAsync(string path);

    /// <summary>
    /// Envia uma requisição HTTP POST para o endpoint especificado, com o conteúdo fornecido.
    /// </summary>
    /// <param name="path">O caminho (endpoint) da requisição.</param>
    /// <param name="content">O conteúdo a ser enviado no corpo da requisição.</param>
    /// <returns>
    /// Um <see cref="Task{TResult}"/> que representa a operação assíncrona. O resultado será o corpo da resposta como uma <see cref="string"/>, ou <c>null</c> em caso de falha.
    /// </returns>
    Task<string> PostAsync(string path, string content);

    /// <summary>
    /// Envia uma requisição HTTP do tipo POST para o endpoint especificado com o conteúdo fornecido, um tempo de timeout e o token de autenticação.
    /// </summary>
    /// <param name="path">O caminho da URL do endpoint da requisição.</param>
    /// <param name="content">O conteúdo a ser enviado no corpo da requisição. Este pode ser uma string JSON ou outro formato conforme necessário pelo endpoint.</param>
    /// <param name="timeOut">O tempo limite (timeout) para a requisição. Se o tempo de execução ultrapassar esse limite, a requisição será cancelada automaticamente.</param>
    /// <returns>
    /// Uma <see cref="Task"/> que representa a operação assíncrona. O valor retornado é uma instância de <see cref="HttpResponseMessage"/> que contém a resposta da requisição HTTP.
    /// Caso ocorra algum erro ou falha na comunicação, o valor retornado pode ser <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza um token de autenticação recuperado através do método <see cref="GetAuthenticationToken"/>.
    /// O token é então passado para o método `PostRawAsync` da instância de <see cref="_apiClient"/>. 
    /// A requisição será cancelada se o tempo limite (timeout) for atingido.
    /// </remarks>
    Task<HttpResponseMessage> PostRawAsync(string path, string content, TimeSpan timeOut);

    /// <summary>
    /// Envia uma requisição HTTP DELETE para o endpoint especificado, com o conteúdo fornecido.
    /// </summary>
    /// <param name="path">O caminho (endpoint) da requisição.</param>
    /// <param name="content">O conteúdo a ser enviado no corpo da requisição.</param>
    /// <returns>
    /// Um <see cref="Task{TResult}"/> que representa a operação assíncrona. O resultado será o corpo da resposta como uma <see cref="string"/>, ou <c>null</c> em caso de falha.
    /// </returns>
    Task<string> DeleteAsync(string path, string content);
}

/// <summary>
/// Serviço base para comunicação com APIs HTTP, encapsulando operações comuns de GET, POST e DELETE com autenticação automática.
/// </summary>
/// <remarks>
/// Esta classe utiliza um <see cref="ITokenService"/> para obter o token de autenticação e um <see cref="IApiClient"/> para realizar as requisições.
/// </remarks>
public abstract class BaseApiService : IBaseApiService
{
    /// <summary>
    /// Serviço responsável por fornecer o token de autenticação.
    /// </summary>
    protected readonly ITokenService _tokenService;

    /// <summary>
    /// Cliente HTTP utilizado para enviar requisições à API.
    /// </summary>
    protected readonly IApiClient _apiClient;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="BaseApiService"/> com os serviços de token e API client fornecidos.
    /// </summary>
    /// <param name="tokenService">Serviço utilizado para obter o token de autenticação.</param>
    /// <param name="apiClient">Cliente utilizado para executar as requisições HTTP.</param>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="tokenService"/> ou <paramref name="apiClient"/> for <c>null</c>.</exception>
    public BaseApiService(ITokenService tokenService, IApiClient apiClient)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    /// <summary>
    /// Obtém o token de autenticação atual.
    /// </summary>
    /// <returns>Token JWT como <see cref="string"/>.</returns>
    protected string GetAuthenticationToken()
    {
        return _tokenService.ObterAutenticador();
    }

    public async Task<string> GetAsync(string path)
    {
        string token = GetAuthenticationToken();
        return await _apiClient.GetAsync(path, token);
    }

    public async Task<string> PostAsync(string path, string content)
    {
        string token = GetAuthenticationToken();
        return await _apiClient.PostAsync(path, content, token: token);
    }

    public async Task<HttpResponseMessage> PostRawAsync(string path, string content, TimeSpan timeOut)
    {
        string token = GetAuthenticationToken();
        return await _apiClient.PostRawAsync(path, content, timeOut, token);
    }

    public async Task<string> DeleteAsync(string path, string content)
    {
        string token = GetAuthenticationToken();
        return await _apiClient.DeleteAsync(path, content, token);
    }
}
