using DrawioFunctions.Helpers;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection.Metadata;

[assembly: FunctionsStartup(typeof(DrawioFunctions.Startup))]
namespace DrawioFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddFilter(level => true);
            });

            var config = (IConfiguration)builder.Services.First(d => d.ServiceType == typeof(IConfiguration)).ImplementationInstance;

            builder.Services.AddSingleton((s) =>
            {
                CosmosClientBuilder cosmosClientBuilder = new CosmosClientBuilder(Constants.COSMOS_DB_CONNECTIONSTRING);

                return cosmosClientBuilder.WithConnectionModeDirect()
                    .WithApplicationRegion("North Europe")
                    .WithBulkExecution(true)
                    .Build();
            });
        }
    }
}