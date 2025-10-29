/// <summary>
/// Interface para definir a política de retry para requisições HTTP.
/// </summary>
public interface IRetryPolicy
{
    /// <summary>
    /// Executa uma ação (geralmente uma requisição HTTP) com a lógica de retry aplicada.
    /// </summary>
    /// <param name="action">A função assíncrona que representa a ação HTTP a ser executada. Deve retornar um <see cref="HttpResponseMessage"/>.</param>
    /// <returns>Um <see cref="Task"/> que representa a operação assíncrona. O resultado é o <see cref="HttpResponseMessage"/> retornado pela ação executada.</returns>
    Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> action);
}

/// <summary>
/// Implementação de uma política de retry para requisições HTTP utilizando o pacote Polly.
/// </summary>
public class RetryPolicy : IRetryPolicy
{
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    /// <summary>
    /// Construtor que configura a política de retry.
    /// A política tenta até 3 vezes, com intervalo exponencial entre tentativas.
    /// </summary>
    public RetryPolicy()
    {
        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r =>
                !r.IsSuccessStatusCode ||
                r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable
            )
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))
            );
    }

    public Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> action)
    {
        return _retryPolicy.ExecuteAsync(action);
    }
}
