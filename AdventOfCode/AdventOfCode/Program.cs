using System;
using System.Linq;
using AdventOfCode.Days;
using AdventOfCode.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public const string Session = "53616c7465645f5f5ab8a516ba4ff92b9468d3a1996d234f7c947cb84057888d7959f428c2263e2a972941df1078afaa";

        static void Main()
        {
            //var builder = new ConfigurationBuilder();
            //builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            //var isDevelopment = string.IsNullOrEmpty(environment) || environment.ToLower() == "development";
            //if (isDevelopment)
            //{
            //    builder.AddUserSecrets<Program>();
            //}
            //Program.Configuration = builder.Build();

            //var services = new ServiceCollection() as IServiceCollection;

            //var temp = Program.Configuration.GetSection(nameof(InputConfiguration));

            ////Map the implementations of your classes here ready for DI
            //services
            //    //.Configure<InputConfiguration>()
            //    //.Configure<InputConfiguration>(Program.Configuration.GetSection(nameof(InputConfiguration)))
            //    .AddOptions()
            //    //.AddLogging()
            //    .BuildServiceProvider();

            //var serviceProvider = services.BuildServiceProvider();

            Program.Run();
        }

        static void Run()
        {
            Console.WriteLine("Advent of Code");

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IDay).IsAssignableFrom(p));

            foreach (var type in types)
            {
                if (type == typeof(BaseDay))
                {
                    continue;
                }

                try
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        continue;
                    }
                    var instance = constructor.Invoke(new object[] { });
                    var day = instance as IDay;

                    var answer1 = day.ExecuteOne();
                    Console.WriteLine($"Answer 1 for {type}: {answer1}");

                    var answer2 = day.ExecuteTwo();
                    Console.WriteLine($"Answer 2 for {type}: {answer2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }

            Console.WriteLine("Happy Christmas!");
        }
    }
}
