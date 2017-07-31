using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit.Picking
{
    public class UpToConstraintTests
    {
        public UpToConstraintTests()
        {
            uniqueRandomGenerator = Substitute.For<IUniqueRandomGenerator>();
        }

        private readonly IUniqueRandomGenerator uniqueRandomGenerator;
        private const int count = 5;

        [Fact]
        public void ShouldBeAbleToUseBetweenPickerConstraint()
        {
            var constraint = new UpToConstraint(uniqueRandomGenerator, count);

            uniqueRandomGenerator.Next(0, count).Returns(1);
            var end = constraint.GetEnd();

            end.ShouldBe(1);
        }
    }
}