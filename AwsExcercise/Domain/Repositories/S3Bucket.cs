using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Domain.Configurations;

namespace Domain.Repositories
{
    public abstract class S3Bucket : IS3Bucket
    {
        private readonly AwsConfiguration _config;
        protected abstract string BucketName { get; }

        public S3Bucket(AwsConfiguration awsConfigOptions)
        {
            _config = awsConfigOptions;
        }

        public AmazonS3Client CreateS3Client()
        {
            return new AmazonS3Client(_config.AccessKeyId, _config.AccessKeySecret, S3Region.APSoutheast2);
        }

        public async Task<GetObjectResponse> GetFile(string fileName)
        {
            using (AmazonS3Client client = CreateS3Client())
            {
                return await client.GetObjectAsync(BucketName, fileName);
            }
        }
    }
}
