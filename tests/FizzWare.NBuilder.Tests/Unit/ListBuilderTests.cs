using System.Collections.Generic;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class ListBuilderTests
    {
        public ListBuilderTests()
        {
            reflectionUtil = Substitute.For<IReflectionUtil>();
            propertyNamer = Substitute.For<IPropertyNamer>();
        }

        private readonly IReflectionUtil reflectionUtil;
        private readonly IPropertyNamer propertyNamer;
        private readonly MyClass myClass = new MyClass();
        private const int listSize = 10;

        [Fact]
        public void Constructing_AssignsValuesToProperties()
        {
            propertyNamer.SetValuesOfAllIn(Arg.Any<IList<MyClass>>());

            var list = new List<MyClass>();

            var builder = new ListBuilder<MyClass>(listSize, propertyNamer, reflectionUtil, new BuilderSettings());

            builder.Name(list);
        }

        [Fact]
        public void ConstructShouldComplainIfTypeNotParameterlessNoAllAndSumOfItemsInDeclarationsDoNotEqualCapacity()
        {
            var declaration1 = Substitute.For<IDeclaration<MyClassWithConstructor>>();
            var declaration2 = Substitute.For<IDeclaration<MyClassWithConstructor>>();

            declaration1.NumberOfAffectedItems.Returns(2);
            declaration2.NumberOfAffectedItems.Returns(2);
            reflectionUtil.RequiresConstructorArgs(typeof(MyClass)).Returns(true);
            var builder = new ListBuilder<MyClass>(10, propertyNamer, reflectionUtil, new BuilderSettings());

            Assert.Throws<BuilderException>(() => builder.Construct());
        }

        [Fact]
        public void IfNoAllExistsAndSumOfAffectedItemsInDeclarationsIsLessThanCapacity_ShouldAddADefaultAll()
        {
            var builder = new ListBuilder<MyClass>(30, propertyNamer, reflectionUtil, new BuilderSettings());
            builder.TheFirst(10);

            reflectionUtil.RequiresConstructorArgs(typeof(MyClass)).Returns(false);

            // Even though a declaration of 10 has been added, we expect the list builder to add
            // a default GlobalDeclaration (All). Therefore we expect CreateInstanceOf to be called 40 times
            reflectionUtil.CreateInstanceOf<MyClass>().Returns(myClass);
            builder.Construct();
        }

        [Fact]
        public void ShouldBeAbleToBuildAList()
        {
            var declaration = Substitute.For<IDeclaration<MyClass>>();

            var builder = new ListBuilder<MyClass>(listSize, propertyNamer, reflectionUtil, new BuilderSettings());

            reflectionUtil.RequiresConstructorArgs(typeof(MyClass)).Returns(false);
            reflectionUtil.CreateInstanceOf<MyClass>().Returns(myClass);
            declaration.Construct();
            declaration.AddToMaster(Arg.Any<MyClass[]>());
            propertyNamer.SetValuesOfAllIn(Arg.Any<IList<MyClass>>());
            declaration.CallFunctions(Arg.Any<IList<MyClass>>());

            builder.Build();
        }

        [Fact]
        public void ShouldConstructDeclarations()
        {
            var declaration = Substitute.For<IGlobalDeclaration<MyClass>>();

            var builder = new ListBuilder<MyClass>(listSize, propertyNamer, reflectionUtil, new BuilderSettings());

            builder.AddDeclaration(declaration);

            declaration.Construct();

            builder.Construct();
        }
    }
}