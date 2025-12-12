using Auth0.AspNetCore.Authentication;
using JobApplicationTracker.Data;
//using JobApplicationTracker.Service;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // UI
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // EF
            builder.Services.AddDbContext<JobContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Auth0
            builder.Services
            .AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"];
                options.ClientId = builder.Configuration["Auth0:ClientId"];
                options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
            });

            // Services
            // builder.Services.AddScoped<IJobSearchService, JobSearchService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            app.Run();
        }
    }
}
