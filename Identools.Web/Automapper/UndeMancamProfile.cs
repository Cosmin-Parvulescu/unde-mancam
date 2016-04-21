using System.Linq;
using System.Web;
using AutoMapper;
using UndeMancam.Core.Entities;
using UndeMancam.Core.Models;

namespace Identools.Web.Automapper
{
    public class UndeMancamProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Suggestion, SuggestionCardModel>()
                    .ForMember(scm => scm.AttendeeCount, mo => mo.MapFrom(s => s.SuggestionAttendees.Count))
                    .ForMember(scm => scm.Attendees,
                        mo => mo.MapFrom(s =>
                            s.SuggestionAttendees.Select(sa => sa.UserName.Split('\\').Last())))
                    .ForMember(scm => scm.Attending,
                        mo =>
                            mo.MapFrom(
                                scm =>
                                    scm.SuggestionAttendees.Any(
                                        sa => sa.UserName == HttpContext.Current.User.Identity.Name)));
            });
        }
    }
}