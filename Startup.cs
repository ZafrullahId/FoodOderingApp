using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Core.Implimentations.Repositories;
using EF_Core.Implimentations.Service;
using EF_Core.Interface.Repositories;
using EF_Core.Interface.Services;
using EF_Core.Models.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace MVC_Project
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
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options => options
            .UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSession();

            services.AddScoped<ICustomerService,CustomerService>();
            services.AddScoped<ICustomerRepo,CustomerRepo>();

            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStaffRepo, StaffRepo>();

            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IFoodRepo, FoodRepo>();

            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IOrderRepo, OrderRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Customer}/{action=LogIn}/{id?}");
            });
        }
    }
}
