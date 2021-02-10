using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using WordLadder.Models;
using WordLadder.Services;
using WordLadder.Services.Abstract;
using WordLadder.Services.Imp;

namespace WordLadder
{
    public class Program
    {


        public  static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //[todo: remove this section]
                })
                .ConfigureServices((hostContext, services) =>
                {
                    /* configuration options */
                    services.Configure<WordLadderOptions>(hostContext.Configuration.GetSection(
                                       WordLadderOptions.Key));

                    /* host processor*/
                    services.AddHostedService<WordLadderHostedService>();
                    services.AddSingleton<IPayloadManager, CommandLinePayloadLoader>();
                    services.AddSingleton<IWordLadderProcessor, WordLadderProcessor>();
                    services.AddSingleton<IWordListRepository, WordListRepository>();
                    services.AddSingleton<IPublisherHub, PublisherHub>();
                    //publishers
                    services.AddSingleton<IPublisher, ConsolePublisher>();
                    services.AddSingleton<IPublisher, FileSystemTextPublisher>();
                    services.AddSingleton<IPublisher, FileSystemCSVPublisher>();
                })
            ;
    }
}
