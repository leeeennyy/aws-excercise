using System.Threading.Tasks;

namespace CSVProcessor
{
    public interface ICsvProcessor
    {
        public Task ProcessFile(string filepath);
    }
}
