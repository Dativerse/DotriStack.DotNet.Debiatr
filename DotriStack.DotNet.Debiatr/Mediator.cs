using Microsoft.Extensions.DependencyInjection;

namespace DotriStack.DotNet.Debiatr;

public class Mediator(IServiceProvider provider) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)request, cancellationToken);
    }
}
