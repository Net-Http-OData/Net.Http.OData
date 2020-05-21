using System;
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
    public class QueryableExtensionsTests
    {
        private readonly IList<Category> _categories;
        private readonly IList<Customer> _customers;
        private readonly IList<Employee> _employees;
        private readonly IList<Manager> _managers;
        private readonly IList<Product> _products;

        public QueryableExtensionsTests()
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

            _managers = new[]
            {
                new Manager { AccessLevel = AccessLevel.Read | AccessLevel.Write | AccessLevel.Delete, AnnualBudget = 5000000.00M, BirthDate = new DateTime(1971, 5, 16), EmailAddress = "Bob.Jones@odata.org", Forename="Bob", Id = "Bob.Jones", JoiningDate = new DateTime(2007, 3, 8), Surname = "Jones", Title = "Mr" },
                new Manager { AccessLevel = AccessLevel.Read | AccessLevel.Write | AccessLevel.Delete, AnnualBudget = 8500000.00M, BirthDate = new DateTime(1982, 6, 21), EmailAddress = "Jess.Smith@odata.org", Forename="Jess", Id = "Jess.Smith", JoiningDate = new DateTime(2006, 7, 15), Surname = "Smith", Title = "Mrs" },
            };

            _employees = new List<Employee>(_managers)
            {
                new Employee { AccessLevel = AccessLevel.Read | AccessLevel.Write, BirthDate = new DateTime(1989, 4, 21), EmailAddress = "Alice.Rake@odata.org", Forename="Alice", Id = "Alice.Rake", JoiningDate = new DateTime(2012, 12, 6), Manager = _managers[0], Surname = "Rake", Title = "Miss" },
                new Employee { AccessLevel = AccessLevel.Read | AccessLevel.Write, BirthDate = new DateTime(1992, 7, 2), EmailAddress = "Mark.Strong@odata.org", Forename="Mark", Id = "Mark.Strong", JoiningDate = new DateTime(2012, 7, 23), Manager = _managers[1], Surname = "Strong", Title = "Mr" },
            };

            _customers = new[]
            {
                new Customer { AccountManager = _employees[_managers.Count + 1], Address = "Some Street", City = "Star City", CompanyName = "Target", ContactName = "Geoff Jr", Country = "USA", LegacyId = 8763, Phone = "555-4202", PostalCode = "76542" },
            };
        }

        [Fact]
        public void Apply_Filter_Single_Property_Add_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price add 11.00M eq 50M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price + 11.00M == 50M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Add_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price add 11 eq 50M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price + 11 == 50M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Concat()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=concat(concat(Forename, ', '), Surname) eq 'Jess, Smith'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => (x.Forename + ", " + x.Surname) == "Jess, Smith").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess" && ((dynamic)x).Surname == "Smith"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Contains()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=contains(Description, 'Case')",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.Contains("Case")).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Contains("Case")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Divide_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price div 2M eq 19.5M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price / 2 == 19.5M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Divide_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price div 2 eq 19.5M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price / 2 == 19.5M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_EndsWith()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=endswith(Description, 'Blue')",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.EndsWith("Blue")).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.EndsWith("Blue")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Equals_Enum()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Colour eq Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Colour == Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(Colour.Blue, ((dynamic)x).Colour));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Equals_Int()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=ProductId eq 4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Single(results);

            Assert.Equal(4, ((dynamic)results[0]).ProductId);
        }

        [Fact]
        public void Apply_Filter_Single_Property_Equals_Null()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description eq null",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Empty(results);
        }

        [Fact]
        public void Apply_Filter_Single_Property_Equals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description eq 'iPhone SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Single(results);

            Assert.Equal(_products.Single(x => x.Description == "iPhone SE 64GB Blue").ProductId, ((dynamic)results[0]).ProductId);
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThan_And_Single_Property_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price gt 469.00M and Colour eq Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price > 469.00M && x.Colour == Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price > 469.00M && ((dynamic)x).Colour == Colour.Blue));
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThan_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price gt 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price > 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price > 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThan_Or_Single_Property_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price gt 469.00M or Colour eq Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price > 469.00M || x.Colour == Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price > 469.00M || ((dynamic)x).Colour == Colour.Blue));
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThanOrEqual_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price ge 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price >= 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price >= 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Has_Enum()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=AccessLevel has Sample.Model.AccessLevel'Read,Write'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.AccessLevel.HasFlag(AccessLevel.Read | AccessLevel.Write)).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).AccessLevel.HasFlag(AccessLevel.Read | AccessLevel.Write)));
        }

        [Fact]
        public void Apply_Filter_Single_Property_IndexOf()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=indexof(Description, '64GB') eq 10",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.IndexOf("64GB") == 10).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Contains("64GB")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Length()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=length(Description) eq 19",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.Length == 19).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Length == 19));
        }

        [Fact]
        public void Apply_Filter_Single_Property_LessThan_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price lt 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price < 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price < 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_LessThanOrEqual_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price le 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price <= 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price <= 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Modulo_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Rating mod 2f eq 0.68f",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Rating % 2 == 0.68f).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Modulo_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Rating mod 2 eq 0.68f",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Rating % 2 == 0.68f).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Multiply_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price mul 2M eq 78M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price * 2 == 78M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Multiply_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price mul 2 eq 78M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price * 2 == 78M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Not_Equals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=not Description eq 'iPhone SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description != "iPhone SE 64GB Blue").Count(), results.Count);

            Assert.All(results, x => Assert.NotEqual("iPhone SE 64GB Blue", ((dynamic)x).Description));
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_Enum()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Colour ne Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Colour != Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.NotEqual(Colour.Blue, ((dynamic)x).Colour));
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_Int()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=ProductId ne 4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count - 1, results.Count);
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_Null()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description ne null",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description ne 'iPhone SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description != "iPhone SE 64GB Blue").Count(), results.Count);

            Assert.All(results, x => Assert.NotEqual("iPhone SE 64GB Blue", ((dynamic)x).Description));
        }

        [Fact]
        public void Apply_Filter_Single_Property_StartsWith()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=startswith(Description, 'iPhone SE 64GB')",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.StartsWith("iPhone SE 64GB")).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.StartsWith("iPhone SE 64GB")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Substring()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=substring(Description, 7) eq 'SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.Substring(7) == "SE 64GB Blue").Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description == "iPhone SE 64GB Blue"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Subtract_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price sub 11M eq 28M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price - 11M == 28M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Subtract_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price sub 11 eq 28M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price - 11 == 28M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_ToLower()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=tolower(Forename) eq 'jess'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.Forename.ToLower() == "jess").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_ToUpper()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=toupper(Forename) eq 'JESS'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.Forename.ToUpper() == "JESS").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Trim()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=trim(Forename) eq 'Jess'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.Forename.Trim() == "Jess").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess"));
        }

        [Fact]
        public void Apply_Filter_Single_PropertyPath_Equals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Category/Name eq 'Accessories'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Category.Name == "Accessories").Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Contains("Case")));
        }

        [Fact]
        public void Apply_Filter_Single_PropertyPath_NotEquals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Category/Name ne 'Accessories'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Category.Name != "Accessories").Count(), results.Count);

            Assert.All(results, x => Assert.False(((dynamic)x).Description.Contains("Case")));
        }

        [Fact]
        public void Apply_OrderBy_NotDeclared()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);

            Assert.Equal(_products.Min(x => x.ProductId), ((dynamic)results[0]).ProductId);
            Assert.Equal(_products.Max(x => x.ProductId), ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void Apply_OrderBy_Properties()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Price,Rating desc",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);

            IOrderedEnumerable<Product> orderedProducts = _products.OrderBy(x => x.Price).ThenByDescending(x => x.Rating);

            Assert.Equal(orderedProducts.First().ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(orderedProducts.Last().ProductId, ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void Apply_OrderBy_Properties_IncludingPropertyPath()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Category/Name,Name,Price desc",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);

            IOrderedEnumerable<Product> orderedProducts = _products.OrderBy(x => x.Category.Name).ThenBy(x => x.Name).ThenByDescending(x => x.Price);

            Assert.Equal(orderedProducts.First().ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(orderedProducts.Last().ProductId, ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void Apply_OrderBy_SingleProperty_Ascending()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Rating",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);
            Assert.Equal(_products.Min(x => x.Rating), ((dynamic)results[0]).Rating);
            Assert.Equal(_products.Max(x => x.Rating), ((dynamic)results[results.Count - 1]).Rating);
        }

        [Fact]
        public void Apply_OrderBy_SingleProperty_Descending()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$orderby=Rating desc",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);
            Assert.Equal(_products.Max(x => x.Rating), ((dynamic)results[0]).Rating);
            Assert.Equal(_products.Min(x => x.Rating), ((dynamic)results[results.Count - 1]).Rating);
        }

        [Fact]
        public void Apply_Select_Expand()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=CompanyName,ContactName,AccountManager/Forename,AccountManager/Surname,AccountManager/EmailAddress&$expand=AccountManager/Manager",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _customers.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_customers.Count, results.Count);

            for (int i = 0; i < _customers.Count; i++)
            {
                Assert.Equal(3, ((IDictionary<string, object>)results[i]).Count);
                Assert.Equal(_customers[i].CompanyName, ((dynamic)results[i]).CompanyName);
                Assert.Equal(_customers[i].ContactName, ((dynamic)results[i]).ContactName);

                dynamic accountManager = ((dynamic)results[i]).AccountManager;
                Assert.Equal(4, ((IDictionary<string, object>)accountManager).Count);
                Assert.Equal(_customers[i].AccountManager.Forename, ((dynamic)accountManager).Forename);
                Assert.Equal(_customers[i].AccountManager.Surname, ((dynamic)accountManager).Surname);
                Assert.Equal(_customers[i].AccountManager.EmailAddress, ((dynamic)accountManager).EmailAddress);

                dynamic manager = ((dynamic)accountManager).Manager;
                Assert.Equal(11, ((IDictionary<string, object>)manager).Count);
                Assert.Equal(_customers[i].AccountManager.Manager.AccessLevel, ((dynamic)manager).AccessLevel);
                Assert.Equal(_customers[i].AccountManager.Manager.AnnualBudget, ((dynamic)manager).AnnualBudget);
                Assert.Equal(_customers[i].AccountManager.Manager.BirthDate, ((dynamic)manager).BirthDate);
                Assert.Equal(_customers[i].AccountManager.Manager.EmailAddress, ((dynamic)manager).EmailAddress);
                Assert.Equal(_customers[i].AccountManager.Manager.Forename, ((dynamic)manager).Forename);
                Assert.Equal(_customers[i].AccountManager.Manager.Id, ((dynamic)manager).Id);
                Assert.Equal(_customers[i].AccountManager.Manager.ImageData, ((dynamic)manager).ImageData);
                Assert.Equal(_customers[i].AccountManager.Manager.JoiningDate, ((dynamic)manager).JoiningDate);
                Assert.Equal(_customers[i].AccountManager.Manager.LeavingDate, ((dynamic)manager).LeavingDate);
                Assert.Equal(_customers[i].AccountManager.Manager.Surname, ((dynamic)manager).Surname);
                Assert.Equal(_customers[i].AccountManager.Manager.Title, ((dynamic)manager).Title);
            }
        }

        [Fact]
        public void Apply_Select_NotDeclared()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

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
        public void Apply_Select_Properties()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=Name,Price,Rating",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

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
        public void Apply_Select_Properties_IncludingPropertyPath()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=Name,Price,Rating,Category/Name",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

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
        public void Apply_Select_SingleProperty()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=Name",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);

            for (int i = 0; i < _products.Count; i++)
            {
                Assert.Equal(1, ((IDictionary<string, object>)results[i]).Count);
                Assert.Equal(_products[i].Name, ((dynamic)results[i]).Name);
            }
        }

        [Fact]
        public void Apply_Select_Star()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$select=*",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

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
        public void Apply_Skip()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$skip=4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            IEnumerable<Product> skippedProducts = _products.Skip(4);

            Assert.Equal(_products.Count - 4, results.Count);
            Assert.Equal(skippedProducts.First().ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(skippedProducts.Last().ProductId, ((dynamic)results[results.Count - 1]).ProductId);
        }

        [Fact]
        public void Apply_Throws_ArgumentNullException_For_Null_Queryable()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$count=true",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            Assert.Throws<ArgumentNullException>(() => QueryableExtensions.Apply(null, queryOptions));
        }

        [Fact]
        public void Apply_Throws_ArgumentNullException_For_Null_QueryOptions()
            => Assert.Throws<ArgumentNullException>(() => QueryableExtensions.Apply(_categories.AsQueryable(), null));

        [Fact]
        public void Apply_Throws_InvalidOperationException_For_Incorrect_QueryType()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$count=true",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            Assert.Throws<InvalidOperationException>(() => _categories.AsQueryable().Apply(queryOptions));
        }

        [Fact]
        public void Apply_Top()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$top=4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(4, results.Count);
            Assert.Equal(_products[0].ProductId, ((dynamic)results[0]).ProductId);
            Assert.Equal(_products[3].ProductId, ((dynamic)results[3]).ProductId);
        }
    }
}
