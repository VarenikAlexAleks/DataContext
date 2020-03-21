using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Data;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private WebAppData ctx;
        private WebAppData InitDb()
        {
            var db = new WebAppData("DataSource=test.db", "Microsoft.Data.Sqlite");

            using( var tx = db.OpenTx())
            {
                db.ExecuteSql("create table if not exists Category (ID INTEGER PRIMARY KEY  AUTOINCREMENT, Name text)");    
                db.ExecuteSql("create table if not exists Product (id integer primary key autoincrement, name text, categoryid int)"); 

                var food = new Category{ Name= "Еда"};
                var closes = new Category{ Name= "Одежда"};

                db.Categories.Save(food);
                db.Categories.Save(closes);

                db.Products.Save( new Product {Name = "Колбаса", CategoryId = food.Id });
                db.Products.Save( new Product {Name = "Шапка", CategoryId = closes.Id });
                db.Products.Save( new Product {Name = "Хлеб", CategoryId = food.Id });
                db.Products.Save( new Product {Name = "Штаны", CategoryId = closes.Id });

                tx.Complete();
            }
            return db;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddScoped<WebAppData>(o => ctx ?? (ctx = InitDb()));
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
