using System;
using HospiceNiagara.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using System.IO;

namespace HospiceNiagara.DAL
{
    public class HospiceNiagaraContext : DbContext
    {
    
        public HospiceNiagaraContext() : base("HospiceNiagaraContext")
        {
        }
        
        public DbSet<Event> Events { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<FileStore> FileStores { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
