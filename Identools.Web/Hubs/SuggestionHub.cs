using Identools.Web.Models;
using Microsoft.AspNet.SignalR;

namespace Identools.Web.Hubs
{
    public class SuggestionHub : Hub
    {
        public static void AddSuggestion(SuggestionListModel suggestion)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.addSuggestion(suggestion);
        }

        public static void UpdateSuggestion(SuggestionListModel suggestion)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.updateSuggestion(suggestion);
        }
    }
}