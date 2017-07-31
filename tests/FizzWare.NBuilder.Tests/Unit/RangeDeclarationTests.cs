using System.Collections.Generic;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class RangeDeclarationTests
    {
        public RangeDeclarationTests()
        {
            listBuilderImpl = Substitute.For<IListBuilderImpl<SimpleClass>>();
            objectBuilder = Substitute.For<IObjectBuilder<SimpleClass>>();
        }

        private Declaration<SimpleClass> declaration;
        private readonly IObjectBuilder<SimpleClass> objectBuilder;
        private readonly IListBuilderImpl<SimpleClass> listBuilderImpl;

        [Fact]
        public void DeclarationShouldAddToMasterListInCorrectPlace()
        {
            var masterList = new SimpleClass[19];
            var obj1 = new SimpleClass();
            var obj2 = new SimpleClass();

            {
                listBuilderImpl.BuilderSettings.Returns(new BuilderSettings());
                objectBuilder.BuilderSettings.Returns(new BuilderSettings());
                objectBuilder.Construct(9).Returns(obj1);
                objectBuilder.Construct(10).Returns(obj2);
            }

            declaration = new RangeDeclaration<SimpleClass>(listBuilderImpl, objectBuilder, 9, 10);
            declaration.Construct();
            declaration.AddToMaster(masterList);

            masterList[9].ShouldBe(obj1);
            masterList[10].ShouldBe(obj2);
        }

        [Fact]
        public void DeclarationShouldUseObjectBuilderToConstructItems()
        {
            {
                listBuilderImpl.BuilderSettings.Returns(new BuilderSettings());
                objectBuilder.BuilderSettings.Returns(new BuilderSettings());

                objectBuilder.Construct(Arg.Any<int>()).Returns(new SimpleClass());
            }

            {
                declaration = new RangeDeclaration<SimpleClass>(listBuilderImpl, objectBuilder, 0, 9);

                declaration.Construct();
            }
        }

        [Fact]
        public void ShouldBeAbleToUseAll()
        {
            {
                listBuilderImpl.BuilderSettings.Returns(new BuilderSettings());
                objectBuilder.BuilderSettings.Returns(new BuilderSettings());
                listBuilderImpl.All().Returns(declaration);

                declaration = new RangeDeclaration<SimpleClass>(listBuilderImpl, objectBuilder, 9, 10);

                declaration.All();
            }
        }

        [Fact]
        public void ShouldCallFunctionsOnItemsInTheMasterList()
        {
            var masterList = Substitute.For<IList<SimpleClass>>();

            {
                masterList[4].Returns((SimpleClass) null);
                masterList[5].Returns((SimpleClass) null);
            }

            {
                declaration = new RangeDeclaration<SimpleClass>(listBuilderImpl, objectBuilder, 9, 11);
                declaration.MasterListAffectedIndexes = new List<int> {4, 5};
                declaration.CallFunctions(masterList);
            }

            objectBuilder.Received().CallFunctions(null, 0);
        }

        [Fact]
        public void ShouldRecordMasterListKeys()
        {
            var masterList = new SimpleClass[19];

            objectBuilder.Construct(Arg.Any<int>()).Returns(new SimpleClass());

            declaration = new RangeDeclaration<SimpleClass>(listBuilderImpl, objectBuilder, 9, 10);
            declaration.Construct();

            declaration.AddToMaster(masterList);

            declaration.MasterListAffectedIndexes.Count.ShouldBe(2);
            declaration.MasterListAffectedIndexes[0].ShouldBe(9);
            declaration.MasterListAffectedIndexes[1].ShouldBe(10);
        }
    }
}