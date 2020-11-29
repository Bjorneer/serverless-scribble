using DurableTask.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scribble.Functions.Functions
{
    public static class DeleteOldGames
    {
        [FunctionName(nameof(DeleteOldGames))]
        public static Task Run([TimerTrigger("0 2 * * *")

            ]TimerInfo myTimer,
            ILogger log,
            [DurableClient] IDurableOrchestrationClient client)
        {
            log.LogInformation($"C# Timer trigger function {nameof(DeleteOldGames)} executed at: {DateTime.Now}");
            return client.PurgeInstanceHistoryAsync(
                DateTime.MinValue,
                DateTime.UtcNow.AddDays(-1),
                new List<OrchestrationStatus>
                {
                    OrchestrationStatus.Completed,
                    OrchestrationStatus.Failed,
                    OrchestrationStatus.Canceled,
                    OrchestrationStatus.ContinuedAsNew,
                    OrchestrationStatus.Pending,
                    OrchestrationStatus.Running,
                    OrchestrationStatus.Terminated,
                });

        }
    }
}
