using System.Threading.Tasks;

namespace CSVProcessor
{
    public interface IMeterRepository
    {
        public Task<Meter> GetMeterFromMeterName(string meterName);
    }
}
