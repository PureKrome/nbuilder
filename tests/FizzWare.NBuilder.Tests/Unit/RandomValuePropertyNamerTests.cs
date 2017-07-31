using System.Collections.Generic;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class RandomValuePropertyNamerTests
    {
        public RandomValuePropertyNamerTests()
        {
            var builderSetup = new BuilderSettings();
            reflectionUtil = Substitute.For<IReflectionUtil>();
            generator = Substitute.For<IRandomGenerator>();
            propertyNamer = new RandomValuePropertyNamer(generator, reflectionUtil, false, builderSetup);
        }

        private readonly RandomValuePropertyNamer propertyNamer;
        private readonly IReflectionUtil reflectionUtil;
        private readonly IRandomGenerator generator;

        [Fact]
        public void SetValuesOfAllIn_ListOfTypeWithPrivateSetOnlyProperty_ValueIsNotSet()
        {
            var privateSetOnlyType = new MyClassWithGetOnlyPropertySpy();

            propertyNamer.SetValuesOfAllIn(new List<MyClassWithGetOnlyPropertySpy> {privateSetOnlyType});

            privateSetOnlyType.IsSet.ShouldBeFalse();
        }
    }
}