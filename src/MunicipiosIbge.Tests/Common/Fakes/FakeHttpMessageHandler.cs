using System.Net;

namespace MunicipiosIbge.Tests.Common.Fakes;

public sealed class FakeHttpMessageHandler(
    HttpStatusCode statusCode,
    string content,
    Exception? exception = null) : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        LastRequest = request;

        if (exception is not null)
        {
            return Task.FromException<HttpResponseMessage>(exception);
        }

        return Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content)
        });
    }
}
