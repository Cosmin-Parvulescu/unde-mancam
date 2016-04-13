using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Identools.Web.Data;
using Identools.Web.Entities;
using Identools.Web.Hubs;
using Microsoft.AspNet.SignalR;

namespace Identools.Web.Controllers
{
    public class SuggestionsController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            var suggestions = new List<Suggestion>();

            using (var context = new IdentoolsDbContext())
            {
                try
                {
                    var todaysSuggestions =
                        await context.Suggestions.Where(s => s.StartTime.Day == DateTime.Now.Day).ToListAsync();
                    suggestions.AddRange(todaysSuggestions);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok(suggestions);
        }

        public async Task<IHttpActionResult> Get(Guid id)
        {
            Suggestion suggestion;

            using (var context = new IdentoolsDbContext())
            {
                suggestion = await context.Suggestions.FindAsync(id);
            }

            return Ok(suggestion);
        }

        public async Task<IHttpActionResult> Post([FromBody] Suggestion suggestion)
        {
            var newSuggestion = new Suggestion
            {
                Location = suggestion.Location,
                StartTime = suggestion.StartTime,
                UserName = HttpContext.Current.User.Identity.Name
            };

            using (var context = new IdentoolsDbContext())
            {
                context.Suggestions.Add(newSuggestion);
                await context.SaveChangesAsync();
            }

            SuggestionHub.AddSuggestion(newSuggestion);

            return Ok(newSuggestion);
        }

        public async Task<IHttpActionResult> Put(Guid id, [FromBody] Suggestion suggestion)
        {
            throw new NotImplementedException();
        }

        public async Task<IHttpActionResult> Delete(Guid id)
        {
            using (var context = new IdentoolsDbContext())
            {
                var suggestion = await context.Suggestions.FindAsync(id);
                context.Suggestions.Remove(suggestion);
                await context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IHttpActionResult> Vote(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IHttpActionResult> Attend(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}