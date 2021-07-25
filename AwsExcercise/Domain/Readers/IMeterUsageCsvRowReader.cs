using System.Collections.Generic;
using Domain.Models;

namespace Domain.Readers
{
    public interface IMeterUsageCsvRowReader
    {
        public List<MeterUsage> GetMeterUsagesFromRow(string csvRow, int rowNumber);
    }
}
