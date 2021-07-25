using NUnit.Framework;
using Domain.Exceptions;
using Domain.Models;
using Domain.Readers;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace Domain.Tests
{
    public class MeterUsageCsvRowReaderTests
    {
        [Test]
        public void GetMeterUsagesFromRow_ReturnsMeterUsages()
        {
            // ARRANGE
            string row = "EE00011,1/01/2019,9,8,8,10,4,10,10,8,6,5,4,9,9,6,7,10,4,6,4,5,7,10,7,7,10,9,10,8,8,5,5,9,7,10,10,7,4,8,10,10,5,10,8,10,8,6,7,8";

            IMeterUsageCsvRowReader reader = new MeterUsageCsvRowReader();

            // ACT
            List<MeterUsage> usages = reader.GetMeterUsagesFromRow(row, 1);

            // ASSERT
            usages.Count.Should().Be(48);
        }

        [Test]
        public void GetMeterUsagesFromRow_WithInvalidColumnLength_ThrowsError()
        {
            // ARRANGE
            string row = "EE00011,1/01/2019,9,8,8,10,4,10,10,8,6,5,4,9,9,6,7,10,4,6,4,5,7,10,7,7,10,9,10,8,8,9,7,10,10,7,4,8,10,10,5,10,8,10,8,6,7,8";

            IMeterUsageCsvRowReader reader = new MeterUsageCsvRowReader();

            // ACT
            Action action = () => reader.GetMeterUsagesFromRow(row, 1);

            // ASSERT
            action.Should().Throw<InvalidRowException>()
                .WithMessage("Invalid column length in row 1.");
        }

        [Test]
        public void GetMeterUsagesFromRow_WithInvalidUsage_ThrowsError()
        {
            // ARRANGE
            string row = "EE00011,1/01/2019,abc,8,8,10,4,10,10,8,6,5,4,9,9,6,7,10,4,6,4,5,7,10,7,7,10,9,10,8,8,5,5,9,7,10,10,7,4,8,10,10,5,10,8,10,8,6,7,8";

            IMeterUsageCsvRowReader reader = new MeterUsageCsvRowReader();

            // ACT
            Action action = () => reader.GetMeterUsagesFromRow(row, 1);

            // ASSERT
            action.Should().Throw<InvalidRowException>()
                .WithMessage("Could not parse row 1.");
        }
    }
}