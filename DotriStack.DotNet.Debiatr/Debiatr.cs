using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DotriStack.DotNet.Debiatr;

public static class Debiatr
{
    public static IServiceCollection AddDebiatr(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        services.AddScoped<ISender, Sender>();
        
        var handlerTypes = assembly
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(i => (Interface: i, Implementation: type)));

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.Interface, handler.Implementation);
        }

        return services;
    }
}
