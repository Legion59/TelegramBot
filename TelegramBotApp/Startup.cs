﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot;
using TelegramBotApp;
using TelegramBotApp.Database;
using TelegramBotApp.Services;
using TelegramBotApp.Services.TelegramSevices;
using TelegramBotApp.Services.WeatherServices;

namespace BeetrootTgBot
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
            services.AddControllers()
                .AddNewtonsoftJson();

            var botConfiguration = Configuration.Get<BotConfiguration>();

            services.AddSingleton(botConfiguration);

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            services.AddHttpClient("OpenWeatherMap", client =>
            {
                client.BaseAddress = new Uri("https://api.openweathermap.org");
                client.Timeout = TimeSpan.FromSeconds(10);
            })
                .AddTypedClient<IWeatherApiClient>(client => new WeatherApiClient(client, settings, botConfiguration.ApiKey));

            services.AddHttpClient("tgclient")
                .AddTypedClient<ITelegramBotClient>(client =>
                    new TelegramBotClient(botConfiguration.BotAccessToken, client));

            services.AddTransient<ITelegramServices, TelegramServices>();

            services.AddHostedService<InitService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BotConfiguration botConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                var token = botConfiguration.BotAccessToken;

                endpoints.MapControllerRoute("tgwebhook",
                    $"bot/{token}",
                    new { controller = "Bot", action = "Post" });

                endpoints.MapControllers();
            });
        }
    }
}