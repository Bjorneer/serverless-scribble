using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scribble.Functions.Functions
{
    public static class SignalR
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "game", UserId = "{headers.x-ms-signalr-userid}")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
        //https://charliedigital.com/2019/09/02/azure-functions-signalr-and-authorization/
        //https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-signalr-service-output?tabs=csharp
        //https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-concept-serverless-development-config
        [FunctionName("joinGroup")]
        public static Task JoinGroup( // call from client
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {
            return signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = req.Headers["x-ms-signalr-userid"],
                    GroupName = req.Headers["x-ms-signalr-group"],
                    Action = GroupAction.Add
                });
        }


        [FunctionName("leaveGroup")]
        public static Task leaveGroup(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {

            return signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = req.Headers["x-ms-signalr-userid"],
                    GroupName = req.Headers["x-ms-signalr-group"],
                    Action = GroupAction.Remove
                });
        }
    }
}
