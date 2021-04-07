using System;
using BookStore.Areas.Identity.Data;
using BookStore.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BookStore.Areas.Identity.IdentityHostingStartup))]
namespace BookStore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AuthBookStoreContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthBookStoreContextConnection")));

                services.AddDefaultIdentity<BookStoreUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<AuthBookStoreContext>();
            });
        }
    }
}