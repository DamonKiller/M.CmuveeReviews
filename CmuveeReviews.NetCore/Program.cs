using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CmuveeReviews.NetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            X.ORMData.ORMBase.CreateDataSource(X.ORMData.DataSourceType.Sqlite, $"Data Source=/app/movies.db");
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
