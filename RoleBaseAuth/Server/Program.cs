namespace RoleBaseAuth.Server
{
    using log4net.Repository;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System.IO;
    using System.Reflection;

    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

      
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
