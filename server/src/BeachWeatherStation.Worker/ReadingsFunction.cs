using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BeachWeatherStation.Worker;

public class ReadingsFunction
{
    private readonly ILogger<ReadingsFunction> _logger;

    public ReadingsFunction(ILogger<ReadingsFunction> logger)
    {
        _logger = logger;
    }

    [Function("ReadingsFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}