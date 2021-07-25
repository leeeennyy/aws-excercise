using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace Domain.Repositories
{
    public interface IS3Bucket
    {
        AmazonS3Client CreateS3Client();

        Task<GetObjectResponse> GetFile(string fileName);
    }
}
