using System.Text.Json;
using System.Text.Json.Serialization;

namespace apiservice_consumer_with_bearer.Extensions;

/// <summary>
/// Extensões utilitárias para serialização de objetos em JSON.
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// Retorna as opções padrão para serialização JSON.
    /// </summary>
    /// <param name="indented">Define se o JSON deve ser formatado com indentação (padrão: true).</param>
    public static JsonSerializerOptions GetDefaultOptions(bool indented = true)
    {
        return new JsonSerializerOptions
        {
            WriteIndented = indented,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    /// <summary>
    /// Serializa um objeto em uma string JSON formatada.
    /// </summary>
    /// <param name="obj">O objeto a ser serializado.</param>
    /// <param name="options">As opções de serialização JSON (se não fornecido, usa padrão).</param>
    /// <param name="indented">Define se o JSON deve ser formatado com indentação (padrão: true).</param>
    /// <returns>Uma string JSON representando o objeto; retorna <c>null</c> se o objeto for nulo.</returns>
    public static string? FormatToJson(this object? obj, JsonSerializerOptions? options = null, bool indented = true)
    {
        if (obj == null)
            return null;

        options ??= GetDefaultOptions(indented);

        return JsonSerializer.Serialize(obj, options);
    }

    /// <summary>
    /// Desserializa uma string JSON para um objeto do tipo especificado.
    /// </summary>
    /// <typeparam name="T">O tipo de objeto a ser retornado.</typeparam>
    /// <param name="json">A string JSON a ser desserializada.</param>
    /// <param name="options">As opções de desserialização JSON (opcional).</param>
    /// <returns>
    /// Um objeto do tipo <typeparamref name="T"/> representando os dados do JSON;
    /// retorna o valor padrão de <typeparamref name="T"/> se o JSON for nulo ou vazio.
    /// </returns>
    /// <exception cref="JsonException">Lançada se o JSON estiver em formato inválido.</exception>
    public static T? FromJson<T>(this string? json, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        options ??= GetDefaultOptions();

        return JsonSerializer.Deserialize<T>(json, options);
    }
}
