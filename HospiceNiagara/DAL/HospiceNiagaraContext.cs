using System;
using HospiceNiagara.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HospiceNiagara.DAL
{
        public enum Domain { Volunteer, Staff, Board, Organizational };
        
        public class HospiceNiagaraContext : IdentityDbContext<ApplicationUser>
        {
        public HospiceNiagaraContext()
            : base("HospiceNiagaraContext", throwIfV1Schema:false)
        {
        }

        public static HospiceNiagaraContext Create()
        {
            return new HospiceNiagaraContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<FileStore> FileStores { get; set; }
        public DbSet<DeathNotice> DeathNotices { get; set; }
        public DbSet<TeamDomain> TeamDomains { get; set; }
        public DbSet<ResourceCategory> ResourceCategories { get; set; }
        public DbSet<ResourceSubCategory> ResourceSubCategories { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public System.Data.Entity.DbSet<HospiceNiagara.Models.MeetingResource> MeetingResources { get; set; }

        public System.Data.Entity.DbSet<HospiceNiagara.Models.BoardMember> BoardMembers { get; set; }

        public System.Data.Entity.DbSet<HospiceNiagara.Models.JobDescription> JobDescriptions { get; set; }
    }
}
