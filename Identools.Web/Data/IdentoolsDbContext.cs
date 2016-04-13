using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Identools.Web.Entities;

namespace Identools.Web.Data
{
    public class IdentoolsDbContext : DbContext
    {
        public DbSet<Suggestion> Suggestions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureSuggestions(modelBuilder);

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
        }
    }
}