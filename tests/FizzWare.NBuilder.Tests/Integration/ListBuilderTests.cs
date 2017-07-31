﻿using System;
using System.ComponentModel;
using System.Linq;
using FizzWare.NBuilder.Implementation;
using FizzWare.NBuilder.PropertyNaming;
using FizzWare.NBuilder.Tests.Integration.Models;
using Shouldly;
using Xunit;

namespace FizzWare.NBuilder.Tests.Integration
{
    /// <remarks>
    ///     To run these tests, create a local database named 'NBuilderTests'
    /// </remarks>
    public class ListBuilderTests
    {
        public ListBuilderTests()
        {
            // Need to call this explicitly here to overcome a bug in resharper's test runner
            new RepositoryBuilderSetup().DoSetup();
        }

        public void UsingAllToSetValues()
        {
            var builderSetup = new BuilderSettings();
            var products = new Builder(builderSetup)
                .CreateListOfSize<Product>(10)
                .All()
                .With(x => x.Title = "A special title")
                .Build();

            for (var i = 0; i < products.Count; i++)
                products[i].Title.ShouldBe("A special title");
        }

        [Fact]
        public void AutomaticPropertyAndPublicFieldNamingCanBeSwitchedOff()
        {
            var builderSetup = new BuilderSettings();
            builderSetup.AutoNameProperties = false;

            var products = new Builder(builderSetup).CreateListOfSize<Product>(10).Build();

            products[0].Title.ShouldBeNull();
            products[9].Title.ShouldBeNull();

            products[0].Id.ShouldBe(0);
            products[9].Id.ShouldBe(0);
        }


        [Fact]
        public void ChainingDeclarationsTogether()
        {
            var builderSetup = new BuilderSettings();
            var list = new Builder(builderSetup)
                .CreateListOfSize<Product>(30)
                .TheFirst(10)
                .With(x => x.Title = "Special Title 1")
                .TheNext(10)
                .With(x => x.Title = "Special Title 2")
                .TheNext(10)
                .With(x => x.Title = "Special Title 3")
                .Build();

            list[0].Title.ShouldBe("Special Title 1");
            list[9].Title.ShouldBe("Special Title 1");
            list[10].Title.ShouldBe("Special Title 2");
            list[19].Title.ShouldBe("Special Title 2");
            list[20].Title.ShouldBe("Special Title 3");
            list[29].Title.ShouldBe("Special Title 3");
        }


        [Fact]
        public void CreatingAList()
        {
            var builderSetup = new BuilderSettings();
            var products = new Builder(builderSetup).CreateListOfSize<Product>(10).Build();

            // Note: The asserts here are intentionally verbose to show how NBuilder works

            // It sets strings to their name plus their 1-based sequence number
            products[0].Title.ShouldBe("Title1");
            products[1].Title.ShouldBe("Title2");
            products[2].Title.ShouldBe("Title3");
            products[3].Title.ShouldBe("Title4");
            products[4].Title.ShouldBe("Title5");
            products[5].Title.ShouldBe("Title6");
            products[6].Title.ShouldBe("Title7");
            products[7].Title.ShouldBe("Title8");
            products[8].Title.ShouldBe("Title9");
            products[9].Title.ShouldBe("Title10");

            // Ints are set to their 1-based sequence number
            products[0].Id.ShouldBe(1);
            // ... 2, 3, 4, 5, 6, 7, 8 ...
            products[9].Id.ShouldBe(10);

            // Any other numeric types are set to their 1-based sequence number
            products[0].PriceBeforeTax.ShouldBe(1m);
            // ... 2m, 3m, 4m, 5m, 6m, 7m, 8m ...
            products[9].PriceBeforeTax.ShouldBe(10m);
        }

        [Fact]
        public void CreatingAListOfATypeWithAConstructor()
        {
            var builderSetup = new BuilderSettings();
            // ctor: BasketItem(ShoppingBasket basket, Product product, int quantity)

            var builder = new Builder(builderSetup);
            var basket = builder.CreateNew<ShoppingBasket>().Build();
            var product = builder.CreateNew<Product>().Build();
            const int quantity = 5;

            var basketItems =
                builder.CreateListOfSize<BasketItem>(10)
                    .All()
                    .WithConstructor(() => new BasketItem(basket, product,
                        quantity)) // passes these arguments to the constructor
                    .Build();

            foreach (var basketItem in basketItems)
            {
                basketItem.Basket.ShouldBe(basket);
                basketItem.Product.ShouldBe(product);
                basketItem.Quantity.ShouldBe(quantity);
            }
        }

        [Fact]
        public void CreatingAListOfProductsAndAddingThemToCategories()
        {
            var builderSetup = new BuilderSettings();
            var builder = new Builder(builderSetup);
            var categories = builder.CreateListOfSize<Category>(50).Build();

            var products = builder
                .CreateListOfSize<Product>(500)
                .All()
                .With(x => x.Categories = Pick<Category>.UniqueRandomList(With.Between(5, 10).Elements).From(categories)
                    .ToList())
                .Build();

            foreach (var product in products)
            {
                product.Categories.Count.ShouldBeGreaterThanOrEqualTo(5);
                product.Categories.Count.ShouldBeLessThanOrEqualTo(10);
            }
        }

        [Fact]
        public void DeclaringThatARandomNumberOfElementsShouldHaveCertainProperties()
        {
            var builderSetup = new BuilderSettings();
            const string specialdescription = "SpecialDescription";
            const decimal specialPrice = 10m;
            var products = new Builder(builderSetup).CreateListOfSize<Product>(10)
                .Random(5)
                .With(x => x.Description = specialdescription)
                .And(x => x.PriceBeforeTax = specialPrice)
                .Build();

            var count = 0;

            foreach (var product in products)
                if (product.Description == specialdescription &&
                    product.PriceBeforeTax == specialPrice)
                    count++;

            count.ShouldBe(5);
        }

        [Fact]
        public void DifferentPartsOfTheListCanBeConstructedDifferently()
        {
            var builderSetup = new BuilderSettings();
            var basket1 = new ShoppingBasket();
            var product1 = new Product();
            const int quantity1 = 5;

            var basket2 = new ShoppingBasket();
            var product2 = new Product();
            const int quantity2 = 7;

            var items = new Builder(builderSetup)
                .CreateListOfSize<BasketItem>(4)
                .TheFirst(2)
                .WithConstructor(() => new BasketItem(basket1, product1, quantity1))
                .TheNext(2)
                .WithConstructor(() => new BasketItem(basket2, product2, quantity2))
                .Build();

            items[0].Basket.ShouldBe(basket1);
            items[0].Basket.ShouldBe(basket1);
            items[0].Basket.ShouldBe(basket1);
            items[0].Basket.ShouldBe(basket1);
            items[0].Basket.ShouldBe(basket1);
            items[0].Basket.ShouldBe(basket1);
        }

        [Fact]
        public void ItIsPossibleToSwitchOffAutomaticPropertyNamingForASinglePropertyOfASpecificType()
        {
            var builderSetup = new BuilderSettings();
            builderSetup.DisablePropertyNamingFor<Product, int>(x => x.Id);

            var products = new Builder(builderSetup).CreateListOfSize<Product>(10).Build();

            // The Product.Id will always be its default value
            products[0].Id.ShouldBe(0);
            products[9].Id.ShouldBe(0);

            // Other properties are still given automatic values as normal
            products[0].QuantityInStock.ShouldBe(1);
            products[9].QuantityInStock.ShouldBe(10);
        }

        [Fact]
        public void SequentialGenerator_DateTimeGeneration()
        {
            var builderSetup = new BuilderSettings();
            const int increment = 2;
            var dateTimeGenerator = new SequentialGenerator<DateTime>
            {
                IncrementDateBy = IncrementDate.Day,
                IncrementDateValueBy = increment
            };

            var startingDate = DateTime.MinValue;

            dateTimeGenerator.StartingWith(startingDate);


            var list = new Builder(builderSetup).CreateListOfSize<Product>(2)
                .All()
                .With(x => x.Created = dateTimeGenerator.Generate())
                .Build();

            list[0].Created.ShouldBe(startingDate);
            list[1].Created.ShouldBe(startingDate.AddDays(increment));
        }

        [Fact]
        public void SettingTheValueOfAProperty()
        {
            var builderSetup = new BuilderSettings();
            var products = new Builder(builderSetup)
                .CreateListOfSize<Product>(10)
                .TheFirst(2)
                .With(x => x.Title = "A special title")
                .Build();

            products[0].Title.ShouldBe("A special title");
            products[1].Title.ShouldBe("A special title");

            // After that the naming would carry on
            products[2].Title.ShouldBe("Title3");
            products[3].Title.ShouldBe("Title4");
            // ...4, 5, 6, 7, 8...
            products[9].Title.ShouldBe("Title10");
        }

        [Fact]
        public void SpecifyingADifferentDefaultPropertyNamer()
        {
            var builderSetup = new BuilderSettings();
            builderSetup.SetDefaultPropertyNamer(new RandomValuePropertyNamer(new RandomGenerator(),
                new ReflectionUtil(), true, DateTime.Now, DateTime.Now.AddDays(10), true, new BuilderSettings()));

            var products = new Builder(builderSetup).CreateListOfSize<Product>(10).Build();

            products[0].Title.ShouldNotBe("StringOne1");
            products[9].Title.ShouldNotBe("StringOne10");
        }

        [Fact]
        public void StructsCanHavePropertyAssignments()
        {
            var builderSetup = new BuilderSettings();
            var locations = new Builder(builderSetup)
                .CreateListOfSize<WarehouseLocation>(10)
                .Section(5, 6)
                .WithConstructor(() => new WarehouseLocation('A', 1, 2))
                .Build();

            locations[5].Aisle.ShouldBe('A');
            locations[6].Aisle.ShouldBe('A');

            locations[5].Shelf.ShouldBe(1);
            locations[6].Shelf.ShouldBe(1);

            locations[5].Location.ShouldBe(2);
            locations[6].Location.ShouldBe(2);
        }

        [Fact]
        public void SupportsStructsButDoesNotSupportAutomaticallyNamingTheProperties()
        {
            var builderSetup = new BuilderSettings();
            var locations = new Builder(builderSetup)
                .CreateListOfSize<WarehouseLocation>(10)
                .Build();

            locations.Count.ShouldBe(10);
            locations[0].Location.ShouldBe(0);
            locations[1].Location.ShouldBe(0);
        }

        [Fact]
        public void UsingAGenerator()
        {
            var builderSetup = new BuilderSettings();
            var generator = new RandomGenerator();

            var products = new Builder(builderSetup)
                .CreateListOfSize<Product>(10)
                .All()
                .With(x => x.PriceBeforeTax = generator.Next(50, 1000))
                .Build();


            foreach (var product in products)
            {
                product.PriceBeforeTax.ShouldBeGreaterThanOrEqualTo(50m);
                product.PriceBeforeTax.ShouldBeLessThanOrEqualTo(1000m);
            }
        }

        ////[Fact]
        ////public void UsingAndTheRemaining()
        ////{ 
        ////    var list = Builder<Product>
        ////        .CreateListOfSize(4)
        ////        .TheFirst(2)
        ////            .With(x => x.Title = "Special Title 1")
        ////        .TheRemainder()
        ////            .With(x => x.Title = "Special Title 2")
        ////        .Build();

        ////    list[0].Title.ShouldBe("Special Title 1");
        ////    list[1].Title.ShouldBe("Special Title 1");
        ////    list[2].Title.ShouldBe("Special Title 2");
        ////    list[3].Title.ShouldBe("Special Title 2");
        ////}

        [Fact]
        public void UsingAndThePrevious()
        {
            var builderSetup = new BuilderSettings();
            var list = new Builder(builderSetup)
                .CreateListOfSize<Product>(30)
                .TheLast(10)
                .With(x => x.Title = "Special Title 1")
                .ThePrevious(10)
                .With(x => x.Title = "Special Title 2")
                .Build();

            list[10].Title.ShouldBe("Special Title 2");
            list[19].Title.ShouldBe("Special Title 2");
            list[20].Title.ShouldBe("Special Title 1");
            list[29].Title.ShouldBe("Special Title 1");
        }

        [Fact]
        public void UsingAndWithAnIndex()
        {
            var builderSetup = new BuilderSettings();
            var invoices = new Builder(builderSetup)
                .CreateListOfSize<Product>(2)
                .TheFirst(2)
                .With(p => p.Title = "Title")
                .And((p, idx) => p.Description = "Description" + (idx + 5))
                .Build();

            invoices[0].Title.ShouldBe("Title");
            invoices[0].Description.ShouldBe("Description5");
            invoices[1].Title.ShouldBe("Title");
            invoices[1].Description.ShouldBe("Description6");
        }

        [Fact]
        public void UsingAndWithImmutableClassProperties()
        {
            var builderSetup = new BuilderSettings();
            var invoices = new Builder(builderSetup)
                .CreateListOfSize<Invoice>(2)
                .TheFirst(2)
                .With(p => p.Amount, 100)
                .And(p => p.Id, 200)
                .Build();

            invoices[0].Amount.ShouldBe(100);
            invoices[1].Amount.ShouldBe(100);
            invoices[0].Id.ShouldBe(200);
            invoices[1].Id.ShouldBe(200);
        }

        [Fact]
        [Description("You can use Do to do something to all the items in the declaration")]
        public void UsingDo()
        {
            var builder = new Builder();
            var children = builder.CreateListOfSize<Category>(3).Build();

            var categories = builder
                .CreateListOfSize<Category>(10)
                .TheFirst(2)
                .Do(x => x.AddChild(children[0]))
                .And(x => x.AddChild(children[1]))
                .TheNext(2)
                .Do(x => x.AddChild(children[2]))
                .Build();

            categories[0].Children[0].ShouldBe(children[0]);
            categories[0].Children[1].ShouldBe(children[1]);
            categories[1].Children[0].ShouldBe(children[0]);
            categories[1].Children[1].ShouldBe(children[1]);
            categories[2].Children[0].ShouldBe(children[2]);
            categories[3].Children[0].ShouldBe(children[2]);
        }

        [Fact]
        public void UsingDoAndPickTogether()
        {
            var builderSetup = new BuilderSettings();
            var children = new Builder(builderSetup).CreateListOfSize<Category>(10).Build();

            var categories = new Builder(builderSetup)
                .CreateListOfSize<Category>(10)
                .TheFirst(2)
                .Do(x => x.AddChild(Pick<Category>.RandomItemFrom(children)))
                .Build();

            categories[0].Children.Count.ShouldBe(1);
            categories[1].Children.Count.ShouldBe(1);
        }

        [Fact]
        public void UsingHasWithImmutableClassProperties()
        {
            var builderSetup = new BuilderSettings();
            var invoices = new Builder(builderSetup)
                .CreateListOfSize<Invoice>(1)
                .TheFirst(1)
                .With(p => p.Amount, 100)
                .Build();

            invoices[0].Amount.ShouldBe(100);
        }

        [Fact]
        [Description("Multi functions allow you to call a method on an object on each item in a list")]
        public void UsingMultiFunctions()
        {
            var builderSetup = new BuilderSettings();
            var categories = new Builder(builderSetup).CreateListOfSize<Category>(5).Build();

            // Here we are saying, add every product to all of the categories

            var products = new Builder(builderSetup)
                .CreateListOfSize<Product>(10)
                .All().DoForEach((product, category) => product.AddToCategory(category), categories)
                .Build();

            // Assertions are intentionally verbose for clarity
            foreach (var product in products)
            {
                product.Categories.Count.ShouldBe(5);
                product.Categories[0].ShouldBe(categories[0]);
                product.Categories[1].ShouldBe(categories[1]);
                product.Categories[2].ShouldBe(categories[2]);
                product.Categories[3].ShouldBe(categories[3]);
                product.Categories[4].ShouldBe(categories[4]);
            }
        }

        [Fact]
        public void UsingRandomGenerator()
        {
            var builderSetup = new BuilderSettings();
            var generator = new RandomGenerator();

            var list = new Builder(builderSetup).CreateListOfSize<Product>(3)
                .All()
                .With(x => x.QuantityInStock = generator.Next(1000, 2000))
                .Build();

            list[0].QuantityInStock.ShouldBeGreaterThanOrEqualTo(1000);
            list[0].QuantityInStock.ShouldBeLessThanOrEqualTo(2000);

            list[1].QuantityInStock.ShouldBeGreaterThanOrEqualTo(1000);
            list[1].QuantityInStock.ShouldBeLessThanOrEqualTo(2000);

            list[2].QuantityInStock.ShouldBeGreaterThanOrEqualTo(1000);
            list[2].QuantityInStock.ShouldBeLessThanOrEqualTo(2000);
        }

        [Fact]
        public void UsingSection()
        {
            var builderSetup = new BuilderSettings();
            var list = new Builder(builderSetup)
                .CreateListOfSize<Product>(30)
                .All()
                .With(x => x.Title = "Special Title 1")
                .Section(12, 14)
                .With(x => x.Title = "Special Title 2")
                .Section(16, 18)
                .With(x => x.Title = "Special Title 3")
                .Build();

            // All
            list[0].Title.ShouldBe("Special Title 1");
            list[1].Title.ShouldBe("Special Title 1");
            // ...
            list[29].Title.ShouldBe("Special Title 1");

            // Section 1 - 12 - 14
            list[12].Title.ShouldBe("Special Title 2");
            list[13].Title.ShouldBe("Special Title 2");
            list[14].Title.ShouldBe("Special Title 2");

            // Section 2 - 16 - 18
            list[16].Title.ShouldBe("Special Title 3");
            list[17].Title.ShouldBe("Special Title 3");
            list[18].Title.ShouldBe("Special Title 3");
        }

        [Fact]
        public void UsingSectionAndTheNext()
        {
            var builderSetup = new BuilderSettings();
            var list = new Builder(builderSetup)
                .CreateListOfSize<Product>(30)
                .All()
                .With(x => x.Title = "Special Title 1")
                .Section(12, 14)
                .With(x => x.Title = "Special Title 2")
                .TheNext(2)
                .With(x => x.Title = "Special Title 3")
                .Build();

            list[0].Title.ShouldBe("Special Title 1");
            list[15].Title.ShouldBe("Special Title 3");
        }


        [Fact]
        public void UsingSequentialGenerators()
        {
            var builderSetup = new BuilderSettings();
            // Arrange
            var decimalGenerator = new SequentialGenerator<decimal>
            {
                Increment = 10,
                Direction = GeneratorDirection.Descending
            };

            decimalGenerator.StartingWith(2000);

            var intGenerator = new SequentialGenerator<int> {Increment = 10000};

            // Act
            var list = new Builder(builderSetup).CreateListOfSize<Product>(3)
                .All()
                .With(x => x.PriceBeforeTax = decimalGenerator.Generate())
                .And(x => x.Id = intGenerator.Generate())
                .Build();

            // Assert
            list[0].PriceBeforeTax.ShouldBe(2000);
            list[1].PriceBeforeTax.ShouldBe(1990);
            list[2].PriceBeforeTax.ShouldBe(1980);

            list[0].Id.ShouldBe(0);
            list[1].Id.ShouldBe(10000);
            list[2].Id.ShouldBe(20000);
        }

        [Fact]
        public void UsingSingularSyntaxInstead()
        {
            var builderSetup = new BuilderSettings();
            var products = new Builder(builderSetup)
                .CreateListOfSize<Product>(10)
                .TheFirst(1)
                .With(x => x.Title = "Special title 1")
                .Build();

            products[0].Title.ShouldBe("Special title 1");
        }

        [Fact]
        public void UsingTheSequentialGenerator()
        {
            var builderSetup = new BuilderSettings();
            var generator = new SequentialGenerator<int> {Direction = GeneratorDirection.Descending, Increment = 2};
            generator.StartingWith(6);

            var products = new Builder(builderSetup)
                .CreateListOfSize<Product>(3)
                .All()
                .With(x => x.Id = generator.Generate())
                .Build();

            products[0].Id.ShouldBe(6);
            products[1].Id.ShouldBe(4);
            products[2].Id.ShouldBe(2);
        }

        [Fact]
        public void UsingTheWithBetween_And_SyntaxForGreaterReadability()
        {
            var builderSetup = new BuilderSettings();
            var builder = new Builder(builderSetup);
            var categories = builder.CreateListOfSize<Category>(50).Build();

            var products = builder
                .CreateListOfSize<Product>(500)
                .All()
                .With(x => x.Categories = Pick<Category>.UniqueRandomList(With.Between(5).And(10).Elements)
                    .From(categories).ToList())
                .Build();

            foreach (var product in products)
            {
                product.Categories.Count.ShouldBeGreaterThanOrEqualTo(5);
                product.Categories.Count.ShouldBeLessThanOrEqualTo(10);
            }
        }

        [Fact]
        public void UsingWith_WithAnIndex()
        {
            var builderSetup = new BuilderSettings();
            var invoices = new Builder(builderSetup)
                .CreateListOfSize<Product>(2)
                .TheFirst(2)
                .With((p, idx) => p.Description = "Description" + (idx + 5))
                .Build();

            invoices[0].Description.ShouldBe("Description5");
            invoices[1].Description.ShouldBe("Description6");
        }

        [Fact]
        public void UsingWith_WithImmutableClassProperties()
        {
            var builderSetup = new BuilderSettings();
            var invoices = new Builder(builderSetup)
                .CreateListOfSize<Invoice>(2)
                .TheFirst(2)
                .With(p => p.Amount, 100)
                .Build();

            invoices[0].Amount.ShouldBe(100);
            invoices[1].Amount.ShouldBe(100);
        }

        [Fact]
        public void WillComplainIfYouTryToBuildAClassThatCannotBeInstantiatedDirectly()
        {
            Assert.Throws<BuilderException>(() =>
            {
                var builderSetup = new BuilderSettings();
                new Builder(builderSetup).CreateListOfSize<ChuckNorris>(10).Build();
            });
        }

        [Fact]
        public void WillNotLetYouDoThingsThatDoNotMakeSense()
        {
            var builderSetup = new BuilderSettings();

            Assert.Throws<BuilderException>(() =>
            {
                new Builder(builderSetup)
                    .CreateListOfSize<Product>(10)
                    .TheFirst(5)
                    .With(x => x.Title = "titleone")
                    .TheNext(10)
                    .With(x => x.Title = "titletwo")
                    .Build();
            });
        }
    }
}