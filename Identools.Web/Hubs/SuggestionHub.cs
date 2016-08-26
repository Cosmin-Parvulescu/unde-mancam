using System;
using Microsoft.AspNet.SignalR;
using UndeMancam.Core.Entities;
using UndeMancam.Core.Models;

namespace Identools.Web.Hubs
{
    public class SuggestionHub : Hub
    {
        public static void AddSuggestion(SuggestionCardModel suggestion)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.updateSuggestions(new { type = "POSTED_SUGGESTION", suggestion });
        }

        public static void UpdateSuggestion(Guid suggestionId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.updateSuggestion(suggestionId);
        }

        public static void AnnounceAttendance(string username, string suggestionLocation, bool attend)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.notifyAttendance(username, suggestionLocation, attend);
        }
    }
}