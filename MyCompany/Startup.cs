using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCompany.Domain;
using MyCompany.Domain.Repositories.EntityFramework;
using MyCompany.Domain.Repositories.Interface;
using MyCompany.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;


        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //connect Config with appsettings.json
            Configuration.Bind("Project", new Config());

            /* connect the necessary functionality of the application as services.
            this functionality makes it possible to change the entity system to another one,
            you just need to change the implementation of the interfaces */
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemRepository, EFServiceItemRepository>();
            services.AddTransient<DataManager>();

            //connect AppDbContext
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            //set up identity system
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;//confirm Email
                opts.Password.RequiredLength = 6;//min long of password 
                opts.Password.RequireNonAlphanumeric = false;//can't contain a non-alphanumeric character
                opts.Password.RequireLowercase = false;//need to use lowercase
                opts.Password.RequireUppercase = false;//need to use uppercase
                opts.Password.RequireDigit = false;////need to use digits
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //set up authentication cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";//cookies name
                options.Cookie.HttpOnly = true;//not visible for client side
                options.LoginPath = "/account/login";//permission of administration panel
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;//The SlidingExpiration is set to true to instruct the handler to re-issue a new cookie with a new expiration time any time it processes a request which is more than halfway through the expiration window.
            });

            //configure the authorization policy for the Admin area
            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });//a check that requires the user to be an admin to get permission
            });
            //add support for mvc : controllers and views
            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
            })
                //Set compatibility with asp.net core 3.0
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();//detailed errors information in development mode
            

            //Enable support for static files in the application (css,js and more)
            app.UseStaticFiles();

            //connect the routing system
            app.UseRouting();

            //app.UseHttpsRedirection();

            //Middleware registration order is important,authentication and authorization connect after UseRouting and before UseEndpoints
            
            //connect authentication and authorization
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            // register the needed routes (endpoints)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
