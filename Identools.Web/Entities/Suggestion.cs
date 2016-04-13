using System;
using System.Collections;
using System.Collections.Generic;
using Identools.Web.Data;

namespace Identools.Web.Entities
{
    public class Suggestion : IEntity
    {
        public Guid Id { get; set; }

        public string Location { get; set; }

        public string UserName { get; set; }

        public DateTime StartTime { get; set; }

        public ICollection<SuggestionAttendee> SuggestionAttendees { get; set; }

        public Suggestion()
        {
            SuggestionAttendees = new List<SuggestionAttendee>();
        }
    }
}
