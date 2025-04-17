namespace API.Data.StartupExtensions
{
    using System.Linq;
    using API.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Serilog;
    public static class AppStartup
    {
        public static void HandleMigration(KingPriceDbContext kPContext)
        {
            Log.Information("Configuring Core Startup");
            if (kPContext.Database.GetPendingMigrations().Any())
            {
                Log.Information("Updating the database");
                kPContext.Database.Migrate();
                kPContext.SaveChanges();
                Log.Information("Updating of the database completed");
            }

        }
        public static void SeedDummyData(KingPriceDbContext kPContext)
        {
            var groups = new List<Group>
            {
                new Group { Id = Guid.Parse("BDED2628-34E5-4FBF-87DB-0479DF2E02B3"), Name = "Designers", Description = "UI/UX designers" },
                new Group { Id = Guid.Parse("A66368EA-9ADF-4D62-B9FF-086233D8FF43"), Name = "ProjectManagers", Description = "Project management team" },
                new Group { Id = Guid.Parse("8D5B1864-0BC7-4E5A-906D-1C1E76F50F57"), Name = "Auditors", Description = "Audit team access" },
                new Group { Id = Guid.Parse("7B0B0958-0138-4B81-8D97-1D0D4EC0B18A"), Name = "Management", Description = "Senior management group" },
                new Group { Id = Guid.Parse("556F815D-38F8-4076-A82F-2614F5C25E25"), Name = "Legal", Description = "Legal advisors" },
                new Group { Id = Guid.Parse("DA42B439-E8FF-40BA-948F-3216A2E4A535"), Name = "Onboarding", Description = "Employee onboarding team" },
                new Group { Id = Guid.Parse("3C1D6AC7-AEC5-4E90-A93E-3548D421D283"), Name = "Finance", Description = "Finance department users" },
                new Group { Id = Guid.Parse("C41306BC-1B73-4794-8D4F-3550139E9D6A"), Name = "Viewers", Description = "Read-only access" },
                new Group { Id = Guid.Parse("EEE11AB8-79D1-433A-880E-372178B50F79"), Name = "Support", Description = "Customer support group" },
                new Group { Id = Guid.Parse("166385D8-2695-41B5-A00A-3E368A7B56C3"), Name = "ContentCreators", Description = "Content and media team" },
                new Group { Id = Guid.Parse("8F83C94E-235B-42F6-A0BA-461600A5ED07"), Name = "Testers", Description = "Testing team" },
                new Group { Id = Guid.Parse("D6643FA3-CCF8-4E71-9F46-4F566391BA7E"), Name = "FieldAgents", Description = "Remote field agents" },
                new Group { Id = Guid.Parse("5B9F5EBC-970F-484B-8549-51235BBCC2B9"), Name = "HR", Description = "Human resources" },
                new Group { Id = Guid.Parse("0FC051F9-4F09-4A6F-84F1-526B17E56F43"), Name = "Trainers", Description = "Training department" },
                new Group { Id = Guid.Parse("61E76D0B-E918-49D8-A7BD-530FADB7248D"), Name = "Security", Description = "Security personnel" },
                new Group { Id = Guid.Parse("EEC219FF-1951-4194-A237-56D893B536DA"), Name = "Executives", Description = "Executive leadership" },
                new Group { Id = Guid.Parse("55DEE607-FAB9-43F3-9269-6D7EF5B9A949"), Name = "SysAdmins", Description = "System administrators" },
                new Group { Id = Guid.Parse("07CBD88D-96DA-46A4-B2AD-6F2EE9284417"), Name = "SocialMedia", Description = "Social media managers" },
                new Group { Id = Guid.Parse("92FC1175-14C2-47C2-BFCD-7907D74D22AA"), Name = "Procurement", Description = "Procurement and buying" },
                new Group { Id = Guid.Parse("42E66741-B959-4CC6-A39C-827D6CA196A3"), Name = "Analytics", Description = "BI and analytics group" },
                new Group { Id = Guid.Parse("9680EF8D-26CC-426C-BF01-832D5C206A6F"), Name = "Marketing", Description = "Marketing team" },
                new Group { Id = Guid.Parse("42E0E8CC-6255-4531-AE16-844CBB9ACFBE"), Name = "Vendors", Description = "External vendors" },
                new Group { Id = Guid.Parse("E47D15AE-C7B2-4516-BCA2-8C9ADB69C4E9"), Name = "Operations", Description = "Operations team" },
                new Group { Id = Guid.Parse("18F90D47-B485-4790-8544-8D5DAC867D93"), Name = "QA", Description = "Quality assurance team" },
                new Group { Id = Guid.Parse("152CE762-EFEE-4C1D-BDA3-90D8E7A38BF5"), Name = "Research", Description = "R&D department" },
                new Group { Id = Guid.Parse("FA48B949-641B-44A2-9F6C-9AB253F575BC"), Name = "Editors", Description = "Can edit content" },
                new Group { Id = Guid.Parse("7F7344D5-255F-4A74-ABB6-A122FAAA5800"), Name = "Warehouse", Description = "Warehouse staff" },
                new Group { Id = Guid.Parse("0B9C7229-7C7B-4EE7-BD55-A4FFC4450B63"), Name = "Sales", Description = "Sales department" },
                new Group { Id = Guid.Parse("C2F68581-9B7F-40BD-907D-AC524EB3E389"), Name = "Contractors", Description = "External contractors" },
                new Group { Id = Guid.Parse("C6A95776-F365-42CE-8ACE-BD7CB0BC6348"), Name = "DataScience", Description = "Data scientists and analysts" },
                new Group { Id = Guid.Parse("F4E60639-9174-48B6-9F17-C09484E35F7F"), Name = "Developers", Description = "Software development team" },
                new Group { Id = Guid.Parse("2DE985A6-4CBF-4218-8889-C560507866C2"), Name = "Admins", Description = "Group of administrators" },
                new Group { Id = Guid.Parse("9296E53C-F67E-4DFE-B35B-C8BFD203A209"), Name = "Interns", Description = "Temporary access for interns" },
                new Group { Id = Guid.Parse("3079BDAB-E640-4F21-9B3B-CCCDA0022814"), Name = "Moderators", Description = "Forum and content moderators" },
                new Group { Id = Guid.Parse("4FC583D5-8DF2-4638-ABB4-CDBD53A12856"), Name = "IT", Description = "IT department" },
                new Group { Id = Guid.Parse("367DA07D-90CC-415A-950A-D6EA5FC40920"), Name = "Translators", Description = "Language and translation" },
                new Group { Id = Guid.Parse("D462131B-66F7-4006-847D-D704E29C83B2"), Name = "DevOps", Description = "DevOps engineers" },
                new Group { Id = Guid.Parse("6FC5DED3-D9A0-4C80-9DCD-DF5833B47BBE"), Name = "Compliance", Description = "Regulatory compliance" },
                new Group { Id = Guid.Parse("0103AC54-DE5C-4187-B024-F999F9722E4D"), Name = "Logistics", Description = "Logistics and delivery" },
                new Group { Id = Guid.Parse("CCC982E2-3BFE-4CE1-98B5-FDBE75AE5FDA"), Name = "Partners", Description = "Business partners" }
            };

            foreach (var group in groups)
            {
                if (!kPContext.Groups.Any(g => g.Id == group.Id))
                {
                    kPContext.Groups.Add(group);
                }
            }

            kPContext.SaveChanges();

            var permissions = new List<Permission>
            {
                new Permission { Id = Guid.Parse("F4D2E91F-2076-49E1-8C0C-034DE747E277"), Name = "AssignUsersToGroups", Description = "Allows assigning users to groups" },
                new Permission { Id = Guid.Parse("49300C19-B880-47FF-8AB7-055F5433DABD"), Name = "DeletePermissions", Description = "Allows deleting permissions" },
                new Permission { Id = Guid.Parse("A979E4D6-3E17-4132-A7AA-0F9A05CB3D24"), Name = "ManageRoles", Description = "Allows managing user roles" },
                new Permission { Id = Guid.Parse("858B6A49-2B5E-4F59-8A3D-2514E01AA9BD"), Name = "AccessAdminPanel", Description = "Allows access to admin panel" },
                new Permission { Id = Guid.Parse("E9F4B59C-659D-44DB-9AEE-391CF1916535"), Name = "ViewGroups", Description = "Allows viewing of groups" },
                new Permission { Id = Guid.Parse("5258068B-5741-41A6-8A9F-3E1F1F519744"), Name = "ResetPasswords", Description = "Allows resetting user passwords" },
                new Permission { Id = Guid.Parse("3D53A941-6464-4A97-8887-4DEAF0FE4449"), Name = "ImportData", Description = "Allows importing data from files" },
                new Permission { Id = Guid.Parse("8E4FEE4C-D517-4808-B7F5-58763AF8DAB8"), Name = "RemoveUsersFromGroups", Description = "Allows removing users from groups" },
                new Permission { Id = Guid.Parse("0AFC1331-746D-4566-8797-5BD3DA0559BD"), Name = "ViewPermissions", Description = "Allows viewing available permissions" },
                new Permission { Id = Guid.Parse("94F12D0A-FD06-468A-B1AC-63C9BC2CFFDC"), Name = "ViewAuditLogs", Description = "Allows viewing audit logs" },
                new Permission { Id = Guid.Parse("357753D6-F032-439C-B1F5-6AD9C68B750A"), Name = "CreateGroups", Description = "Allows creating new groups" },
                new Permission { Id = Guid.Parse("57404B69-A90D-4967-B541-72EF7084C664"), Name = "DeleteUsers", Description = "Allows deleting users" },
                new Permission { Id = Guid.Parse("45CD4498-236F-44CB-8043-730B1AA94C75"), Name = "CreatePermissions", Description = "Allows creating permissions" },
                new Permission { Id = Guid.Parse("55BDC3C7-7DC6-4C2B-81FD-77636BC2B63E"), Name = "ViewUsers", Description = "Allows viewing of user list" },
                new Permission { Id = Guid.Parse("2C460E9F-FE7A-496C-A858-AC21C5040E1A"), Name = "EditUsers", Description = "Allows editing user details" },
                new Permission { Id = Guid.Parse("E995052F-2BCD-4D0D-981C-AEC0538693AE"), Name = "CreateUsers", Description = "Allows creating new users" },
                new Permission { Id = Guid.Parse("E4EA6DA0-5192-4776-A3F7-B6B5611CF88F"), Name = "ExportData", Description = "Allows exporting data to files" },
                new Permission { Id = Guid.Parse("795B25E1-C6C3-48F7-A957-C831432E0A31"), Name = "DeleteGroups", Description = "Allows deleting groups" },
                new Permission { Id = Guid.Parse("4285AB66-491D-4745-BEC4-CE2ABD92272B"), Name = "EditPermissions", Description = "Allows editing permissions" },
                new Permission { Id = Guid.Parse("C213F2F5-431F-4C13-9A01-F0E35505CB91"), Name = "EditGroups", Description = "Allows editing group details" }
            };

            foreach (var permission in permissions)
            {
                if (!kPContext.Permissions.Any(p => p.Id == permission.Id))
                {
                    kPContext.Permissions.Add(permission);
                }
            }

            kPContext.SaveChanges();

            var groupPermissions = new List<GroupPermission>
            {
                new GroupPermission { GroupId = Guid.Parse("BDED2628-34E5-4FBF-87DB-0479DF2E02B3"), PermissionId = Guid.Parse("8E4FEE4C-D517-4808-B7F5-58763AF8DAB8") },
                new GroupPermission { GroupId = Guid.Parse("BDED2628-34E5-4FBF-87DB-0479DF2E02B3"), PermissionId = Guid.Parse("49300C19-B880-47FF-8AB7-055F5433DABD") },
                new GroupPermission { GroupId = Guid.Parse("A66368EA-9ADF-4D62-B9FF-086233D8FF43"), PermissionId = Guid.Parse("49300C19-B880-47FF-8AB7-055F5433DABD") },
                new GroupPermission { GroupId = Guid.Parse("8D5B1864-0BC7-4E5A-906D-1C1E76F50F57"), PermissionId = Guid.Parse("A979E4D6-3E17-4132-A7AA-0F9A05CB3D24") },
                new GroupPermission { GroupId = Guid.Parse("7B0B0958-0138-4B81-8D97-1D0D4EC0B18A"), PermissionId = Guid.Parse("858B6A49-2B5E-4F59-8A3D-2514E01AA9BD") },
                new GroupPermission { GroupId = Guid.Parse("556F815D-38F8-4076-A82F-2614F5C25E25"), PermissionId = Guid.Parse("E9F4B59C-659D-44DB-9AEE-391CF1916535") },
                new GroupPermission { GroupId = Guid.Parse("3C1D6AC7-AEC5-4E90-A93E-3548D421D283"), PermissionId = Guid.Parse("3D53A941-6464-4A97-8887-4DEAF0FE4449") },
                new GroupPermission { GroupId = Guid.Parse("C41306BC-1B73-4794-8D4F-3550139E9D6A"), PermissionId = Guid.Parse("8E4FEE4C-D517-4808-B7F5-58763AF8DAB8") },
                new GroupPermission { GroupId = Guid.Parse("EEE11AB8-79D1-433A-880E-372178B50F79"), PermissionId = Guid.Parse("0AFC1331-746D-4566-8797-5BD3DA0559BD") },
                new GroupPermission { GroupId = Guid.Parse("166385D8-2695-41B5-A00A-3E368A7B56C3"), PermissionId = Guid.Parse("94F12D0A-FD06-468A-B1AC-63C9BC2CFFDC") },
                new GroupPermission { GroupId = Guid.Parse("8F83C94E-235B-42F6-A0BA-461600A5ED07"), PermissionId = Guid.Parse("357753D6-F032-439C-B1F5-6AD9C68B750A") },
                new GroupPermission { GroupId = Guid.Parse("D6643FA3-CCF8-4E71-9F46-4F566391BA7E"), PermissionId = Guid.Parse("57404B69-A90D-4967-B541-72EF7084C664") },
                new GroupPermission { GroupId = Guid.Parse("5B9F5EBC-970F-484B-8549-51235BBCC2B9"), PermissionId = Guid.Parse("45CD4498-236F-44CB-8043-730B1AA94C75") },
                new GroupPermission { GroupId = Guid.Parse("0FC051F9-4F09-4A6F-84F1-526B17E56F43"), PermissionId = Guid.Parse("55BDC3C7-7DC6-4C2B-81FD-77636BC2B63E") },
                new GroupPermission { GroupId = Guid.Parse("61E76D0B-E918-49D8-A7BD-530FADB7248D"), PermissionId = Guid.Parse("2C460E9F-FE7A-496C-A858-AC21C5040E1A") },
                new GroupPermission { GroupId = Guid.Parse("EEC219FF-1951-4194-A237-56D893B536DA"), PermissionId = Guid.Parse("E995052F-2BCD-4D0D-981C-AEC0538693AE") },
                new GroupPermission { GroupId = Guid.Parse("55DEE607-FAB9-43F3-9269-6D7EF5B9A949"), PermissionId = Guid.Parse("E4EA6DA0-5192-4776-A3F7-B6B5611CF88F") },
                new GroupPermission { GroupId = Guid.Parse("07CBD88D-96DA-46A4-B2AD-6F2EE9284417"), PermissionId = Guid.Parse("795B25E1-C6C3-48F7-A957-C831432E0A31") },
                new GroupPermission { GroupId = Guid.Parse("92FC1175-14C2-47C2-BFCD-7907D74D22AA"), PermissionId = Guid.Parse("4285AB66-491D-4745-BEC4-CE2ABD92272B") },
                new GroupPermission { GroupId = Guid.Parse("42E66741-B959-4CC6-A39C-827D6CA196A3"), PermissionId = Guid.Parse("C213F2F5-431F-4C13-9A01-F0E35505CB91") }
                };

            foreach (var item in groups)
            {
                groupPermissions.Add(new() { GroupId = item.Id,PermissionId = Guid.Parse("F4D2E91F-2076-49E1-8C0C-034DE747E277") });
            }

            foreach (var groupPermission in groupPermissions)
            {
                if (!kPContext.GroupPermissions.Any(gp => gp.GroupId == groupPermission.GroupId && gp.PermissionId == groupPermission.PermissionId))
                {
                    kPContext.GroupPermissions.Add(groupPermission);
                }
            }

            kPContext.SaveChanges();

            var random = new Random();
            var users = new List<User>();

            for (int i = 0; i < 60; i++)
            {
                var id = Guid.NewGuid();
                var fullName = $"User {random.Next(1000, 9999)}";
                var email = $"user{random.Next(1000, 9999)}@example.com";
                var userName = $"user{random.Next(1000, 9999)}";
                var createdAt = DateTime.UtcNow.AddDays(-random.Next(0, 365));
                var password = $"Password{random.Next(1000, 9999)}";

                users.Add(new User
                {
                    Id = id,
                    FullName = fullName,
                    Email = email,
                    UserName = userName,
                    CreatedAt = createdAt,
                    Password = password
                });
            }

            foreach (var user in users)
            {
                if (!kPContext.Users.Any(u => u.Id == user.Id))
                {
                    kPContext.Users.Add(user);
                }
            }

            kPContext.SaveChanges();
        }
    }

}
