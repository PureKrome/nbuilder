using System.Collections.Generic;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit.Picking
{
    public class RandomItemPickerTests
    {
        public RandomItemPickerTests()
        {
            randomGenerator = Substitute.For<IRandomGenerator>();
            list = Substitute.For<IList<MyClass>>();
        }

        private readonly IRandomGenerator randomGenerator;
        private readonly IList<MyClass> list;

        [Fact]
        public void RandomItemPickerShouldHitRandomGeneratorEveryTimeAnItemIsPicked()
        {
            var zero = new MyClass();
            var one = new MyClass();

            var theList = new List<MyClass> {zero, one};

            var endIndex = theList.Count;


            randomGenerator.Next(0, endIndex).Returns(0, 1);

            var picker = new RandomItemPicker<MyClass>(theList, randomGenerator);

            picker.Pick().ShouldBe(zero);
            picker.Pick().ShouldBe(one);
        }

        [Fact]
        public void ShouldBeAbleToUseRandomItemPicker()
        {
            const int listCount = 5;
            list.Count.Returns(listCount);
            randomGenerator.Next(0, listCount).Returns(2);

            var picker = new RandomItemPicker<MyClass>(list, randomGenerator);

            // Act
            picker.Pick();

            // Assert
            //http://stackoverflow.com/questions/39610125/how-to-check-received-calls-to-indexer-with-nsubstitute
            var ignored = list.Received()[2];
        }
    }
}