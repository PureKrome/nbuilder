﻿using System.Linq;
using FizzWare.NBuilder.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class ExtensibilityTests
    {
        private const string theString = "test";

        [Fact]
        public void ShouldBeAbleToAddCustomExtension()
        {
            var builderSetup = new BuilderSettings();
            var list = Builder<MyClass>.CreateListOfSize(10).AllEven().With(x => x.StringOne = theString).Build();
            list.Count(x => x.StringOne == theString).ShouldBe(5);
        }
    }
}