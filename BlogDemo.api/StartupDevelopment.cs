﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BlogDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BlogDemo.Core.Interfaces;
using BlogDemo.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using BlogDemo.api.Extensions;
using AutoMapper;
using FluentValidation;
using BlogDemo.Infrastructure.Resource;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using BlogDemo.Infrastructure.Services;
using BlogDemo.Infrastructure.Resources;
using Newtonsoft.Json.Serialization;

namespace BlogDemo.api
{
    public class StartupDevelopment
    {
        private static IConfiguration Configuration { get; set; }

        public StartupDevelopment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                ;
            services.AddDbContext<MyContext>(options => {
                var connectionString= Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
                }
            );
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper();
            services.AddTransient<IValidator<PostResource>, PostResourceValidator>();

            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<PostPropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<ITypeHelperService, TypeHelperService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,ILoggerFactory loggerFactory)
        {
            //app.UseDeveloperExceptionPage();
            //上面这个适用于MVC,但API不合适
            app.UseMyExceptionHandler(loggerFactory);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
