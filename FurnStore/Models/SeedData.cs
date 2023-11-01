using Microsoft.EntityFrameworkCore;
using FurnStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FurnStore.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FurnStoreContext(
              serviceProvider.GetRequiredService<
                  DbContextOptions<FurnStoreContext>>()))
            {
                if (!context.Product.Any())
                {
                    context.Product.AddRange(
                        new Product
                        {
                            Name = "White Office Chair",
                            Material = "Wood",
                            Description = "White Chair perfect for office environments or for the dining table ",
                            Price = 900
                        },

                         new Product
                         {
                             Name = "Black & Brown Leather Sofa ",
                             Description = "Black and Brown Leather Sofa which is padded for maximum comfort",
                             Material = "Leather",
                             Price = 3500
                         },

                           new Product
                           {
                               Name = "Rounded Wooden Table",
                               Description = "Rounded table with dark bottom",
                               Material = "Wood",
                               Price = 1700
                           },

                             new Product
                             {
                                 Name = "Wooden Nightstand",
                                 Description = "Nightstand made specifically for the bedroom",
                                 Material = "Wood",
                                 Price = 1200
                             },

                             new Product
                             {
                                 Name = "Wooden Analog Clock",
                                 Description = "A wooden clock in analog form designed for living Rooms and open spaces",
                                 Material = "Wood",
                                 Price = 600
                             }
                    );
                }

                IdentityRole admin = SeedAdminstratorRole(context);
                SeedAdmininistratorUser(context, admin);
                context.SaveChanges();
            }
        }

        private static IdentityRole SeedAdminstratorRole(IdentityDbContext<IdentityUser> context)
        {
            IdentityRole admin = context.Roles.FirstOrDefault(x => x.Name == "Administrator");
            if (admin == null)
            {
                admin = new IdentityRole("Administrator");
                context.Roles.Add(admin);
            }
            return (admin);
        }

        private static void SeedAdmininistratorUser(IdentityDbContext<IdentityUser> context, IdentityRole admin)
        {
            IdentityUserRole<string> adminRole = context.UserRoles.FirstOrDefault(x => x.RoleId == admin.Id);
            if (adminRole == null)
            {
                IdentityUser adminUser = SeedUser(context, "admin@furnstore.com", "Admin");
                adminRole = new IdentityUserRole<string> { RoleId = admin.Id, UserId = adminUser.Id };
                context.Add(adminRole); 
            }
        }

        private static IdentityUser SeedUser(IdentityDbContext<IdentityUser> context, string email, string password)
        {
            IdentityUser user = context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    Email = email,
                    UserName = email,
                    NormalizedEmail = email.Normalize(),
                    NormalizedUserName = email.Normalize(),
                    EmailConfirmed = true
                };
                PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
                user.PasswordHash = hasher.HashPassword(user, password);
                context.Add(user);
            }
            return user;
        }
    }
}
