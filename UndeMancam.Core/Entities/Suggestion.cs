using System;
using System.Collections.Generic;

namespace UndeMancam.Core.Entities
{
    public class Suggestion : IUndeMancamEntity
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
