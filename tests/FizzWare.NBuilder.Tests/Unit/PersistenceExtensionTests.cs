using System;
using System.Collections.Generic;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    // ReSharper disable InvokeAsExtensionMethod

    public class PersistenceExtensionTests
    {
        public PersistenceExtensionTests()
        {
            persistenceService = Substitute.For<IPersistenceService>();
            listBuilderImpl = Substitute.For<IListBuilderImpl<MyClass>>();
            operable = Substitute.For<IOperable<MyClass>, IDeclaration<MyClass>>();
            singleObjectBuilder = Substitute.For<ISingleObjectBuilder<MyClass>>();

            theList = new List<MyClass>();
        }

        private readonly IPersistenceService persistenceService;
        private readonly IOperable<MyClass> operable;
        private readonly IListBuilderImpl<MyClass> listBuilderImpl;
        private readonly ISingleObjectBuilder<MyClass> singleObjectBuilder;
        private readonly IList<MyClass> theList;

        [Fact]
        public void Persist_TypeOfIOperableOnlyNotIDeclaration_ThrowsException()
        {
            var operableOnly = Substitute.For<IOperable<MyClass>>();

            {
                Assert.Throws<ArgumentException>(() => PersistenceExtensions.Persist(operableOnly));
            }
        }

        [Fact]
        public void ShouldBeAbleToPersistFromADeclaration()
        {
            var builderSetup = new BuilderSettings();
            {
                listBuilderImpl.BuilderSettings.Returns(builderSetup);

                listBuilderImpl.Build().Returns(theList);
                listBuilderImpl.BuilderSettings.Returns(builderSetup);
                persistenceService.Create(theList);
                ((IDeclaration<MyClass>) operable).ListBuilderImpl.Returns(listBuilderImpl);
            }

            {
                builderSetup.SetPersistenceService(persistenceService);
                PersistenceExtensions.Persist(operable);
            }
        }

        [Fact]
        public void ShouldBeAbleToPersistUsingListBuilder()
        {
            var builderSetup = new BuilderSettings();
            {
                listBuilderImpl.BuilderSettings.Returns(builderSetup);

                listBuilderImpl.Build().Returns(theList);
                persistenceService.Create(theList);
            }


            {
                builderSetup.SetPersistenceService(persistenceService);
                PersistenceExtensions.Persist(listBuilderImpl);
            }
        }

        [Fact]
        public void ShouldBeAbleToPersistUsingSingleObjectBuilder()
        {
            var builderSetup = new BuilderSettings();
            var obj = new MyClass();

            {
                singleObjectBuilder.BuilderSettings.Returns(builderSetup);

                singleObjectBuilder.Build().Returns(obj);
                persistenceService.Create(obj);
            }

            {
                builderSetup.SetPersistenceService(persistenceService);

                PersistenceExtensions.Persist(singleObjectBuilder);
            }
        }
    }
    // ReSharper restore InvokeAsExtensionMethod
}