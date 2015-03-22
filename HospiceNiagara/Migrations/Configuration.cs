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
            var resourceDomains = new List<TeamDomain>
            {
                new TeamDomain{ID=1,Description="Volunteer"},
                new TeamDomain{ID=2,Description="Staff"},
                new TeamDomain{ID=3,Description="Board"},
                new TeamDomain{ID=4,Description="Organizational"}
            };
            resourceDomains.ForEach(rd => context.TeamDomains.Add(rd));
            context.SaveChanges();
            var deathNotice = new List<DeathNotice>
                {
                    new DeathNotice{FirstName="Joe", MiddleName="T", LastName="Smith", Date=DateTime.Parse("2014-12-16"), Location="Bob's House", Notes="Volunteer: Ted Tennant"},
                    new DeathNotice{FirstName="Rachel", LastName="Jones", Date=DateTime.Parse("2014-12-14"), Location="The Stabler Centre", Notes="Room 4"},
                    new DeathNotice{FirstName="Mary", LastName="Brown", Date=DateTime.Parse("2014-12-08"), Location="NN Outreach Team"},
                    new DeathNotice{FirstName="Sally", LastName="Williams", Date=DateTime.Parse("2014-11-30"), Location="NS Outreach Team"}
                };
            deathNotice.ForEach(dn => context.DeathNotices.Add((dn)));
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
                    new Contact {FirstName="Billy", LastName="Bragg", Position="Staff Deputy", Phone="9058992333", Email="bBragg@gmail.com",TeamDomainID=2},
                    new Contact{FirstName="Rhory", LastName="Andrews", Position="Front Desk", Phone="9058993322", Email="rRhor@gmail.com", TeamDomainID=1},
                    new Contact{FirstName="Kate", LastName="Murrell", Position="Administrative Assistant",Phone="x305", Email="kmurrell@hospiceniagara.ca",TeamDomainID=4},
                    new Contact{FirstName="Jessica", LastName="Estabrooks",Position="Finance and Operations Manager",Email="jestabrooks@hospiceniagara.ca",Phone="x238",TeamDomainID=4},
                    new Contact{FirstName="Robert", LastName="Jeffries",Position="Staff Leader",Email="rj@gmail.com",Phone="x298",TeamDomainID=1},
                    new Contact{FirstName="Jane", LastName="Frisell",Position="Board member 1",Email="jf@gmail.com",Phone="x218",TeamDomainID=3},
                    new Contact{FirstName="Rita", LastName="Lang",Position="Board member 2",Email="rl@gmail.com",Phone="x215",TeamDomainID=3},
                };
            contacts.ForEach(u => context.Contacts.Add(u));
            context.SaveChanges();

            var invitations = new List<Invitation>
                {
                    new Invitation{EventID = 1, ContactID=1}
                };
            invitations.ForEach(i => context.Invitations.Add((i)));
            context.SaveChanges();

            //organizational resources
            var resourceCatsOrg = new List<ResourceCategory>
             {
                 new ResourceCategory{ID=1,Name="Forms",TeamDomainID=4},
                 new ResourceCategory{ID=2,Name="Strategic Plan",TeamDomainID=4},
                 new ResourceCategory{ID=3,Name="Quality Improvement Plan",TeamDomainID=4},
                 new ResourceCategory{ID=4,Name="Memos",TeamDomainID=4},
                 new ResourceCategory{ID=5,Name="Links",TeamDomainID=4},
                 new ResourceCategory{ID=6,Name="Events",TeamDomainID=4},
                 new ResourceCategory{ID=7,Name="Test Agenda",TeamDomainID=4},
                 new ResourceCategory{ID=8,Name="Schedules",TeamDomainID=4}
             };
            resourceCatsOrg.ForEach(rco => context.ResourceCategories.Add(rco));
            context.SaveChanges();
            //staff resources
            var resourceCatStaff = new List<ResourceCategory>
            {
                //main categories
                new ResourceCategory{ID=9,Name="Operational Policies",TeamDomainID=2},
                new ResourceCategory{ID=10,Name="Orientation Documents",TeamDomainID=2},
                new ResourceCategory{ID=11,Name="Memos",TeamDomainID=2},
                new ResourceCategory{ID=12,Name="Staff Meeting Minutes",TeamDomainID=2},
                new ResourceCategory{ID=13,Name="Emergency Call Chart",TeamDomainID=2},
                new ResourceCategory{ID=14,Name="Staff Links",TeamDomainID=2}
            };
            resourceCatStaff.ForEach(rcs => context.ResourceCategories.Add(rcs));
            context.SaveChanges();

            var resourceCatBoard = new List<ResourceCategory>
            {
                new ResourceCategory{ID=15,Name="Board Minutes",TeamDomainID=3},
                new ResourceCategory{ID=16,Name="Board Packages",TeamDomainID=3},
                new ResourceCategory{ID=17,Name="Board Orientation",TeamDomainID=3},
                new ResourceCategory{ID=18,Name="Board Contact List",TeamDomainID=3},
                new ResourceCategory{ID=19,Name="Governance Policies",TeamDomainID=3},
                new ResourceCategory{ID=20,Name="Audited Financials",TeamDomainID=3},
                new ResourceCategory{ID=21,Name="Audit and Finance Committee",TeamDomainID=3}
            };
            resourceCatBoard.ForEach(rcd => context.ResourceCategories.Add(rcd));
            context.SaveChanges();

            var resourceCatVol = new List<ResourceCategory>
            {
                new ResourceCategory{ID=22,Name="Town Hall Meeting Minutes",TeamDomainID=1},
                new ResourceCategory{ID=23,Name="Memos",TeamDomainID=1},
                new ResourceCategory{ID=24,Name="Volunteer Updates",TeamDomainID=1},
                new ResourceCategory{ID=25,Name="Orientation Documents",TeamDomainID=1},
                new ResourceCategory{ID=26,Name="Application",TeamDomainID=1},
                new ResourceCategory{ID=27,Name="Presentation",TeamDomainID=1},
                new ResourceCategory{ID=28,Name="Volunteer Related Policies",TeamDomainID=1},
                new ResourceCategory{ID=29,Name="Volunteer Roles Chart",TeamDomainID=1}
            };
            resourceCatVol.ForEach(rcv => context.ResourceCategories.Add(rcv));
            context.SaveChanges();

            //Subcategories
            ///////////////
            var resourceSubCats = new List<ResourceSubCategory>
            {
                 new ResourceSubCategory{Name="Health and Safety", ResourceCategoryID=1},
                 new ResourceSubCategory{Name="Incident Report", ResourceCategoryID=1},
                 new ResourceSubCategory{Name="Mileage & Expense",ResourceCategoryID=1},
                new ResourceSubCategory{Name="Residential",ResourceCategoryID=12},
                new ResourceSubCategory{Name="Town", ResourceCategoryID=12},
                new ResourceSubCategory{Name="Welcome Desk Schedule",ResourceCategoryID=8},
                new ResourceSubCategory{Name="Residential Schedule",ResourceCategoryID=8},
                new ResourceSubCategory{Name="Pet Therapy Schedule",ResourceCategoryID=8},
                new ResourceSubCategory{Name="Welland Day Hospice Schedule",ResourceCategoryID=8},
                new ResourceSubCategory{Name="Event Schedule",ResourceCategoryID=8}
            };
            resourceSubCats.ForEach(rsc => context.ResourceSubCategories.Add(rsc));
            context.SaveChanges();
        }
    }
}
