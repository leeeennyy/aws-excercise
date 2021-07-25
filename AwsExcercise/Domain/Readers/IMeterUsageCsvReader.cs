using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Readers
{
    public interface IMeterUsageCsvReader
    {
        public Task<List<MeterUsage>> GetMeterUsagesFromS3Bucket(string filename);

        public List<MeterUsage> GetMeterUsagesFromStream(Stream stream);
    }
}
