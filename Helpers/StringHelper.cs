using System.Text;

namespace apiservice_consumer_with_bearer.Helpers;

/// <summary>
/// Métodos utilitários para manipulação e conversão de strings.
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// Gera um objeto <see cref="StringContent"/> com codificação UTF-8 e tipo de mídia <c>application/json</c>.
    /// </summary>
    /// <param name="content">O conteúdo da string a ser convertido.</param>
    /// <returns>
    /// Um objeto <see cref="StringContent"/> configurado com o conteúdo informado, 
    /// codificado em UTF-8 e com cabeçalho <c>Content-Type: application/json</c>.
    /// Retorna <c>null</c> se o conteúdo for nulo ou vazio.
    /// </returns>
    public static StringContent? GerarStringContentUTF8(string? content)
    {
        return !string.IsNullOrWhiteSpace(content) 
            ? new StringContent(content, Encoding.UTF8, "application/json") 
            : null;
    }
}
