namespace MunicipiosIbge.Api.Common.Mediator;

public sealed class InMemoryMediator(IServiceProvider serviceProvider) : IMediator
{
    public Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);

        return (Task<TResponse>)handlerType
            .GetMethod("HandleAsync")!
            .Invoke(handler, [request, cancellationToken])!;
    }
}
