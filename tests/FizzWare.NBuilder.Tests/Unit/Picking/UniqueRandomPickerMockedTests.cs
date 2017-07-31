using System.Collections.Generic;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit.Picking
{
    public class UniqueRandomPickerMockedTests
    {
        public UniqueRandomPickerMockedTests()
        {
            constraint = Substitute.For<IConstraint>();
            uniqueRandomGenerator = Substitute.For<IUniqueRandomGenerator>();
        }

        private readonly IConstraint constraint;
        private readonly IUniqueRandomGenerator uniqueRandomGenerator;


        [Fact]
        public void UniqueRandomPickerShouldBeAbleToPickFromList()
        {
            var list = Substitute.For<IList<MyClass>>();
            var picker = new UniqueRandomPicker<MyClass>(constraint, uniqueRandomGenerator);

            var capacity = 10;
            var randomIndex = 3;
            var end = 2;

            list.Count.Returns(capacity);
            constraint.GetEnd().Returns(end);
            uniqueRandomGenerator.Next(0, capacity).Returns(randomIndex);
            list[randomIndex].Returns(new MyClass());

            picker.From(list);

            uniqueRandomGenerator.Received().Reset();
        }
    }
}