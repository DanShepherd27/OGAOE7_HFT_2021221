using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OGAOE7_HFT_2021221.Data;
using OGAOE7_HFT_2021221.Logic;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Endpoint
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<IPizzaLogic, PizzaLogic>();
            services.AddTransient<IDrinkLogic, DrinkLogic>();
            services.AddTransient<IPromoOrderLogic, PromoOrderLogic>();

            services.AddTransient<IPizzaRepository, PizzaRepository>();
            services.AddTransient<IDrinkRepository, DrinkRepository>();
            services.AddTransient<IPromoOrderRepository, PromoOrderRepository>();

            services.AddTransient<PizzaDbContext, PizzaDbContext>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:44566")
            );

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {               
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/hub");
            });
        }
    }
}
