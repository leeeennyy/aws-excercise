using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Domain.Configurations;
using Domain.Readers;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CSVProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceProvider serviceProvider = RegisterServices();
            ICsvProcessor csvProcessor = serviceProvider.GetService<ICsvProcessor>();
            Task t = csvProcessor.ProcessFile("");
            t.Wait();
        }

        private static ServiceProvider RegisterServices()
        {
            ServiceCollection serviceProvider = new ServiceCollection();
            serviceProvider.AddTransient<ICsvProcessor, CsvProcessor>();
            serviceProvider.AddTransient<IMeterUsageCsvReader, MeterUsageCsvReader>();
            serviceProvider.AddTransient<IMeterUsageS3Bucket, MeterUsageS3Bucket>();

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            AwsConfiguration awsConfig = new();
            config.Bind(nameof(AwsConfiguration), awsConfig);
            serviceProvider.AddSingleton(awsConfig);

            return serviceProvider.BuildServiceProvider();
        }
    }
}
