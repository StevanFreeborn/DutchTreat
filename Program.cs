using System.IO;
using DutchTreat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (args.Length == 1 && args[0].ToLower() == "/seed")
            {
                RunSeeding(host);
            }
            else
            {
                host.Run();
            }
        }

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
            seeder.SeedAsync().Wait();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AddConfiguration)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        private static void AddConfiguration(HostBuilderContext ctx, IConfigurationBuilder bldr)
        {
            bldr
                .Sources.Clear();

            bldr
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();
        }
    }
}