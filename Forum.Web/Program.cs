using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Forum.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((builderContext, config) =>
               {
                   IHostingEnvironment env = builderContext.HostingEnvironment;
                   config.AddJsonFile("azureSettings.json", optional: false, reloadOnChange: true);
               })
                .UseStartup<Startup>()
                .Build();
    }
}
