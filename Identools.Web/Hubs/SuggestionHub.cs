using System;
using Identools.Web.Models;
using Microsoft.AspNet.SignalR;

namespace Identools.Web.Hubs
{
    public class SuggestionHub : Hub
    {
        public static void AddSuggestion(SuggestionListModel suggestion)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.updateSuggestions(new { type = "POSTED_SUGGESTION", suggestion });
        }

        public static void UpdateSuggestion(Guid suggestionId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.updateSuggestion(suggestionId);
        }
    }
}