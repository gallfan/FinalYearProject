namespace MechanicTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<MechanicTracker.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MechanicTracker.Models.UsersContext context)
        {
        
            SeedMembership();
        }

        //Seed database with login and users and roles
        private void SeedMembership()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                    "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;
            //create roles
            if (!roles.RoleExists("FrontDesk"))
            {
                roles.CreateRole("FrontDesk");
            }
            if (!roles.RoleExists("Manager"))
            {
                roles.CreateRole("Manager");
            }
            if (!roles.RoleExists("Mechanic"))
            {
                roles.CreateRole("Mechanic");
            }

            //Front Desk Seeding
            if (membership.GetUser("RachelK", false) == null)
            {
                membership.CreateUserAndAccount("RachelK", "password");
            }
            if (!roles.GetRolesForUser("RachelK").Contains("FrontDesk"))
            {
                roles.AddUsersToRoles(new[] { "RachelK" }, new[] { "FrontDesk" });
            }
            if (membership.GetUser("TomS", false) == null)
            {
                membership.CreateUserAndAccount("TomS", "tracker");
            }
            if (!roles.GetRolesForUser("TomS").Contains("FrontDesk"))
            {
                roles.AddUsersToRoles(new[] { "TomS" }, new[] { "FrontDesk" });
            }

            //Manager Seeding
            if (membership.GetUser("LukeG", false) == null)
            {
                membership.CreateUserAndAccount("LukeG", "manager1");
            }
            if (!roles.GetRolesForUser("LukeG").Contains("Manager"))
            {
                roles.AddUsersToRoles(new[] { "LukeG" }, new[] { "Manager" });
            }
           //New Manager
            if (membership.GetUser("BrianQ", false) == null)
            {
                membership.CreateUserAndAccount("BrianQ", "manager2");
            }
            if (!roles.GetRolesForUser("BrianQ").Contains("Manager"))
            {
                roles.AddUsersToRoles(new[] { "BrianQ" }, new[] { "Manager" });
            }




            //Mechanic Seeding
            if (membership.GetUser("tfreehill", false) == null)
            {
                membership.CreateUserAndAccount("tfreehill", "liverpool");
            }
            if (!roles.GetRolesForUser("tfreehill").Contains("Mechanic"))
            {
                roles.AddUsersToRoles(new[] { "tfreehill" }, new[] { "Mechanic" });
            }

            //New Mechanic
            if (membership.GetUser("jcapper", false) == null)
            {
                membership.CreateUserAndAccount("jcapper", "manutd");
            }
            if (!roles.GetRolesForUser("jcapper").Contains("Mechanic"))
            {
                roles.AddUsersToRoles(new[] { "jcapper" }, new[] { "Mechanic" });
            }

            if (membership.GetUser("pranger", false) == null)
            {
                membership.CreateUserAndAccount("pranger", "liverpool");
            }
            if (!roles.GetRolesForUser("pranger").Contains("Mechanic"))
            {
                roles.AddUsersToRoles(new[] { "pranger" }, new[] { "Mechanic" });
            }

            //New Mechanic
            if (membership.GetUser("cgray", false) == null)
            {
                membership.CreateUserAndAccount("cgray", "manutd");
            }
            if (!roles.GetRolesForUser("cgray").Contains("Mechanic"))
            {
                roles.AddUsersToRoles(new[] { "cgray" }, new[] { "Mechanic" });
            }

            if (membership.GetUser("nfitzgerald", false) == null)
            {
                membership.CreateUserAndAccount("nfitzgerald", "liverpool");
            }
            if (!roles.GetRolesForUser("nfitzgerald").Contains("Mechanic"))
            {
                roles.AddUsersToRoles(new[] { "nfitzgerald" }, new[] { "Mechanic" });
            }

            //New Mechanic
            if (membership.GetUser("rlittle", false) == null)
            {
                membership.CreateUserAndAccount("rlittle", "manutd");
            }
            if (!roles.GetRolesForUser("rlittle").Contains("Mechanic"))
            {
                roles.AddUsersToRoles(new[] { "rlittle" }, new[] { "Mechanic" });
            }

            //New Mechanic
            if (membership.GetUser("zelfron", false) == null)
            {
                membership.CreateUserAndAccount("zelfron", "manutd");
            }
            if (!roles.GetRolesForUser("zelfron").Contains("Mechanic"))
            {
                roles.AddUsersToRoles(new[] { "zelfron" }, new[] { "Mechanic" });
            }


        }
    }
}
