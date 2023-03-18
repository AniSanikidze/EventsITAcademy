using EventsITAcademy.Domain.Users;
using EventsITAcademy.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using EventsITAcademy.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EventsITAcademy.Persistence.Seed
{
    public class EventsITAcademySeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            Migrate(database);
            SeedEverything(database);
        }

        private static void Migrate(ApplicationContext context)
        {
            context.Database.Migrate();
        }

        private static void SeedEverything(ApplicationContext context)
        {
            var seeded = false;

            //SeedUsers(context, ref seeded);
            SeedUsers(context, ref seeded);
            SeedRoles(context, ref seeded);
            SeedUserRoles(context, ref seeded);

            if (seeded)
                context.SaveChanges();
        }

        private static void SeedUsers(ApplicationContext context, ref bool seeded)
        {
            var users = new List<User>()
            {
                new User
                {
                    Status = EntityStatuses.Active,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserName = "Administrator",
                    Email = "admin@gmail.com",
                    PasswordHash = GenerateHash("admin123"),
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    NormalizedUserName = "ADMINISTRATOR"
                },
                new User
                {
                    Status = EntityStatuses.Active,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserName = "User1",
                    Email = "user1@gmail.com",
                    PasswordHash = GenerateHash("user"),
                    NormalizedEmail = "user1@gmail.com",
                    NormalizedUserName = "User1"
                },
                new User
                {
                    Status = EntityStatuses.Active,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserName = "User2",
                    Email = "user2@gmail.com",
                    PasswordHash = GenerateHash("user"),
                    NormalizedEmail = "USER2@GMAIL.COM",
                    NormalizedUserName = "USER2"
                },
                new User
                {
                    Status = EntityStatuses.Active,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserName = "Moderator",
                    Email = "moderator@gmail.com",
                    PasswordHash = GenerateHash("moderator123"),
                    NormalizedEmail = "MODERATOR@GMAIL.COM",
                    NormalizedUserName = "MODERATOR"
                },
            };

            foreach (var user in users)
            {
                if (context.Users.Any(x => x.Email == user.Email && x.UserName == user.UserName)) continue;
                context.Users.Add(user);

                seeded = true;
            }
            context.SaveChanges();
        }

        private static void SeedRoles(ApplicationContext context, ref bool seeded)
        {
            var roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                }
            };

            foreach (var role in roles)
            {
                if (context.Roles.Any(x => x.Name == role.Name)) continue;
                context.Roles.Add(role);

                seeded = true;
            }
            context.SaveChanges();
        }
        private static void SeedUserRoles(ApplicationContext context, ref bool seeded)
        {
            var userRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    UserId = context.Users.First(x=> x.UserName == "Administrator").Id,
                    RoleId = context.Roles.First(x=> x.Name == "Admin").Id
                },
                new IdentityUserRole<string>()
                {
                    UserId = context.Users.First(x=> x.UserName == "User1").Id,
                    RoleId = context.Roles.First(x=> x.Name == "User").Id
                },
                new IdentityUserRole<string>()
                {
                    UserId = context.Users.First(x=> x.UserName == "User2").Id,
                    RoleId = context.Roles.First(x=> x.Name == "User").Id
                },
                new IdentityUserRole<string>()
                {
                    UserId = context.Users.First(x=> x.UserName == "Moderator").Id,
                    RoleId = context.Roles.First(x=> x.Name == "Moderator").Id
                }
            };

            foreach (var userRole in userRoles)
            {
                if (context.UserRoles.Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId)) continue;
                context.UserRoles.Add(userRole);

                seeded = true;
            }
            context.SaveChanges();
        }

        //private static void SeedSubtasks(ApplicationContext context, ref bool seeded)
        //{
        //    var subtasks = new List<Subtask>()
        //    {
        //        new Subtask
        //        {
        //           ToDoItemId = context.ToDos.SingleOrDefault(x=>x.Title == "ToDo1 for User 1").Id,
        //           CreatedAt = DateTime.Now,
        //           ModifiedAt = DateTime.Now,
        //           Status = EntityStatuses.Active,
        //           Title = "Subtask1 For ToDo1"
        //        },
        //        new Subtask
        //        {
        //           ToDoItemId = context.ToDos.SingleOrDefault(x=>x.Title == "ToDo1 for User 1").Id,
        //           CreatedAt = DateTime.Now,
        //           ModifiedAt = DateTime.Now,
        //           Status = EntityStatuses.Active,
        //           Title = "Subtask2 For ToDo1"
        //        },
        //        new Subtask
        //        {
        //           ToDoItemId = context.ToDos.SingleOrDefault(x=>x.Title == "ToDo2 for User 1").Id,
        //           CreatedAt = DateTime.Now,
        //           ModifiedAt = DateTime.Now,
        //           Status = EntityStatuses.Active,
        //           Title = "Subtask withId 3 For ToDo2"
        //        },
        //        new Subtask
        //        {
        //            ToDoItemId = context.ToDos.SingleOrDefault(x=>x.Title == "ToDo3 for User 1").Id,
        //            CreatedAt = DateTime.Now,
        //            ModifiedAt = DateTime.Now,
        //            Status = EntityStatuses.Active,
        //            Title = "Subtask with Id 5 for  For ToDo3"
        //        }
        //    };
        //    foreach (var subtask in subtasks)
        //    {
        //        if (context.Subtasks.Any(x => x.Title == subtask.Title)) continue;

        //        context.Subtasks.Add(subtask);

        //        seeded = true;
        //    }
        //    context.SaveChanges();
        //}
        private static string GenerateHash(string input)
        {
            const string SECRET_KEY = "lfherffg324";
            using (SHA512 sha = SHA512.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input + SECRET_KEY);
                byte[] hashBytes = sha.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        //private static void SeedUsers(ApplicationContext context, ref bool seeded)
        //{
        //    var users = new List<User>()
        //    {
        //        new User
        //        {
        //            Username = "TestUser1",
        //            Password = GenerateHash("TestPassword1"),
        //            CreatedAt = DateTime.Now,
        //            ModifiedAt = DateTime.Now,
        //            Status = EntityStatuses.Active
        //        },
        //        new User
        //        {
        //            Username = "TestUser2",
        //            Password = GenerateHash("TestPassword2"),
        //            CreatedAt = DateTime.Now,
        //            ModifiedAt = DateTime.Now,
        //            Status = EntityStatuses.Active
        //        }

        //    };

        //    foreach (var user in users)
        //    {
        //        if (context.Users.Any(x => x.Username == user.Username)) continue;

        //        context.Users.Add(user);

        //        seeded = true;
        //    }
        //    context.SaveChanges();
        //}
    }
}
