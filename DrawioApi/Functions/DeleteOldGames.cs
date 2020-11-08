using System;
using DrawioFunctions.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DrawioFunctions.Functions
{
    public static class DeleteOldGames
    {
        [FunctionName(nameof(DeleteOldGames))]
        public static void Run([TimerTrigger("0 2 * * *")]TimerInfo myTimer, 
            ILogger log,
            [DurableClient] IDurableEntityClient client)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.webjobs.extensions.durabletask.idurableentityclient.listentitiesasync?view=azure-dotnet
            log.LogInformation($"C# Timer trigger function {nameof(DeleteOldGames)} executed at: {DateTime.Now}");
        }
    }
}
