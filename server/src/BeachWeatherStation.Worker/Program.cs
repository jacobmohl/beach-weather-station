using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureFunctionsWorkerDefaults(builder => {}, options =>
    {
        //options.EnableUserCodeException = true;
    })    
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddMvc(); // Add MVC services for handling HTTP requests;
    })
    .ConfigureLogging(logging =>
    {
        logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            LoggerFilterRule? defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (defaultRule is not null)
            {
                options.Rules.Remove(defaultRule);
            }
        });     
           
        // Disable IHttpClientFactory Informational logs.
        // Note -- you can also remove the handler that does the logging: https://github.com/aspnet/HttpClientFactory/issues/196#issuecomment-432755765 
        //logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
    })    
    .Build();
host.Run();