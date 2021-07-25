using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3.Model;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;

namespace Domain.Readers
{
    public class MeterUsageCsvReader : IMeterUsageCsvReader
    {
        private readonly IMeterUsageS3Bucket _meterUsageBucket;

        public MeterUsageCsvReader(IMeterUsageS3Bucket meterUsageBucket)
        {
            _meterUsageBucket = meterUsageBucket;
        }

        public async Task<List<MeterUsage>> GetMeterUsagesFromFile(string filename)
        {
            List<MeterUsage> meterUsages = new();
            List<Exception> exceptions = new();

            using (GetObjectResponse fileResponse = await _meterUsageBucket.GetFile(filename))
            using (Stream stream = fileResponse.ResponseStream)
            using (StreamReader sr = new StreamReader(stream))
            {
                string line = sr.ReadLine(); // (headers)
                int row = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    row++;
                    string[] meterUsageData = line.Split(',');

                    if (meterUsageData.Length != 50)
                    {
                        exceptions.Add(new InvalidRowException($"Could not parse row {row}."));
                        continue;
                    }

                    try
                    {
                        DateTime date = DateTime.Parse(meterUsageData[1]);

                        int timeSlice = 0;
                        int totalUsage = 0;
                        for (int i = 2; i < meterUsageData.Length; i++)
                        {
                            MeterUsage meterUsage = new MeterUsage
                            {
                                Meter = meterUsageData[0],
                                DateTime = date.AddMinutes(timeSlice * 30),
                                Usage = int.Parse(meterUsageData[i])
                            };

                            totalUsage += meterUsage.Usage;
                            meterUsages.Add(meterUsage);
                        }

                        Console.WriteLine($"{meterUsageData[0]} {date} {totalUsage}");
                    }
                    catch (Exception)
                    {
                        exceptions.Add(new InvalidRowException($"Could not parse row {row}."));
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }

            return meterUsages;
        }
    }
}
