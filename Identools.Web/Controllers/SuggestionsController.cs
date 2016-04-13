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
using Identools.Web.Models;

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
                    var todaysSuggestions = await context.Suggestions.Where(s => s.StartTime.Day == DateTime.Now.Day).Include(s => s.SuggestionAttendees).ToListAsync();
                    suggestions.AddRange(todaysSuggestions);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok(suggestions.Select(s => new SuggestionListModel
            {
                Id = s.Id,
                StartTime = s.StartTime,
                Location = s.Location,
                AttendeeCount = s.SuggestionAttendees.Count,
                Attendees = s.SuggestionAttendees.Select(sa => sa.UserName.Split('\\').Last()),
                Attending = s.SuggestionAttendees.Any(sa => sa.UserName == HttpContext.Current.User.Identity.Name)
            }));
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

                newSuggestion.SuggestionAttendees.Add(new SuggestionAttendee
                {
                    UserName = HttpContext.Current.User.Identity.Name
                });
            }

            SuggestionHub.AddSuggestion(new SuggestionListModel
            {
                Id = newSuggestion.Id,
                StartTime = newSuggestion.StartTime,
                Location = newSuggestion.Location,
                AttendeeCount = newSuggestion.SuggestionAttendees.Count,
                Attendees = newSuggestion.SuggestionAttendees.Select(sa => sa.UserName.Split('\\').Last()),
                Attending = newSuggestion.SuggestionAttendees.Any(sa => sa.UserName == HttpContext.Current.User.Identity.Name)
            });

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
            using (var context = new IdentoolsDbContext())
            {
                try
                {
                    var suggestion = await context.Suggestions.Include(s => s.SuggestionAttendees).SingleAsync(s => s.Id == id);
                    var existingAttendance = suggestion.SuggestionAttendees.SingleOrDefault(sa => sa.UserName == HttpContext.Current.User.Identity.Name);

                    if (existingAttendance == null)
                    {
                        var suggestionAttendee = new SuggestionAttendee
                        {
                            Suggestion = suggestion,
                            UserName = HttpContext.Current.User.Identity.Name
                        };
                        suggestion.SuggestionAttendees.Add(suggestionAttendee);

                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        suggestion.SuggestionAttendees.Remove(existingAttendance);

                        await context.SaveChangesAsync();
                    }

                    SuggestionHub.UpdateSuggestion(new SuggestionListModel
                    {
                        Id = suggestion.Id,
                        StartTime = suggestion.StartTime,
                        Location = suggestion.Location,
                        AttendeeCount = suggestion.SuggestionAttendees.Count,
                        Attendees = suggestion.SuggestionAttendees.Select(sa => sa.UserName.Split('\\').Last()),
                        Attending =
                                suggestion.SuggestionAttendees.Any(
                                    sa => sa.UserName == HttpContext.Current.User.Identity.Name)
                    });
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok();
        }
    }
}