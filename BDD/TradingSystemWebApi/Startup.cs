using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProviderSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderManagementSystem;
using TradingSystem;
using TradingSystem.Reporting;
using TradingSystem.Strategies;
using TradingSystem.Validation;

namespace TradingSystemWebApi
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
            var orderManager = new OrderManager();
            services.AddSingleton<IOrderManager>(orderManager);

            var marketDataSimulator = new MarketDataSimulator();
            services.AddSingleton<IMarketDataSimulator>(marketDataSimulator);
            services.AddSingleton<IMarketDataService>(marketDataSimulator);

            var timeSimulator = new TimeSimulator();
            services.AddSingleton<ITimeSimulator>(timeSimulator);
            
            var tmp = new TradingSystemService(1, orderManager, marketDataSimulator,
                new SimpleStrategyFactory(), new EmptyReporter(), timeSimulator, new SettingsValidator());
            services.AddSingleton(tmp);
            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
