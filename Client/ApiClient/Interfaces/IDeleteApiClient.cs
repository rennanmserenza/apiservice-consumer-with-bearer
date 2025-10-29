namespace apiservice_consumer_with_bearer.Client.ApiClient.Interfaces;

/// <summary>
/// Interface responsável por executar requisições HTTP DELETE com suporte a envio de conteúdo e autenticação.
/// </summary>
public interface IDeleteApiClient
{
    /// <summary>
    /// Envia uma requisição HTTP DELETE para o endpoint especificado, incluindo um corpo JSON opcional.
    /// </summary>
    /// <param name="path">O endpoint (URL relativa) para o qual a requisição DELETE será enviada.</param>
    /// <param name="content">O conteúdo a ser enviado no corpo da requisição, no formato <see cref="StringContent"/> (geralmente JSON).</param>
    /// <param name="token">Token de autenticação opcional. Se fornecido, será adicionado ao cabeçalho da requisição.</param>
    /// <returns>
    /// Um <see cref="Task{TResult}"/> que representa a operação assíncrona. O resultado será o corpo da resposta como uma <see cref="string"/>, ou <c>null</c> em caso de falha.
    /// </returns>
    Task<string?> DeleteAsync(string path, string? content, string? token = null);
}
