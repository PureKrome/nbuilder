using System;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class SequentialGeneratorDateTimeTests
    {
        public SequentialGeneratorDateTimeTests()
        {
            generator = new SequentialGenerator<DateTime>();
            startingValue = new DateTime(9, 9, 9, 9, 9, 9, 9);
            generator.StartingWith(startingValue);
        }

        private SequentialGenerator<DateTime> generator;
        private DateTime startingValue;

        [Fact]
        public void Generate_DaysDecrement_DecrementsByDays()
        {
            var oneDay = startingValue.AddDays(-1);
            var twoDays = startingValue.AddDays(-2);

            generator.IncrementDateBy = IncrementDate.Day;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(oneDay);
            generator.Generate().ShouldBe(twoDays);
        }

        [Fact]
        public void Generate_DaysIncrement_IncrementsByDays()
        {
            var oneDay = startingValue.AddDays(1);
            var twoDays = startingValue.AddDays(2);

            generator.IncrementDateBy = IncrementDate.Day;
            generator.Generate();

            generator.Generate().ShouldBe(oneDay);
            generator.Generate().ShouldBe(twoDays);
        }

        [Fact]
        public void Generate_DefaultIncrement_IncrementsByDay()
        {
            var oneDay = startingValue.AddDays(1);
            var twoDays = startingValue.AddDays(2);

            generator.Generate();

            generator.Generate().ShouldBe(oneDay);
            generator.Generate().ShouldBe(twoDays);
        }

        [Fact]
        public void Generate_DefaultSetUp_IncrementsFromMinDateTimeValue()
        {
            generator = new SequentialGenerator<DateTime>();
            generator.Generate().ShouldBe(DateTime.MinValue);
        }

        [Fact]
        public void Generate_HoursDecrement_DecrementsByHours()
        {
            var oneHour = startingValue.AddHours(-1);
            var twoHours = startingValue.AddHours(-2);

            generator.IncrementDateBy = IncrementDate.Hour;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(oneHour);
            generator.Generate().ShouldBe(twoHours);
        }

        [Fact]
        public void Generate_HoursIncrement_IncrementsByHours()
        {
            var oneHour = startingValue.AddHours(1);
            var twoHours = startingValue.AddHours(2);

            generator.IncrementDateBy = IncrementDate.Hour;
            generator.Generate();

            generator.Generate().ShouldBe(oneHour);
            generator.Generate().ShouldBe(twoHours);
        }

        [Fact]
        public void Generate_MillisecondsDecrement_DecrementsByMilliseconds()
        {
            var oneMillisecond = startingValue.AddMilliseconds(-1);
            var twoMilliseconds = startingValue.AddMilliseconds(-2);

            generator.IncrementDateBy = IncrementDate.Millisecond;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(oneMillisecond);
            generator.Generate().ShouldBe(twoMilliseconds);
        }

        [Fact]
        public void Generate_MillisecondsIncrement_IncrementsByMilliseconds()
        {
            var oneMillisecond = startingValue.AddMilliseconds(1);
            var twoMilliseconds = startingValue.AddMilliseconds(2);

            generator.IncrementDateBy = IncrementDate.Millisecond;
            generator.Generate();

            generator.Generate().ShouldBe(oneMillisecond);
            generator.Generate().ShouldBe(twoMilliseconds);
        }

        [Fact]
        public void Generate_MinutesDecrement_DecrementsByMinutes()
        {
            var oneMinute = startingValue.AddMinutes(-1);
            var twoMinutes = startingValue.AddMinutes(-2);

            generator.IncrementDateBy = IncrementDate.Minute;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(oneMinute);
            generator.Generate().ShouldBe(twoMinutes);
        }

        [Fact]
        public void Generate_MinutesIncrement_IncrementsByMinutes()
        {
            var oneMinute = startingValue.AddMinutes(1);
            var twoMinutes = startingValue.AddMinutes(2);

            generator.IncrementDateBy = IncrementDate.Minute;
            generator.Generate();

            generator.Generate().ShouldBe(oneMinute);
            generator.Generate().ShouldBe(twoMinutes);
        }

        [Fact]
        public void Generate_MonthsDecrement_DecrementsByMonths()
        {
            var oneMonth = startingValue.AddMonths(-1);
            var twoMonths = startingValue.AddMonths(-2);

            generator.IncrementDateBy = IncrementDate.Month;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(oneMonth);
            generator.Generate().ShouldBe(twoMonths);
        }

        [Fact]
        public void Generate_MonthsIncrement_IncrementsByMonths()
        {
            var oneMonth = startingValue.AddMonths(1);
            var twoMonths = startingValue.AddMonths(2);

            generator.IncrementDateBy = IncrementDate.Month;
            generator.Generate();

            generator.Generate().ShouldBe(oneMonth);
            generator.Generate().ShouldBe(twoMonths);
        }

        [Fact]
        public void Generate_MultiValueDecrement_AllowsDatesToBeDecrementedByValuesGreaterThanOne()
        {
            const double increment = 2;

            var expectedIncrementedDate = startingValue.AddMilliseconds(-increment);

            generator.IncrementDateBy = IncrementDate.Millisecond;
            generator.IncrementDateValueBy = increment;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(expectedIncrementedDate);
        }

        [Fact]
        public void Generate_MultiValueIncrement_AllowsDatesToBeIncrementedByValuesGreaterThanOne()
        {
            const double increment = 2;

            var expectedIncrementedDate = startingValue.AddMilliseconds(increment);

            generator.IncrementDateBy = IncrementDate.Millisecond;
            generator.IncrementDateValueBy = increment;
            generator.Generate();

            generator.Generate().ShouldBe(expectedIncrementedDate);
        }

        [Fact]
        public void Generate_SecondsDecrement_DecrementsBySeconds()
        {
            var oneSecond = startingValue.AddSeconds(-1);
            var twoSeconds = startingValue.AddSeconds(-2);

            generator.IncrementDateBy = IncrementDate.Second;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(oneSecond);
            generator.Generate().ShouldBe(twoSeconds);
        }

        [Fact]
        public void Generate_SecondsIncrement_IncrementsBySeconds()
        {
            var oneSecond = startingValue.AddSeconds(1);
            var twoSeconds = startingValue.AddSeconds(2);

            generator.IncrementDateBy = IncrementDate.Second;
            generator.Generate();

            generator.Generate().ShouldBe(oneSecond);
            generator.Generate().ShouldBe(twoSeconds);
        }

        [Fact]
        public void Generate_TicksDecrement_AllowsDecrementingByTicks()
        {
            var oneTick = startingValue.AddTicks(-1);
            var twoTicks = startingValue.AddTicks(-2);

            generator.IncrementDateBy = IncrementDate.Tick;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().Ticks.ShouldBe(oneTick.Ticks);
            generator.Generate().ShouldBe(twoTicks);
        }

        [Fact]
        public void Generate_TicksIncrement_AllowsIncrementingByTicks()
        {
            var oneTick = startingValue.AddTicks(1);
            var twoTicks = startingValue.AddTicks(2);

            generator.IncrementDateBy = IncrementDate.Tick;
            generator.Generate();

            generator.Generate().Ticks.ShouldBe(oneTick.Ticks);
            generator.Generate().ShouldBe(twoTicks);
        }

        [Fact]
        public void Generate_YearsDecrement_DecrementsByYears()
        {
            var oneYear = startingValue.AddYears(-1);
            var twoYears = startingValue.AddYears(-2);

            generator.IncrementDateBy = IncrementDate.Year;
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate();

            generator.Generate().ShouldBe(oneYear);
            generator.Generate().ShouldBe(twoYears);
        }

        [Fact]
        public void Generate_YearsIncrement_IncrementsByYears()
        {
            var oneYear = startingValue.AddYears(1);
            var twoYears = startingValue.AddYears(2);

            generator.IncrementDateBy = IncrementDate.Year;
            generator.Generate();

            generator.Generate().ShouldBe(oneYear);
            generator.Generate().ShouldBe(twoYears);
        }

        // TODO FIX
#if !SILVERLIGHT
        [Fact]
        public void Generate_IncrementDaysMoreThanMaximumAllowedValue_ThrowsException()
        {
            generator.StartingWith(DateTime.MaxValue);
            generator.IncrementDateBy = IncrementDate.Day;
            generator.Generate();
            Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate());
        }

        [Fact]
        public void Generate_IncrementTicksMoreThanMaximumAllowedValue_ThrowsException()
        {
            generator.StartingWith(DateTime.MaxValue);
            generator.IncrementDateBy = IncrementDate.Tick;
            generator.Generate();
            Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate());
        }

        [Fact]
        public void Generate_IncrementMonthsMoreThanMaximumAllowedValue_ThrowsException()
        {
            generator.StartingWith(DateTime.MaxValue);
            generator.IncrementDateBy = IncrementDate.Month;
            generator.Generate();
            Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate());
        }

        [Fact]
        public void Generate_IncrementYearsMoreThanMaximumAllowedValue_ThrowsException()
        {
            generator.StartingWith(DateTime.MaxValue);
            generator.IncrementDateBy = IncrementDate.Year;
            generator.Generate();
            Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate());
        }

#endif
    }
}