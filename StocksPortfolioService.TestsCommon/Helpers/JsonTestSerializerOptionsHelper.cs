using System.Text.Json;
using System.Text.Json.Serialization;

namespace StocksPortfolioService.TestsCommon.Helpers;

public static class JsonTestSerializerOptionsHelper
{
    public static JsonSerializerOptions GetSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
