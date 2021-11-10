using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PawsitivePets.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitivePets
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(
              Configuration.GetConnectionString("DefaultConnection")));
      services.AddDatabaseDeveloperPageExceptionFilter();

      // enable Role Management for Authorization - User Mgmt for Authentication enabled by default
      services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>();
      services.AddControllersWithViews();

      // enable Google Login using NuGet package we installed and API keys we added to appsettings.json
      services.AddAuthentication()
          .AddGoogle(options =>
          {
            options.ClientId = Configuration.GetValue<string>("Authentication:Google:ClientId");
            options.ClientSecret = Configuration.GetValue<string>("Authentication:Google:ClientSecret");
          });

      // enable Facebook Login using NuGet package we installed and API keys we added to appsettings.json
      services.AddAuthentication()
           .AddFacebook(options =>
           {
             options.AppId = Configuration.GetValue<string>("Authentication:Facebook:AppId");
             options.ClientSecret = Configuration.GetValue<string>("Authentication:Facebook:ClientSecret");
           });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });
    }
  }
}
