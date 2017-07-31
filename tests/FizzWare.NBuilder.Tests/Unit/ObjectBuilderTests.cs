﻿using System;
using System.Collections.Generic;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class ObjectBuilderTests
    {
        public ObjectBuilderTests()
        {
            var builderSetup = new BuilderSettings();

            reflectionUtil = Substitute.For<IReflectionUtil>();

            builder = new ObjectBuilder<MyClass>(reflectionUtil, builderSetup);
            myClassWithConstructorBuilder = new ObjectBuilder<MyClassWithConstructor>(reflectionUtil, builderSetup);
            myClassWithOptionalConstructorBuilder =
                new ObjectBuilder<MyClassWithOptionalConstructor>(reflectionUtil, builderSetup);
        }

        private readonly IReflectionUtil reflectionUtil;
        private readonly ObjectBuilder<MyClass> builder;
        private readonly ObjectBuilder<MyClassWithConstructor> myClassWithConstructorBuilder;
        private readonly ObjectBuilder<MyClassWithOptionalConstructor> myClassWithOptionalConstructorBuilder;

        [Fact]
        public void Should_be_able_to_construct_an_object_using_WithConstructor()
        {
            const int arg1 = 1;
            const float arg2 = 2f;


            {
                reflectionUtil.RequiresConstructorArgs(typeof(MyClassWithConstructor)).Returns(true);
                reflectionUtil.CreateInstanceOf<MyClassWithConstructor>(arg1, arg2)
                    .Returns(new MyClassWithConstructor(arg1, arg2));
            }


            {
                myClassWithConstructorBuilder
                    .WithConstructor(() => new MyClassWithConstructor(arg1, arg2))
                    .Construct(1);
            }
        }

        [Fact]
        public void ShouldBeAbleToConstructAnObjectWithConstructorArgs()
        {
            const int arg1 = 1;
            const float arg2 = 2f;


            {
                reflectionUtil.RequiresConstructorArgs(typeof(MyClassWithConstructor)).Returns(true);
                reflectionUtil.CreateInstanceOf<MyClassWithConstructor>(arg1, arg2)
                    .Returns(new MyClassWithConstructor(arg1, arg2));
            }


            {
#pragma warning disable 0618
                myClassWithConstructorBuilder
                    .WithConstructor(() => new MyClassWithConstructor(arg1, arg2))
                    .Construct(1);
#pragma warning restore 0618
            }
        }

        [Fact]
        public void ShouldBeAbleToConstructAnObjectWithNoConstructorArgs()
        {
            reflectionUtil.RequiresConstructorArgs(typeof(MyClass)).Returns(false);
            reflectionUtil.CreateInstanceOf<MyClass>().Returns(new MyClass());

            builder.Construct(1);
        }

        [Fact]
        public void ShouldBeAbleToConstructAnObjectWithNullableConstructorArgs_using_expression_syntax()
        {
            const string arg1 = null;


            {
                reflectionUtil.RequiresConstructorArgs(typeof(MyClassWithConstructor)).Returns(true);
                reflectionUtil.CreateInstanceOf<MyClassWithConstructor>(arg1).Returns(new MyClassWithConstructor(arg1));
            }


            {
                myClassWithConstructorBuilder
                    .WithConstructor(() => new MyClassWithConstructor(arg1))
                    .Construct(Arg.Any<int>());
            }
        }

        [Fact]
        public void WithConstructor_NotANewExpressionSupplied_Throws()
        {
            {
                var myClass = new MyClassWithConstructor(1, 2);
                Assert.Throws<ArgumentException>(() => myClassWithConstructorBuilder.WithConstructor(() => myClass));
            }
        }

        [Fact]
        public void ShouldBeAbleToConstructAnObjectWithOptionalConstructorArgs()
        {
            // ctor: public MyClassWithOptionalConstructor(string arg1, int arg2)

            const string arg1 = "test";
            const int arg2 = 2;


            {
                reflectionUtil.CreateInstanceOf<MyClassWithOptionalConstructor>(arg1, arg2)
                    .Returns(new MyClassWithOptionalConstructor(arg1, arg2));
            }


            {
#pragma warning disable 0618 // (prevent warning for using obsolete method)
                myClassWithOptionalConstructorBuilder
                    .WithConstructor(() => new MyClassWithOptionalConstructor(arg1, arg2))
                    .Construct(1);
#pragma warning restore 0618
            }
        }

        [Fact]
        public void ShouldBeAbleToUseWith()
        {
            {
                var myClass = new MyClass();

                builder.With(x => x.Float = 2f);
                builder.CallFunctions(myClass);

                myClass.Float.ShouldBe(2f);
            }
        }

        [Fact]
        public void ShouldBeAbleToUseWith_WithAnIndex()
        {
            {
                var myClass = new MyClass();

                builder.With((x, idx) => x.StringOne = "String" + (idx + 5));
                builder.CallFunctions(myClass, 9);

                myClass.StringOne.ShouldBe("String14");
            }
        }

        [Fact]
        public void ShouldBeAbleToUseDo()
        {
            var myClass = Substitute.For<MyClass>();


            {
                myClass.DoSomething();
            }


            {
                builder.Do(x => x.DoSomething());
                builder.CallFunctions(myClass);
            }
        }

        [Fact]
        public void ShouldBeAbleToUseANamingStrategy()
        {
            var propertyNamer = Substitute.For<IPropertyNamer>();


            {
                propertyNamer.SetValuesOf(Arg.Any<MyClass>());
            }


            {
                builder.WithPropertyNamer(propertyNamer);
                builder.Name(new MyClass());
            }
        }

        [Fact]
        public void ShouldBeAbleToUseBuild()
        {
            var myClass = new MyClass();
            var propertyNamer = Substitute.For<IPropertyNamer>();


            {
                reflectionUtil.CreateInstanceOf<MyClass>().Returns(myClass);
                propertyNamer.SetValuesOf(Arg.Any<MyClass>());
            }


            {
                builder.WithPropertyNamer(propertyNamer);
                builder.With(x => x.Float = 2f);
                builder.Build();
            }
        }

        [Fact]
        public void ShouldBeAbleToUseDoMultiple()
        {
            var builderSetup = new BuilderSettings();
            var myClass = Substitute.For<IMyInterface>();
            var list = new List<IMyOtherInterface>
            {
                Substitute.For<IMyOtherInterface>(),
                Substitute.For<IMyOtherInterface>(),
                Substitute.For<IMyOtherInterface>()
            };

            var builder2 = new ObjectBuilder<IMyInterface>(reflectionUtil, builderSetup);


            {
                myClass.Add(Arg.Any<IMyOtherInterface>());
            }


            {
                builder2.DoMultiple((x, y) => x.Add(y), list);
                builder2.CallFunctions(myClass);
            }
        }
    }
}