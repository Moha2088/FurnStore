using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FurnStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FurnStore.Data
{
    public class FurnStoreContext : IdentityDbContext<IdentityUser>
    {
        public FurnStoreContext(DbContextOptions<FurnStoreContext> options)
            : base(options)
        {
        }

        public DbSet<FurnStore.Models.Product> Product { get; set; } = default!;
    }
}