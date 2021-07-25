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
            //try
            //{
                List<MeterUsage> meterUsages = await _meterUsageCsvReader.GetMeterUsagesFromFile("consumption.csv");
                foreach (var meterUsage in meterUsages)
                {
                    Console.WriteLine($"{meterUsage.Meter} {meterUsage.DateTime} {meterUsage.Usage}");
                }

                // TODO: Store them into a database in cloud
            //}
            //catch (Exception ex)
            //{
            //    // TODO: Log error and mark the whole file as invalid
            //}

            //filepath = @"C:\Dev\aws-excercise\consumption.csv";
            //using (StreamReader sr = new StreamReader(filepath))
            //{
            //    string line = sr.ReadLine(); // (headers)
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        string[] meterUsageData = line.Split(',');
            //        DateTime date = DateTime.Parse(meterUsageData[1]);

            //        int timeSlice = 0;
            //        int totalUsage = 0;
            //        for (int i = 2; i < meterUsageData.Length; i++)
            //        {
            //            MeterUsage meterUsage = new MeterUsage
            //            {
            //                Meter = meterUsageData[0],
            //                DateTime = date.AddMinutes(timeSlice * 30),
            //                Usage = int.Parse(meterUsageData[i])
            //            };

            //            totalUsage += meterUsage.Usage;
            //            meterUsages.Add(meterUsage);
            //        }

            //        Console.WriteLine($"{meterUsageData[0]} {date} {totalUsage}");
            //    }
            //}
        }
    }
}
