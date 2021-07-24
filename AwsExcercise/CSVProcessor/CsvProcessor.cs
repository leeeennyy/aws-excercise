using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CSVProcessor
{
    public class CsvProcessor : ICsvProcessor
    {
        private readonly IMeterRepository _meterRepository;

        public CsvProcessor()
        {
        }

        public async Task ProcessFile(string filepath)
        {
            List<MeterUsage> meterUsages = new();

            filepath = @"C:\Dev\aws-excercise\consumption.csv";
            using (StreamReader sr = new StreamReader(filepath))
            {
                string line = sr.ReadLine(); // (headers)
                while ((line = sr.ReadLine()) != null)
                {
                    string[] meterUsageData = line.Split(',');

                    Meter meter = new Meter
                    {
                        MeterId = new Guid(),
                        MeterName = meterUsageData[0]
                    };//await _meterRepository.GetMeterFromMeterName(meterUsageData[0]);
                    DateTime date = DateTime.Parse(meterUsageData[1]);

                    int timeSlice = 0;
                    int totalUsage = 0;
                    for (int i = 2; i < meterUsageData.Length; i++)
                    {
                        MeterUsage meterUsage = new MeterUsage
                        {
                            MeterId = meter.MeterId,
                            DateTime = date.AddMinutes(timeSlice * 30),
                            Usage = int.Parse(meterUsageData[i])
                        };

                        totalUsage += meterUsage.Usage;
                        meterUsages.Add(meterUsage);
                    }

                    Console.WriteLine($"{meter} {date} {totalUsage}");
                }
            }
        }
    }
}
