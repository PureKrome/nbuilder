using System;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.Tests.TestClasses;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Unit
{
    public class DeclarationQueueTests
    {
        public DeclarationQueueTests()
        {
            declarations = new DeclarationQueue<MyClass>(listSize);

            declaration1 = Substitute.For<IDeclaration<MyClass>>();
            declaration2 = Substitute.For<IDeclaration<MyClass>>();
            globalDeclaration = Substitute.For<IGlobalDeclaration<MyClass>>();

            declaration1.Start.Returns(0);
            declaration1.End.Returns(10);
        }

        private readonly DeclarationQueue<MyClass> declarations;
        private readonly IDeclaration<MyClass> declaration1;
        private readonly IDeclaration<MyClass> declaration2;
        private readonly IGlobalDeclaration<MyClass> globalDeclaration;

        private const int listSize = 30;

        [Fact]
        public void CountShouldDecreaseWhenDequeuingAnItem()
        {
            declarations.Enqueue(declaration1);
            declarations.Dequeue();
            declarations.Count.ShouldBe(0);
        }

        [Fact]
        public void CountShouldIncreaseWhenAddingAnItem()
        {
            declarations.Enqueue(declaration1);
            declarations.Count.ShouldBe(1);
        }

        [Fact]
        public void FirstItemsInShouldComeOutFirst()
        {
            declarations.Enqueue(declaration1);
            declarations.Enqueue(globalDeclaration);

            declarations.Dequeue().ShouldBe(declaration1);
            declarations.Dequeue().ShouldBe(globalDeclaration);
        }

        [Fact]
        public void GlobalDeclarationsShouldBePrioritised()
        {
            declarations.Enqueue(declaration1);
            declarations.Enqueue(globalDeclaration);

            declarations.Prioritise();

            declarations.Dequeue().ShouldBe(globalDeclaration);
            declarations.Dequeue().ShouldBe(declaration1);
            declarations.Count.ShouldBe(0);
        }

        [Fact]
        public void ShouldBeAbleToDequeueAnItem()
        {
            declarations.Enqueue(declaration1);
            declarations.Dequeue().ShouldBe(declaration1);
        }

        [Fact]
        public void ShouldBeAbleToPeek()
        {
            declarations.Enqueue(declaration1);
            declarations.GetLastItem().ShouldBe(declaration1);
            declarations.Count.ShouldBe(1);
        }

        [Fact]
        public void ShouldBeAbleToPrioritiseMultipleGlobalDeclarations()
        {
            declarations.Enqueue(declaration1);
            declarations.Enqueue(globalDeclaration);
            declarations.Enqueue(globalDeclaration);

            declarations.Prioritise();

            declarations.Dequeue().ShouldBe(globalDeclaration);
            declarations.Dequeue().ShouldBe(globalDeclaration);
            declarations.Dequeue().ShouldBe(declaration1);
        }

        [Fact]
        public void ShouldComplainIfEndIsGreaterThanCapacity()
        {
            declaration1.Start.Returns(0);
            declaration1.End.Returns(9);

            declaration2.Start.Returns(10);
            declaration2.End.Returns(40);

            declarations.Enqueue(declaration1);

            Assert.Throws<BuilderException>(() => { declarations.Enqueue(declaration2); });
        }

        [Fact]
        public void ShouldComplainIfStartIsLessThanZero()
        {
            //declaration1.BackToRecord(BackToRecordOptions.Expectations);

            declaration1.Start.Returns(-2);
            declaration1.End.Returns(9);

            Assert.Throws<BuilderException>(() => { declarations.Enqueue(declaration1); });
        }

        [Fact]
        public void ShouldThrowIfTryToDequeueWhenQueueEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => { declarations.Dequeue(); });
        }
    }
}