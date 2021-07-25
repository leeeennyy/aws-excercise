using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Readers
{
    public interface IMeterUsageCsvReader
    {
        public Task<List<MeterUsage>> GetMeterUsagesFromFile(string filename);
    }
}
