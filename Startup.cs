using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;


namespace BookStore
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            this for memory use not save in a database
            services.AddSingleton<IBookStoreRepository<Author>, AuthorRepository>();
            services.AddSingleton<IBookStoreRepository<Book>, BookRepository>();
            */

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<IBookStoreRepository<Author>, AuthorDbRepository>();
            services.AddScoped<IBookStoreRepository<Book>, BookDbRepository>();
            services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthorization();
            app.UseAuthentication(); 
            app.UseStaticFiles(); 
            app.UseMvc(route=> {
                route.MapRoute("default", "{controller=Author}/{action=Index}/{id?}");
            });
        }
    }
}
