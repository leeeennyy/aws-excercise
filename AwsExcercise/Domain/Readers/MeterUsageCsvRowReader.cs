using System;
using System.Collections.Generic;
using Domain.Exceptions;
using Domain.Models;

namespace Domain.Readers
{
    public class MeterUsageCsvRowReader : IMeterUsageCsvRowReader
    {
        public List<MeterUsage> GetMeterUsagesFromRow(string csvRow, int rowNumber)
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
