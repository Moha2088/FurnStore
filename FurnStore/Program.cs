using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FurnStore.Data;
using FurnStore.Models;
using Microsoft.AspNetCore.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<FurnStoreContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FurnStoreContext") ?? throw new InvalidOperationException("Connection string 'FurnStoreContext' not found.")));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FurnStoreContext>();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            SeedData.Initialize(services);
        }
        
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();
        app.Run();

    }
}