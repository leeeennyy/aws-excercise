using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Readers;

namespace CSVProcessor
{
    public class CsvProcessor : ICsvProcessor
    {
        private readonly IMeterUsageCsvReader _meterUsageCsvReader;

        public CsvProcessor(IMeterUsageCsvReader meterUsageCsvReader)
        {
            _meterUsageCsvReader = meterUsageCsvReader;
        }

        public async Task ProcessFile(string filepath)
        {
            try
            {
                List<MeterUsage> meterUsages = await _meterUsageCsvReader.GetMeterUsagesFromFile("consumption.csv");
                foreach (var meterUsage in meterUsages)
                {
                    Console.WriteLine($"{meterUsage.Meter} {meterUsage.DateTime} {meterUsage.Usage}");
                }
                // TODO: Store them into a database in cloud
            }
            catch (Exception ex)
            {
                // TODO: Log error and mark the whole file as invalid
            }
        }
    }
}
