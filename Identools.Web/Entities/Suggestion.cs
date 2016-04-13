using System;
using Identools.Web.Data;

namespace Identools.Web.Entities
{
    public class Suggestion : IEntity
    {
        public Guid Id { get; set; }

        public string Location { get; set; }

        public string UserName { get; set; }

        public DateTime StartTime { get; set; }
    }
}
