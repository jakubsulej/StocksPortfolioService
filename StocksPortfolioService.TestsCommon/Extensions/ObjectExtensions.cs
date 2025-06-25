using System.Text;
using System.Text.Json;

namespace StocksPortfolioService.TestsCommon.Extensions;

internal static class ObjectExtensions
{
    public static StringContent ToStringContent(this object input)
    {
        return new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
    }
}
