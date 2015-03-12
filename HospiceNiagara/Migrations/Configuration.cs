namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using HospiceNiagara.Models;
    using HospiceNiagara.DAL;
    using System.IO;
    using System.Data.Entity.Validation;
    using System.Text;
    internal sealed class HospiceConfiguration : DbMigrationsConfiguration<HospiceNiagara.DAL.HospiceNiagaraContext>
    {

        public HospiceConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "HospiceNiagara.DAL.HospiceNiagaraContext";

        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
        protected override void Seed(HospiceNiagaraContext context)
        {
            var deathNotice = new List<DeathNotice>
                {
                    new DeathNotice{FirstName="Joe", MiddleName="T", LastName="Smith", Date=DateTime.Parse("2014-12-16"), Location="Bob's House", Notes="Volunteer: Ted Tennant"},
                    new DeathNotice{FirstName="Rachel", LastName="Jones", Date=DateTime.Parse("2014-12-14"), Location="The Stabler Centre", Notes="Room 4"},
                    new DeathNotice{FirstName="Mary", LastName="Brown", Date=DateTime.Parse("2014-12-08"), Location="NN Outreach Team"},
                    new DeathNotice{FirstName="Sally", LastName="Williams", Date=DateTime.Parse("2014-11-30"), Location="NS Outreach Team"}
                };
            deathNotice.ForEach(dn => context.DeathNotices.Add((dn)));
            context.SaveChanges();

            var resourceTypes = new List<ResourceType>
            {
                new ResourceType{Description="Meeting-Attendance"},
                new ResourceType{Description="Meeting-Minutes"},
                new ResourceType{Description="Schedule-PetTherapy"},
                new ResourceType{Description="Schedule-WelcomeDesk"},
                new ResourceType{Description="Schedule-WellandDayHospice"},
                new ResourceType{Description="Announcement-Memo"}
            };
            resourceTypes.ForEach(r => context.ResourceTypes.Add(r));
            context.SaveChanges();

            var events = new List<Event>
                {
                    new Event {Name = "Event 1", Location="Townhall", StartDateTime=DateTime.Parse("2008-01-01T09:30:00"),EndDateTime= DateTime.Parse("2008-01-01T11:30:00")},
                    new Event {Name = "Event 2", Location="Downtown",StartDateTime=DateTime.Parse("2009-06-06T09:30:00"),EndDateTime= DateTime.Parse("2009-06-06T11:30:00")},

                    new Meeting {Name="Important Meeting", Requirements="Formal", Notes="Please attend", Location="The Core",
                    StartDateTime=DateTime.Parse("2009-02-03T11:30:00"), EndDateTime=DateTime.Parse("2009-02-03T13:30:00")}
                };
            events.ForEach(e => context.Events.Add(e));
            context.SaveChanges();

            var contacts = new List<Contact>
                {
                    new Contact {FirstName="Billy", LastName="Bragg", Position="Staff Leader", Phone="9058992333", Email="bBragg@gmail.com"},
                    new Contact{FirstName="Ripping", LastName="Rhory", Position="Front Desk", Phone="9058993322", Email="rRhor@gmail.com"}
                };
            contacts.ForEach(u => context.Contacts.Add(u));
            context.SaveChanges();

            var invitations = new List<Invitation>
                {
                    new Invitation{EventID = 1, ContactID=1}
                };
            invitations.ForEach(i => context.Invitations.Add((i)));
            context.SaveChanges();
        }
    }
}
