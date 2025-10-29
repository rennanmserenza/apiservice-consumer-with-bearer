namespace apiservice_consumer_with_bearer.Client.ApiClient.Interfaces;

/// <summary>
/// Interface para realizar requisições HTTP GET, POST e DELETE.
/// </summary>
/// <remarks>
/// Esta interface agrupa as interfaces <see cref="IGetApiClient"/>, <see cref="IPostApiClient"/> e <see cref="IDeleteApiClient"/> em uma única interface.
/// Ela é projetada para fornecer métodos para interagir com uma API RESTful realizando requisições GET, POST e DELETE.
/// </remarks>
public interface IApiClient : IGetApiClient, IPostApiClient, IDeleteApiClient
{
}
