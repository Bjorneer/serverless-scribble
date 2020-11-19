using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DrawioFunctions.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DrawioFunctions.Functions
{
    public static class GameOrchestrator
    {
        [FunctionName("GameOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            GameEntity game)
        {
            var cts = new CancellationTokenSource();
            var timerTask = context.CreateTimer(DateTime.Now.AddSeconds(30), cts.Token);

            while ()
            {

            }
            var timerTask = context.CreateTimer(DateTime.Now.AddSeconds, );
        }

        [FunctionName("GameOrchestrator_StartNewRound")]
        public static void StartNewRound([ActivityTrigger] string name, ILogger log)
        {
        }
    }
}