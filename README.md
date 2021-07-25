# aws-excercise

## Done
1. Logic to process the file
1. Some unit tests around the reader

## Not done
1. IAC - tried to do some via CDK
1. API to read / sum the data for a date; spent too much time trying to get IAC/S3 bucket working.

## TODO with more time
1. Read CSV file from S3 bucket
1. Implement batching either via S3 batch or AWS Batch; this should trigger Lambdas for CsvProcessor to process each file.
1. Full IAC to deploy S3 bucket, batch jobs, lambdas, Docker repo, docker containers
1. CI/CD pipelines
1. More tests
