using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlogDemo.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseStartup<Startup>();   
                //如果是使用标准的Startup文件的话,就是使用上面这句
                //如果是根据环境变量来使用对应的Startup时,应使用下面的形式
                .UseStartup(typeof(StartupDevelopment).GetTypeInfo().Assembly.FullName);
    }
}
