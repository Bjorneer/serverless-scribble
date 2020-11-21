using DrawioFunctions.Models;
using DrawioFunctions.Requests;
using DrawioFunctions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Scribble.Functions.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scribble.Functions.Functions
{
    public static class GameFunctions
    {
        [FunctionName("draw")]
        public static async Task<IActionResult> Draw(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log,
            [DurableClient] IDurableOrchestrationClient client)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<DrawRequest>(requestBody);


            if (data.GameCode == null)
                return new BadRequestResult();

            var status = await client.GetStatusAsync("g" + data.GameCode);

            if(status == null)
                return new NotFoundResult();

            if (status.RuntimeStatus != OrchestrationRuntimeStatus.Running)
                return new BadRequestResult();

            await client.RaiseEventAsync("g" + data.GameCode, "Draw", data);

            return new OkResult();
        }

        [FunctionName("guess")]
        public static async Task<IActionResult> Guess(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log,
            [DurableClient] IDurableOrchestrationClient client)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<GuessRequest>(requestBody);

            if (data.GameCode == null)
                return new BadRequestResult();

            var status = await client.GetStatusAsync("g" + data.GameCode);

            if (status == null)
                return new NotFoundResult();

            if (status.RuntimeStatus != OrchestrationRuntimeStatus.Running)
                return new BadRequestResult();

            await client.RaiseEventAsync("g" + data.GameCode, "Guess", data);

            return new OkResult();
        }
    }
}
