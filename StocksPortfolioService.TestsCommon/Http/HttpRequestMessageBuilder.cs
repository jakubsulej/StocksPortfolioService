using Microsoft.AspNetCore.WebUtilities;
using System.Web;

namespace StocksPortfolioService.TestsCommon.Http;

public static class HttpRequestMessageBuilder
{
    public static HttpRequestMessage BuildGet(string requestUri, object? model = null)
    {
        var requestUriAsString = requestUri;
        if (model != null)
        {
            var modelAsQueryString = model.GetType().GetProperties()
                .Where(p => p.GetValue(model) != null)
                .Select(p => new KeyValuePair<string, string?>(p.Name, HttpUtility.UrlEncode(p.GetValue(model)?.ToString())));

            requestUriAsString = QueryHelpers.AddQueryString(requestUri, modelAsQueryString);
        }

        var message = new HttpRequestMessage(HttpMethod.Get, requestUriAsString);
        return message;
    }

    public static HttpRequestMessage BuildDelete(string requestUri)
    {
        var message = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        return message;
    }
}
