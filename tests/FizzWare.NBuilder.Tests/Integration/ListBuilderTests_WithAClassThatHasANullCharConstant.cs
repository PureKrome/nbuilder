using FizzWare.NBuilder.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Integration
{
    public class ListBuilderBuilderTests_WithAClassThatHasANullCharConstant
    {
        [Fact]
        public void ShouldBeAbleToCreateAListOfAClassThatHasACharConstant()
        {
            var builderSetup = new BuilderSettings();
            var list = new Builder(builderSetup).CreateListOfSize<MyClassWithCharConst>(2).Build();

            foreach (var item in list)
                item.GetNonNullCharConst().ShouldBe(MyClassWithCharConst.NonNullCharConst);
        }

        [Fact]
        public void ShouldBeAbleToCreateAListOfAClassThatHasANullCharConstant()
        {
            var builderSetup = new BuilderSettings();
            var list = new Builder(builderSetup).CreateListOfSize<MyClassWithCharConst>(2).Build();

            foreach (var item in list)
                item.GetNullCharConst().ShouldBe(MyClassWithCharConst.NullCharConst);
        }
    }
}