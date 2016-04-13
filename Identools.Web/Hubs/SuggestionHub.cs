using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identools.Web.Entities;
using Microsoft.AspNet.SignalR;

namespace Identools.Web.Hubs
{
    public class SuggestionHub : Hub
    {
        public static void UpdateSuggestions()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SuggestionHub>();
            context.Clients.All.updateSuggestions();
        }
    }
}