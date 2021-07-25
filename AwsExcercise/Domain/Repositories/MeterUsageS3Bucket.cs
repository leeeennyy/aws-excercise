using Domain.Configurations;
using Microsoft.Extensions.Options;

namespace Domain.Repositories
{
    public class MeterUsageS3Bucket : S3Bucket, IMeterUsageS3Bucket
    {
        protected override string BucketName => "meter-usage-csvs";

        public MeterUsageS3Bucket(AwsConfiguration config)
            : base(config)
        {
        }
    }
}
