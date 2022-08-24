using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace H5Security
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                HostConfig.CertPath = context.Configuration["CertPath"];
                HostConfig.CertPassword = context.Configuration["CertPassword"];
                HostConfig.DatabaseConnectionString = context.Configuration["H5SecurityDb"];
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var host = Dns.GetHostEntry("h5ss.dk");
                webBuilder.ConfigureKestrel(opt =>
                {
                    opt.Listen(host.AddressList[0], 80);
                    opt.Listen(host.AddressList[0], 443, listopt =>
                    {
                        listopt.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
                    });
                });

                webBuilder.UseStartup<Startup>();
            });
    }
}
