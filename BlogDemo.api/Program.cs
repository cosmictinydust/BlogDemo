using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BlogDemo.Infrastructure.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace BlogDemo.api
{
    public class Program
    {
public static void Main(string[] args)
{
    var host = CreateWebHostBuilder(args).Build();
    
    using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var myContex = services.GetRequiredService<MyContext>();
                    MyContextSeed.SeedAsync(myContex, loggerFactory).Wait();
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "Error Occured seeding the Database.");
                }
            }


    host.Run();
}

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseStartup<Startup>();   
                //如果是使用标准的Startup文件的话,就是使用上面这句
                //如果是根据环境变量来使用对应的Startup时,应使用下面的形式
                .UseStartup(typeof(StartupDevelopment).GetTypeInfo().Assembly.FullName);
    }
}
