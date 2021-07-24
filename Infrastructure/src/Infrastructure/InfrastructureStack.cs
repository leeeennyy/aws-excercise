using Amazon.CDK;
using Amazon.CDK.AWS.S3;

namespace Infrastructure
{
    public class InfrastructureStack : Stack
    {
        internal InfrastructureStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            new Bucket(this, "AwsExcerciseBucket", new BucketProps
            {
                BucketName = "meter-usage-csvs"
            });
        }
    }
}
