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
            //CreateHostBuilder(args).Build().Run();
            //var _path = @"C:\Work\Projectos2021\Puzzle\words-english\words-english.txt";

            //var _startWord = "same";
            //var _finalWord = "cost"; 

            //programName.exe -s bard -f chaw -d "C:\Work\Projectos2021\Puzzle\words-english\words-english.txt" -o "C:\Work\Projectos2021\Puzzle\words-english\result.txt"
            /*
            CommandLinePayloadLoader cmdLoader = new CommandLinePayloadLoader(args, 4);
            string errors = "";
            if (cmdLoader.IsValid(out errors))
            {
                JobPayload payload = cmdLoader.LoadJob();
                //var _startWord = "bard";
                //var _finalWord = "chaw";
                var words = File.ReadLines(payload.SourceFilePath);
                TmpProcessingService _service = new TmpProcessingService();

                _service.ProcessTwo(words.Select(e => e.ToLower()).ToList(), payload);
            }
            else {
                Console.WriteLine(errors);
            }
            */
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
                    services.AddSingleton<IPublisher, FileSystemPublisher>();
                })
            ;
    }
}
