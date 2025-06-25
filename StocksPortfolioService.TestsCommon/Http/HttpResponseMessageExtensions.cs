using System.Net;
using Xunit;

namespace StocksPortfolioService.TestsCommon.Http;

public static class HttpResponseMessageExtensions
{
    public static async Task AssertStatusCode(this HttpResponseMessage message, HttpStatusCode expectedHttpStatusCode)
    {
        var responseContent = await message.Content.ReadAsStringAsync();
        Assert.True(message.StatusCode == expectedHttpStatusCode,
            $"Expected status code is {(int)expectedHttpStatusCode}, but got {(int)message.StatusCode}." +
            $"$Server response: {responseContent}");
    }
}
