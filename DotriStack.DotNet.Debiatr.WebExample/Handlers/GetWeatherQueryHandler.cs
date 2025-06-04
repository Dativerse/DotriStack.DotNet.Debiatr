using DotriStack.DotNet.Debiatr.WebExample.Models;

namespace DotriStack.DotNet.Debiatr.WebExample.Handlers;

public class GetWeatherQuery : IRequest<WeatherDto[]>
{
}
    
public class GetWeatherQueryHandler() : IRequestHandler<GetWeatherQuery, WeatherDto[]>
{
    private readonly List<string> summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

    public async Task<WeatherDto[]> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherDto()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Count)]
                }).ToArray();
        
        return forecast;
        
    }
}