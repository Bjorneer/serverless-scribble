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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "game", UserId = "{headers.x-ms-signalr-userid}")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("addToGroup")]
        public static Task AddToGroup(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ClaimsPrincipal claimsPrincipal,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {

            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            return signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = userIdClaim.Value,
                    GroupName = "myGroup",
                    Action = GroupAction.Add
                });
        }


        [FunctionName("removeFromGroup")]
        public static Task RemoveFromGroup(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ClaimsPrincipal claimsPrincipal,
            [SignalR(HubName = "game")]
            IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            return signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = userIdClaim.Value,
                    GroupName = "myGroup",
                    Action = GroupAction.Remove
                });
        }
    }
}
