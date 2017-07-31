﻿using System.Diagnostics;
using System.Linq;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class SequentialGeneratorTests
    {
        public SequentialGeneratorTests()
        {
            generator = new SequentialGenerator<int>();
        }

        private readonly SequentialGenerator<int> generator;

        [DebuggerDisplay("Property: {Property}")]
        public class TestClass
        {
            public string Property { get; set; }
        }

        [Fact]
        public void RangeDeclarationsShouldExecuteInOrderOfStartingPosition()
        {
            var generator = new SequentialGenerator<int>();
            var build = Builder<TestClass>
                .CreateListOfSize(10)
                .All()
                .Do(x => x.Property = "item")
                .TheFirst(2)
                .Do(x => x.Property += string.Format("{0}{1}", generator.Generate(), generator.Generate()))
                .TheNext(6)
                .Do(x => x.Property += generator.Generate())
                .Build();

            var expected = new[]
                {"item01", "item23", "item4", "item5", "item6", "item7", "item8", "item9", "item", "item"};
            var actual = build.Select(row => row.Property).ToArray();

            actual.ShouldBe(expected);
        }

        [Fact]
        public void ShouldBeAbleToGenerate()
        {
            generator.Generate().ShouldBe(0);
            generator.Generate().ShouldBe(1);
            generator.Generate().ShouldBe(2);
        }

        [Fact]
        public void ShouldBeAbleToGenerateInReverse()
        {
            generator.Direction = GeneratorDirection.Descending;
            generator.Generate().ShouldBe(0);
            generator.Generate().ShouldBe(-1);
            generator.Generate().ShouldBe(-2);
            generator.Generate().ShouldBe(-3);
        }

        [Fact]
        public void ShouldBeAbleToSetIncrement()
        {
            generator.Increment = 2;
            generator.Generate().ShouldBe(0);
            generator.Generate().ShouldBe(2);
            generator.Generate().ShouldBe(4);
            generator.Generate().ShouldBe(6);
        }
    }
}