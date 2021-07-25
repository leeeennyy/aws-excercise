using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3.Model;
using Domain.Models;
using Domain.Repositories;

namespace Domain.Readers
{
    public class MeterUsageCsvReader : IMeterUsageCsvReader
    {
        private readonly IMeterUsageS3Bucket _meterUsageBucket;
        private readonly IMeterUsageCsvRowReader _meterUsageCsvRowReader;

        public MeterUsageCsvReader(IMeterUsageS3Bucket meterUsageBucket,
            IMeterUsageCsvRowReader meterUsageCsvRowReader)
        {
            _meterUsageBucket = meterUsageBucket;
            _meterUsageCsvRowReader = meterUsageCsvRowReader;
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
                        meterUsages.AddRange(_meterUsageCsvRowReader.GetMeterUsagesFromRow(line, row));
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
    }
}
