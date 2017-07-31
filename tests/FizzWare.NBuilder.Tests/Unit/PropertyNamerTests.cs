using System;
using System.Collections.Generic;
using System.Reflection;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class PropertyNamerTests
    {
        public PropertyNamerTests()
        {
            var builderSettings = new BuilderSettings();
            var reflectionUtil = Substitute.For<IReflectionUtil>();
            reflectionUtil.IsDefaultValue(Arg.Any<int?>()).Returns(true);

            propertyNamer = new PropertyNamerStub(reflectionUtil, builderSettings);
        }

        private readonly PropertyNamerStub propertyNamer;

        private class PropertyNamerStub : PropertyNamer
        {
            public PropertyNamerStub(IReflectionUtil reflectionUtil, BuilderSettings builderSettings) : base(
                reflectionUtil, builderSettings)
            {
            }

            public override void SetValuesOfAllIn<T>(IList<T> objects)
            {
            }

            protected override short GetInt16(MemberInfo memberInfo)
            {
                return default(short);
            }

            protected override int GetInt32(MemberInfo memberInfo)
            {
                return default(int);
            }

            protected override long GetInt64(MemberInfo memberInfo)
            {
                return default(long);
            }

            protected override decimal GetDecimal(MemberInfo memberInfo)
            {
                return default(decimal);
            }

            protected override float GetSingle(MemberInfo memberInfo)
            {
                return default(float);
            }

            protected override double GetDouble(MemberInfo memberInfo)
            {
                return default(double);
            }

            protected override ushort GetUInt16(MemberInfo memberInfo)
            {
                return default(ushort);
            }

            protected override uint GetUInt32(MemberInfo memberInfo)
            {
                return default(uint);
            }

            protected override ulong GetUInt64(MemberInfo memberInfo)
            {
                return default(ulong);
            }

            protected override sbyte GetSByte(MemberInfo memberInfo)
            {
                return default(sbyte);
            }

            protected override byte GetByte(MemberInfo memberInfo)
            {
                return default(byte);
            }

            protected override DateTime GetDateTime(MemberInfo memberInfo)
            {
                return default(DateTime);
            }

            protected override string GetString(MemberInfo memberInfo)
            {
                return default(string);
            }

            protected override bool GetBoolean(MemberInfo memberInfo)
            {
                return default(bool);
            }

            protected override char GetChar(MemberInfo memberInfo)
            {
                return default(char);
            }

            protected override Enum GetEnum(MemberInfo memberInfo)
            {
                return default(Enum);
            }

            protected override Guid GetGuid(MemberInfo memberInfo)
            {
                return default(Guid);
            }

            protected override TimeSpan GetTimeSpan(MemberInfo memberInfo)
            {
                return default(TimeSpan);
            }
        }

        [Fact]
        public void SetValuesOf_ClassWithNullCharConst_CharConstantIsNotSetByNamer()
        {
            var mc = new MyClassWithCharConst();

            propertyNamer.SetValuesOf(mc);

            mc.GetNullCharConst().ShouldBe(MyClassWithCharConst.NullCharConst);
            mc.GetNonNullCharConst().ShouldBe(MyClassWithCharConst.NonNullCharConst);

            // TODO: Not sure how to port this.
            //Assert.Pass("A System.FieldAccessException was not thrown because NBuilder didn't try to set the value of the constant");
        }

        [Fact]
        public void SetValuesOf_GetOnlyProperty_PropertyIsNotSet()
        {
            var myClass = new MyClassWithGetOnlyPropertySpy();
            propertyNamer.SetValuesOf(myClass);

            myClass.IsSet.ShouldBeFalse();
        }

        [Fact]
        public void SetValuesOf_GivenObjectWithNullableProperty_SetsTheValueOfTheProperty()
        {
            var mc = new MyClass {NullableInt = null};

            propertyNamer.SetValuesOf(mc);

            mc.NullableInt.HasValue.ShouldBeTrue();
        }
    }
}