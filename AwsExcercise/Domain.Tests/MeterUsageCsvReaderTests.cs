using NUnit.Framework;
using Domain.Exceptions;
using Domain.Models;
using Domain.Readers;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Linq;
using Amazon.S3.Model;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class MeterUsageCsvReaderTests
    {
        public Mock<IMeterUsageS3Bucket> MeterUsageS3BucketMock;
        public Mock<IMeterUsageCsvRowReader> MeterUsageCsvRowReaderMock;

        [SetUp]
        public void Setup()
        {
            MeterUsageS3BucketMock = new Mock<IMeterUsageS3Bucket>();
            MeterUsageCsvRowReaderMock = new Mock<IMeterUsageCsvRowReader>();
        }

        [Test]
        public void GetMeterUsagesFromStream_ReturnsMeterUsages()
        {
            // ARRANGE
            MeterUsage m = new MeterUsage
            {
                Meter = "E0011",
                DateTime = DateTime.Now,
                Usage = 12
            };
            SetupGetMeterUsagesForRowReturns(1, m);

            IMeterUsageCsvReader reader = new 
                MeterUsageCsvReader(MeterUsageS3BucketMock.Object, MeterUsageCsvRowReaderMock.Object);

            // ACT
            List<MeterUsage> actual;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Yay\r\nYay")))
            {
                actual = reader.GetMeterUsagesFromStream(stream);
            }

            // ASSERT
            actual.Count.Should().Be(1);
            actual[0].DateTime.Should().Be(m.DateTime);
            MeterUsageCsvRowReaderMock
                .Verify(x => x.GetMeterUsagesFromRow(It.IsAny<string>(), 1), Times.Once);
        }

        [Test]
        public void GetMeterUsagesFromStream_HasError_ThrowsAggregateException()
        {
            // ARRANGE
            MeterUsage m = new MeterUsage
            {
                Meter = "E0011",
                DateTime = DateTime.Now,
                Usage = 12
            };
            SetupGetMeterUsagesForRowReturns(1, m);
            SetupGetMeterUsagesForRowThrowsError(2);

            IMeterUsageCsvReader reader = new
                MeterUsageCsvReader(MeterUsageS3BucketMock.Object, MeterUsageCsvRowReaderMock.Object);

            // ACT / ASSERT
            Action action;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Yay\r\nYay\r\nErrorrorw")))
            {
                action = () => reader.GetMeterUsagesFromStream(stream);
                action.Should().Throw<AggregateException>()
                    .WithInnerException<InvalidRowException>()
                    .WithMessage("Error");
                MeterUsageCsvRowReaderMock
                    .Verify(x => x.GetMeterUsagesFromRow(It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(2));
            }
        }

        [Test]
        public async Task GetMeterUsagesFromS3Bucket_IsSuccessful()
        {
            // ARRANGE
            MeterUsage m = new MeterUsage
            {
                Meter = "E0011",
                DateTime = DateTime.Now,
                Usage = 12
            };
            SetupGetMeterUsagesForRowReturns(1, m);
            GetObjectResponse response = new GetObjectResponse
            {
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes("Yay\r\nYay"))
            };
            MeterUsageS3BucketMock
                .Setup(x => x.GetFile(It.IsAny<string>()))
                .ReturnsAsync(response);

            string filename = "nicefilename";
            IMeterUsageCsvReader reader = new
                MeterUsageCsvReader(MeterUsageS3BucketMock.Object, MeterUsageCsvRowReaderMock.Object);

            // ACT
            List<MeterUsage> actual = await reader.GetMeterUsagesFromS3Bucket(filename);

            // ASSERT
            MeterUsageS3BucketMock.Verify(x => x.GetFile(filename), Times.Once);
            actual.Count.Should().Be(1);
            actual[0].DateTime.Should().Be(m.DateTime);
        }

        private void SetupGetMeterUsagesForRowReturns(int rowNumber, params MeterUsage[] meterUsages)
        {
            MeterUsageCsvRowReaderMock
                .Setup(x => x.GetMeterUsagesFromRow(It.IsAny<string>(), rowNumber))
                .Returns(meterUsages.Length == 0 ? new List<MeterUsage>() : meterUsages.ToList());
        }

        private void SetupGetMeterUsagesForRowThrowsError(int rowNumber)
        {
            MeterUsageCsvRowReaderMock
                .Setup(x => x.GetMeterUsagesFromRow(It.IsAny<string>(), rowNumber))
                .Throws(new InvalidRowException("Error"));
        }
    }
}