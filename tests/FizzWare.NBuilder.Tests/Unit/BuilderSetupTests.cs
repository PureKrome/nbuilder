using System;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class BuilderSetupTests
    {
        public BuilderSetupTests()
        {
            persistenceService = Substitute.For<IPersistenceService>();
            repository = Substitute.For<IMyClassRepository>();
            builderSettings = new BuilderSettings();
            builderSettings.SetPersistenceService(persistenceService);
        }

        private readonly IPersistenceService persistenceService;
        private readonly IMyClassRepository repository;
        private readonly BuilderSettings builderSettings;

        [Fact]
        public void ShouldBeAbleToRegisterThePersistenceService()
        {
            builderSettings.GetPersistenceService().ShouldBe(persistenceService);
        }

        [Fact]
        public void ShouldBeAbleToSetCreatePersistenceMethod()
        {
            Action<MyClass> func = x => repository.Save(x);

            builderSettings.SetCreatePersistenceMethod(func);

            persistenceService.Received().SetPersistenceCreateMethod(func);
        }

        [Fact]
        public void ShouldBeAbleToSetUpdatePersistenceMethod()
        {
            Action<MyClass> func = x => repository.Save(x);

            builderSettings.SetUpdatePersistenceMethod(func);

            persistenceService.Received().SetPersistenceUpdateMethod(func);
        }
    }
}