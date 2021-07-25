using System;
using System.IO;
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
                List<MeterUsage> meterUsages;
                // TODO: Check if the file has been processed before/has the same tags/etags from database
                // TODO: Change to read the file from s3 bucket
                using (Stream stream = File.OpenRead(@"C:\Dev\aws-excercise\consumption.csv"))
                {
                    meterUsages = _meterUsageCsvReader.GetMeterUsagesFromStream(stream);
                }

                foreach (var meterUsage in meterUsages)
                {
                    Console.WriteLine($"{meterUsage.Meter} {meterUsage.DateTime} {meterUsage.Usage}");
                }

                // TODO: Store usages into a database
            }
            catch (Exception ex)
            {
                // TODO: Log error and mark the whole file as invalid
            }
        }
    }
}
