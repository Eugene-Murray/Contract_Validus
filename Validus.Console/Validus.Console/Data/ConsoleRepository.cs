using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Validus.Core.Data;
using Validus.Models;
using System.Data.Entity.Migrations;
using Microsoft.Practices.Unity;

namespace Validus.Console.Data
{
    public class ConsoleRepository : DataContext, IConsoleRepository
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Validus.Console.Models.DatabaseContext>());
        [InjectionConstructor]
        public ConsoleRepository()
            : base("name=DatabaseContext")
        {
        }

        public ConsoleRepository(DbConnection connection)
            : base(connection, true)
        {

        }

        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SubmissionType> SubmissionTypes { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OptionVersion> OptionVersions { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<QuoteTemplate> QuoteTemplates { get; set; }
        public DbSet<AppAccelerator> AppAccelerators { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMembership> TeamMemberships { get; set; }
        public DbSet<COB> COBs { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<Broker> Brokers { get; set; }  
        public DbSet<Underwriter> Underwriters { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }   

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.FilterMembers).WithMany().Map(u => u.ToTable("UserFilterMembers"));
            modelBuilder.Entity<User>().HasMany(u => u.AdditionalUsers).WithMany().Map(u => u.ToTable("UserAdditionalUsers"));
            modelBuilder.Entity<User>().HasMany(u => u.FilterCOBs).WithMany().Map(u => u.ToTable("UserFilterCOBs"));            
            modelBuilder.Entity<User>().HasMany(u => u.AdditionalCOBs).WithMany().Map(u => u.ToTable("UserAdditionalCOBs"));
            modelBuilder.Entity<User>().HasMany(u => u.FilterOffices).WithMany().Map(u => u.ToTable("UserFilterOffices"));
            modelBuilder.Entity<User>().HasMany(u => u.AdditionalOffices).WithMany().Map(u => u.ToTable("UserAdditionalOffices"));            

            modelBuilder.Entity<Team>().HasMany(t => t.RelatedCOBs).WithMany().Map(t => t.ToTable("TeamRelatedCOBs"));
            modelBuilder.Entity<Team>().HasMany(t => t.RelatedOffices).WithMany().Map(t => t.ToTable("TeamRelatedOffices"));
            modelBuilder.Entity<Team>().HasMany(t => t.RelatedQuoteTemplates).WithMany().Map(t => t.ToTable("TeamRelatedQuoteTemplates"));
            modelBuilder.Entity<Team>().HasMany(t => t.AppAccelerators).WithMany().Map(t => t.ToTable("TeamAppAccelerators"));
            modelBuilder.Entity<Team>().HasMany(t => t.RelatedRisks).WithMany().Map(t => t.ToTable("TeamRelatedRisks"));

            modelBuilder.Entity<Wording>()
               .Map<MarketWording>(m => m.Requires("WordingDesc").HasValue("MarketWording"))
               .Map<TermsNConditionWording>(m => m.Requires("WordingDesc").HasValue("TermsNConditionWording"))
               .Map<SubjectToClauseWording>(m => m.Requires("WordingDesc").HasValue("SubjectToClauseWording"));

            modelBuilder.Entity<Submission>().Map(m => m.ToTable("Submissions"))
                        .Map<SubmissionPV>(m => m.ToTable("SubmissionsPV"))
                        .Map<SubmissionEN>(m => m.ToTable("SubmissionsEN"));

            modelBuilder.Entity<Quote>().Map(m => m.ToTable("Quotes"))
                        .Map<QuoteEN>(m => m.ToTable("QuotesEN"));

            //  Added these to allow two links from Quote to User
            //<??>
            //modelBuilder.Entity<Quote>().HasRequired(p => p.QuoteLastModifiedBy).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Quote>().HasRequired(p => p.QuoteCreatedBy).WithMany().WillCascadeOnDelete(false);

			modelBuilder.Entity<Submission>().HasRequired(p => p.Underwriter).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<Submission>().HasRequired(p => p.UnderwriterContact).WithMany().WillCascadeOnDelete(false);

			modelBuilder.Entity<Quote>().HasRequired(p => p.OriginatingOffice).WithMany().WillCascadeOnDelete(false);
        }
      
    }
}