using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit.Picking
{
    public class BetweenConstraintTests
    {
        public BetweenConstraintTests()
        {
            uniqueRandomGenerator = Substitute.For<IUniqueRandomGenerator>();
        }

        private readonly IUniqueRandomGenerator uniqueRandomGenerator;
        private int lower;
        private int upper;

        [Fact]
        public void ShouldBeAbleToAddUpperUsingAnd()
        {
            uniqueRandomGenerator.Next(lower, upper).Returns(2);

            var constraint = new BetweenConstraint(uniqueRandomGenerator, lower);
            constraint.And(upper);

            constraint.GetEnd();
        }

        [Fact]
        public void ShouldBeAbleToUseBetweenPickerConstraint()
        {
            lower = 1;
            upper = 5;
            uniqueRandomGenerator.Next(lower, upper).Returns(2);
            var constraint = new BetweenConstraint(uniqueRandomGenerator, lower, upper);

            var end = constraint.GetEnd();

            end.ShouldBe(2);
        }
    }
}