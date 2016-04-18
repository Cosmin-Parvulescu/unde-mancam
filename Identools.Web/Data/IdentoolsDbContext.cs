using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using UndeMancam.Core.Entities;

namespace Identools.Web.Data
{
    public class IdentoolsDbContext : DbContext
    {
        public DbSet<Suggestion> Suggestions { get; set; }

        public DbSet<SuggestionAttendee> SuggestionAttendees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureSuggestions(modelBuilder);
            ConfigureSuggestionAttendees(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureSuggestions(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Suggestion>()
                .HasKey(s => s.Id);

            modelBuilder
                .Entity<Suggestion>()
                .Property(s => s.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder
                .Entity<Suggestion>()
                .HasMany(s => s.SuggestionAttendees);
        }

        private void ConfigureSuggestionAttendees(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<SuggestionAttendee>()
                .HasKey(sa => new { sa.SuggestionId, sa.UserName });

            modelBuilder
                .Entity<SuggestionAttendee>()
                .HasRequired(sa => sa.Suggestion);
        }
    }
}