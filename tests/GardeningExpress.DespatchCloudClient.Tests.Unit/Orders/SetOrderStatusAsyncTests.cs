
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using NUnit.Framework;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Orders;

[TestFixture]
public class SetOrderStatusAsyncTests
{

    private readonly DespatchCloudConfig _despatchCloudConfig = new DespatchCloudConfig
    {
        ApiBaseUrl = "https://fake.api",
        LoginPassword = "secret",
        LoginEmailAddress = "test@test.com"
    };

    private Mock<HttpMessageHandler> _handler;
    private DespatchCloudHttpClient _despatchCloudHttpClient;


    [SetUp]
    public void Setup()
    {
        _handler = new Mock<HttpMessageHandler>();
        var httpClient = _handler.CreateClient();
        httpClient.BaseAddress = new Uri(_despatchCloudConfig.ApiBaseUrl);

        _despatchCloudHttpClient = new DespatchCloudHttpClient(httpClient);
    }
    
    [Test]
    public async Task SetOrderStatusAsync_ShouldReturnIsSuccessFalse_AndErrorMessage_WhenNon200ResponseOccurs()
    {
        // ARRANGE
        var uriString = $"{_despatchCloudConfig.ApiBaseUrl}order/{12331444}/update";
        var uri = new Uri(uriString);

        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == uri && r.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = "Internal Server Error"
            });

        // ACT
        var result = await _despatchCloudHttpClient.SetOrderStatusAsync(12331444, 1);

        // ASSERT
        Assert.False(result.IsSuccess);            
        Assert.AreEqual("Response from DespatchCloud: 500 - Internal Server Error", result.Error);
    }
}