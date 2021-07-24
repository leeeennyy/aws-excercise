using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

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

            return serviceProvider.BuildServiceProvider();
        }
    }
}
