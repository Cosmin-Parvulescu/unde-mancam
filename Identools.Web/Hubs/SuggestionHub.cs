using Identools.Web.Entities;
using Microsoft.AspNet.SignalR;

namespace Identools.Web.Hubs
{
    public class SuggestionHub : Hub
    {
        public static void AddSuggestion(Suggestion suggestion)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.addSuggestion(suggestion);
        }
    }
}