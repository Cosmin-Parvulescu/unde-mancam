using System;
using System.Collections.Generic;

namespace Identools.Web.Models
{
    public class SuggestionListModel
    {
        public Guid Id { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public bool Attending { get; set; }

        public int AttendeeCount { get; set; }

        public IEnumerable<string> Attendees { get; set; }
    }
}