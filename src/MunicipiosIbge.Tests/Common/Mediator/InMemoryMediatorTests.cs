using Microsoft.Extensions.DependencyInjection;
using MunicipiosIbge.Api.Common.Mediator;

namespace MunicipiosIbge.Tests.Common.Mediator;

public sealed class InMemoryMediatorTests
{
    [Fact]
    public async Task SendAsync_ResolvesHandlerAndReturnsResponse()
    {
        var services = new ServiceCollection();
        services.AddScoped<IRequestHandler<TestRequest, string>, TestRequestHandler>();
        var mediator = new InMemoryMediator(services.BuildServiceProvider());

        var result = await mediator.SendAsync(new TestRequest("ping"));

        Assert.Equal("handled: ping", result);
    }

    private sealed record TestRequest(string Value) : IRequest<string>;

    private sealed class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult($"handled: {request.Value}");
        }
    }
}
