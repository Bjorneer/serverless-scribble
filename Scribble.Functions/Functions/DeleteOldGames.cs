using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Scribble.Functions.Functions
{
    public static class DeleteOldGames
    {
        [FunctionName(nameof(DeleteOldGames))]
        public static void Run([TimerTrigger("0 2 * * *")]TimerInfo myTimer, 
            ILogger log,
            [DurableClient] IDurableEntityClient client)
        {
            log.LogInformation($"C# Timer trigger function {nameof(DeleteOldGames)} executed at: {DateTime.Now}");

        }
    }
}
