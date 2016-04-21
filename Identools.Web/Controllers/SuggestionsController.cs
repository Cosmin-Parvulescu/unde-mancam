using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Identools.Web.Data;
using Identools.Web.Hubs;
using UndeMancam.Core.Entities;
using UndeMancam.Core.Models;

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

            var suggestionCardModels = suggestions.Select(Mapper.Map<SuggestionCardModel>);

            return Ok(suggestionCardModels);
        }

        [Route("api/suggestions/{id:Guid}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            Suggestion suggestion;

            using (var context = new IdentoolsDbContext())
            {
                suggestion = await context.Suggestions.Include(s => s.SuggestionAttendees).SingleAsync(s => s.Id == id);
            }

            var suggestionCardModel = Mapper.Map<SuggestionCardModel>(suggestion);

            return Ok(suggestionCardModel);
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
                var existingAttendance =
                        await
                            context.SuggestionAttendees.SingleOrDefaultAsync(sa => sa.Suggestion.StartTime.Day == DateTime.Now.Day && sa.UserName == HttpContext.Current.User.Identity.Name);
                Guid? exisitingAttendanceSuggestionId = null;

                if (existingAttendance != null)
                {
                    exisitingAttendanceSuggestionId = existingAttendance.SuggestionId;
                    context.SuggestionAttendees.Remove(existingAttendance);
                }

                newSuggestion.SuggestionAttendees.Add(new SuggestionAttendee
                {
                    UserName = HttpContext.Current.User.Identity.Name
                });
                context.Suggestions.Add(newSuggestion);

                await context.SaveChangesAsync();

                if (exisitingAttendanceSuggestionId.HasValue)
                {
                    SuggestionHub.UpdateSuggestion(exisitingAttendanceSuggestionId.Value);
                }
            }

            var suggestionCardModel = Mapper.Map<SuggestionCardModel>(newSuggestion);

            SuggestionHub.AddSuggestion(suggestionCardModel);

            return Ok();
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
        [Route("api/suggestions/attend/{id:Guid}")]
        public async Task<IHttpActionResult> Attend(Guid id)
        {
            using (var context = new IdentoolsDbContext())
            {
                try
                {
                    var existingAttendance =
                        await
                            context.SuggestionAttendees.SingleOrDefaultAsync(sa => sa.Suggestion.StartTime.Day == DateTime.Now.Day && sa.UserName == HttpContext.Current.User.Identity.Name);

                    if (existingAttendance == null)
                    {
                        var suggestionAttendee = new SuggestionAttendee
                        {
                            SuggestionId = id,
                            UserName = HttpContext.Current.User.Identity.Name
                        };

                        context.SuggestionAttendees.Add(suggestionAttendee);

                        await context.SaveChangesAsync();

                        SuggestionHub.UpdateSuggestion(id);
                    }
                    else
                    {
                        if (existingAttendance.SuggestionId == id)
                        {
                            context.SuggestionAttendees.Remove(existingAttendance);

                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            context.SuggestionAttendees.Remove(existingAttendance);

                            var suggestionAttendee = new SuggestionAttendee
                            {
                                SuggestionId = id,
                                UserName = HttpContext.Current.User.Identity.Name
                            };

                            context.SuggestionAttendees.Add(suggestionAttendee);

                            await context.SaveChangesAsync();

                            SuggestionHub.UpdateSuggestion(id);
                        }

                        SuggestionHub.UpdateSuggestion(existingAttendance.SuggestionId);
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/suggestions/locationhistory")]
        public async Task<IHttpActionResult> LocationHistory()
        {
            var locations = new List<string>();

            using (var context = new IdentoolsDbContext())
            {
                locations.AddRange(await context.Suggestions.Select(s => s.Location).Distinct().ToListAsync());
            }

            return Ok(locations);
        }
    }
}