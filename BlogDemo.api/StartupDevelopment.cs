using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                }
                );
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
