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

        public async Task<List<MeterUsage>> GetMeterUsagesFromS3Bucket(string filename)
        {
            using (GetObjectResponse fileResponse = await _meterUsageBucket.GetFile(filename))
            using (Stream stream = fileResponse.ResponseStream)
            {
                return GetMeterUsagesFromStream(stream);
            }
        }

        public List<MeterUsage> GetMeterUsagesFromStream(Stream stream)
        {
            List<MeterUsage> meterUsages = new();
            List<Exception> exceptions = new();

            using (StreamReader sr = new StreamReader(stream))
            {
                string line = sr.ReadLine(); // (headers)
                int row = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    row++;
                    try
                    {
                        meterUsages.AddRange(GetMeterUsagesFromRow(line, row));
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }

            return meterUsages;
        }

        private List<MeterUsage> GetMeterUsagesFromRow(string csvRow, int rowNumber)
        {
            List<MeterUsage> meterUsages = new();
            string[] meterUsageData = csvRow.Split(',');

            if (meterUsageData.Length != 50)
            {
                throw new InvalidRowException($"Invalid column length in row {rowNumber}.");
            }

            try
            {
                DateTime date = DateTime.Parse(meterUsageData[1]);

                int timeSlice = 0;
                for (int i = 2; i < meterUsageData.Length; i++)
                {
                    MeterUsage meterUsage = new MeterUsage
                    {
                        Meter = meterUsageData[0],
                        DateTime = date.AddMinutes(timeSlice * 30),
                        Usage = int.Parse(meterUsageData[i])
                    };
                    meterUsages.Add(meterUsage);
                }
            }
            catch (Exception)
            {
                throw new InvalidRowException($"Could not parse row {rowNumber}.");
            }

            return meterUsages;
        }
    }
}
