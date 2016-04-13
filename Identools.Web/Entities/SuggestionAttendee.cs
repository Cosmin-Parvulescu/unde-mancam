using System;
using Identools.Web.Data;

namespace Identools.Web.Entities
{
    public class SuggestionAttendee
    {
        public Guid SuggestionId { get; set; }

        public string UserName { get; set; }

        public Suggestion Suggestion { get; set; }
    }
}