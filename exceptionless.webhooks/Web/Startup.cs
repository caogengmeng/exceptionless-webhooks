﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rabbit.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Web.Middleware;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.Get<AppSettings>();

            var enableModules = new[] { "ExceptionLess.WebHook.Abstractions" }.Concat(appSettings.EnableModules)
                .ToDictionary(name => name, name => Assembly.Load(new AssemblyName(name)));

            foreach (var item in enableModules.Where(i => i.Value == null))
                Console.WriteLine($"无法加载模块：{item.Key}");

            var assemblies = enableModules.Values.ToArray();

            services
                .AddLogging()
                .AddMemoryCache()
                .AddHttpClient()
                .AddInterfaceDependency(assemblies)
                .AddMvc();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionLessWebhookMiddleware>();
        }
    }
}