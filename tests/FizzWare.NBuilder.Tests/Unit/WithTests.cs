

namespace FizzWare.NBuilder.Tests.Unit
{
    
    public class WithTests
    {
        [Fact]
        public void ShouldBeAbleToUseWith_Between_LowerAndUpper()
        {
            var constraint = With.Between(1, 5);
            constraint, Is.TypeOf(typeof(BetweenConstraint));
        }

        [Fact]
        public void ShouldBeAbleToUseWith_Between_LowerOnly()
        {
            var constraint = With.Between(1);
            constraint, Is.TypeOf(typeof(BetweenConstraint));
        }

        [Fact]
        public void ShouldBeAbleToUseWith_Exactly()
        {
            var constraint = With.Exactly(1);
            constraint, Is.TypeOf(typeof(ExactlyConstraint));
        }

        [Fact]
        public void ShouldBeAbleToUseWith_UpTo()
        {
            var constraint = With.UpTo(1);
            constraint, Is.TypeOf(typeof(UpToConstraint));
        }
    }
}