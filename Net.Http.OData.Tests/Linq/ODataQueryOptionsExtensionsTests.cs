﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Moq;
using Net.Http.OData.Linq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Sample.Model;
using Xunit;

namespace Net.Http.OData.Tests.Linq
{
    public class ODataQueryOptionsExtensionsTests
    {
        private readonly IList<Category> _categories;
        private readonly IList<Product> _products;

        public ODataQueryOptionsExtensionsTests()
        {
            _categories = new[]
            {
                new Category { Name = "Mobile Phones", Description = "The latest mobile phones"},
                new Category { Name = "Accessories", Description = "Mobile phone accessories"},
            };

            _products = new[]
            {
                new Product { Category = _categories[0], Colour = Colour.Blue, Description = "iPhone SE 64GB Blue", Name = "iPhone SE 64GB Blue", Price = 419.00M, ProductId = 1, Rating = 4.67f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Red, Description = "iPhone SE 64GB Red", Name = "iPhone SE 64GB Red", Price = 419.00M, ProductId = 2, Rating = 4.68f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Green, Description = "iPhone SE 64GB Green", Name = "iPhone SE 64GB Green", Price = 419.00M, ProductId = 3, Rating = 4.25f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Blue, Description = "iPhone SE 128GB Blue", Name = "iPhone SE 128GB Blue", Price = 469.00M, ProductId = 4, Rating = 4.76f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Red, Description = "iPhone SE 128GB Red", Name = "iPhone SE 128GB Red", Price = 469.00M, ProductId = 5, Rating = 4.72f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Green, Description = "iPhone SE 128GB Green", Name = "iPhone SE 128GB Green", Price = 469.00M, ProductId = 6, Rating = 4.36f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Blue, Description = "iPhone SE 256GB Blue", Name = "iPhone SE 256GB Blue", Price = 569.00M, ProductId = 7, Rating = 4.43f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Red, Description = "iPhone SE 256GB Red", Name = "iPhone SE 256GB Red", Price = 569.00M, ProductId = 8, Rating = 4.41f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[0], Colour = Colour.Green, Description = "iPhone SE 256GB Green", Name = "iPhone SE 256GB Green", Price = 569.00M, ProductId = 9, Rating = 4.26f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[1], Colour = Colour.Blue, Description = "iPhone SE Silicone Case - Blue", Name = "iPhone SE Silicone Case - Blue", Price = 39.00M, ProductId = 10, Rating = 3.76f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[1], Colour = Colour.Red, Description = "iPhone SE Silicone Case - Red", Name = "iPhone SE Silicone Case - Red", Price = 39.00M, ProductId = 11, Rating = 3.24f, ReleaseDate = new DateTime(2020, 4, 24) },
                new Product { Category = _categories[1], Colour = Colour.Green, Description = "iPhone SE Silicone Case - Green", Name = "iPhone SE Silicone Case - Green", Price = 39.00M, ProductId = 12, Rating = 3.87f, ReleaseDate = new DateTime(2020, 4, 24) },
            };
        }

        [Fact]
        public void ApplyTo_OrderBy_NotDeclared()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            Assert.Equal(_products.Min(x => x.ProductId), ((dynamic)results[0]).ProductId);
            Assert.Equal(_products.Max(x => x.ProductId), ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void ApplyTo_OrderBy_Properties()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Price,Rating desc",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            IOrderedEnumerable<Product> orderedProducts = _products.OrderBy(x => x.Price).ThenByDescending(x => x.Rating);

            Assert.Equal(orderedProducts.First().ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(orderedProducts.Last().ProductId, ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void ApplyTo_OrderBy_Properties_IncludingPropertyPath()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Category/Name,Name,Price desc",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            IOrderedEnumerable<Product> orderedProducts = _products.OrderBy(x => x.Category.Name).ThenBy(x => x.Name).ThenByDescending(x => x.Price);

            Assert.Equal(orderedProducts.First().ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(orderedProducts.Last().ProductId, ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void ApplyTo_OrderBy_SingleProperty_Ascending()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Rating",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);
            Assert.Equal(_products.Min(x => x.Rating), ((dynamic)results[0]).Rating);
            Assert.Equal(_products.Max(x => x.Rating), ((dynamic)results[results.Count - 1]).Rating);
        }

        [Fact]
        public void ApplyTo_OrderBy_SingleProperty_Descending()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Rating desc",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);
            Assert.Equal(_products.Max(x => x.Rating), ((dynamic)results[0]).Rating);
            Assert.Equal(_products.Min(x => x.Rating), ((dynamic)results[results.Count - 1]).Rating);
        }

        [Fact]
        public void ApplyTo_Select_NotDeclared()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            for (int i = 0; i < _products.Count; i++)
            {
                Assert.Equal(8, ((IDictionary<string, object>)results[i]).Count);
                Assert.Equal(_products[i].Colour, ((dynamic)results[i]).Colour);
                Assert.Equal(_products[i].Description, ((dynamic)results[i]).Description);
                Assert.Equal(_products[i].Discontinued, ((dynamic)results[i]).Discontinued);
                Assert.Equal(_products[i].Name, ((dynamic)results[i]).Name);
                Assert.Equal(_products[i].Price, ((dynamic)results[i]).Price);
                Assert.Equal(_products[i].ProductId, ((dynamic)results[i]).ProductId);
                Assert.Equal(_products[i].Rating, ((dynamic)results[i]).Rating);
                Assert.Equal(_products[i].ReleaseDate, ((dynamic)results[i]).ReleaseDate);
            }
        }

        [Fact]
        public void ApplyTo_Select_Properties()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=Name,Price,Rating",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            for (int i = 0; i < _products.Count; i++)
            {
                Assert.Equal(3, ((IDictionary<string, object>)results[i]).Count);
                Assert.Equal(_products[i].Name, ((dynamic)results[i]).Name);
                Assert.Equal(_products[i].Price, ((dynamic)results[i]).Price);
                Assert.Equal(_products[i].Rating, ((dynamic)results[i]).Rating);
            }
        }

        [Fact]
        public void ApplyTo_Select_Properties_IncludingPropertyPath()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=Name,Price,Rating,Category/Name",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            for (int i = 0; i < _products.Count; i++)
            {
                Assert.Equal(4, ((IDictionary<string, object>)results[i]).Count);
                Assert.Equal(_products[i].Name, ((dynamic)results[i]).Name);
                Assert.Equal(_products[i].Price, ((dynamic)results[i]).Price);
                Assert.Equal(_products[i].Rating, ((dynamic)results[i]).Rating);

                dynamic category = ((dynamic)results[i]).Category;
                Assert.Equal(1, ((IDictionary<string, object>)category).Count);
                Assert.Equal(_products[i].Category.Name, ((dynamic)category).Name);
            }
        }

        [Fact]
        public void ApplyTo_Select_SingleProperty()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=Name",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            for (int i = 0; i < _products.Count; i++)
            {
                Assert.Equal(1, ((IDictionary<string, object>)results[i]).Count);
                Assert.Equal(_products[i].Name, ((dynamic)results[i]).Name);
            }
        }

        [Fact]
        public void ApplyTo_Select_Star()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=*",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(_products.Count, results.Count);

            for (int i = 0; i < _products.Count; i++)
            {
                Assert.Equal(8, ((IDictionary<string, object>)results[i]).Count);
                Assert.Equal(_products[i].Colour, ((dynamic)results[i]).Colour);
                Assert.Equal(_products[i].Description, ((dynamic)results[i]).Description);
                Assert.Equal(_products[i].Discontinued, ((dynamic)results[i]).Discontinued);
                Assert.Equal(_products[i].Name, ((dynamic)results[i]).Name);
                Assert.Equal(_products[i].Price, ((dynamic)results[i]).Price);
                Assert.Equal(_products[i].ProductId, ((dynamic)results[i]).ProductId);
                Assert.Equal(_products[i].Rating, ((dynamic)results[i]).Rating);
                Assert.Equal(_products[i].ReleaseDate, ((dynamic)results[i]).ReleaseDate);
            }
        }

        [Fact]
        public void ApplyTo_Skip()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$skip=4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            IEnumerable<Product> skippedProducts = _products.Skip(4);

            Assert.Equal(_products.Count - 4, results.Count);
            Assert.Equal(skippedProducts.First().ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(skippedProducts.Last().ProductId, ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void ApplyTo_Throws_ArgumentNullException_For_Null_Queryable()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$count=true",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            Assert.Throws<ArgumentNullException>(() => ODataQueryOptionsExtensions.ApplyTo(queryOptions, null));
        }

        [Fact]
        public void ApplyTo_Throws_ArgumentNullException_For_Null_QueryOptions()
            => Assert.Throws<ArgumentNullException>(() => ODataQueryOptionsExtensions.ApplyTo(null, _categories.AsQueryable()));

        [Fact]
        public void ApplyTo_Throws_InvalidOperationException_For_Incorrect_QueryType()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$count=true",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            Assert.Throws<InvalidOperationException>(() => queryOptions.ApplyTo(_categories.AsQueryable()));
        }

        [Fact]
        public void ApplyTo_Top()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$top=4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = queryOptions.ApplyTo(_products.AsQueryable()).ToList();

            Assert.Equal(4, results.Count);
            Assert.Equal(_products[0].ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(_products[3].ProductId, ((dynamic)results[3]).ProductId);
        }
    }
}
