using System;
using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class ODataQueryOptionsTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_EntitySet()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(() => new ODataQueryOptions("", null, Mock.Of<IODataQueryOptionsValidator>()));
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_Query()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(
                () => new ODataQueryOptions(null, EntityDataModel.Current.EntitySets["Products"], Mock.Of<IODataQueryOptionsValidator>()));
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_Validator()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(() => new ODataQueryOptions("", EntityDataModel.Current.EntitySets["Products"], null));
        }

        /// <summary>
        /// https://github.com/TrevorPilley/Net.Http.WebApi.OData/issues/85 - Not parsing ampersand in query
        /// </summary>
        [Fact]
        public void Parse_WithAmpersandInQuery()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=LegacyId+eq+2139+and+CompanyName+eq+'Pool+Farm+%26+Primrose+Hill+Nursery'&$top=1",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            Assert.NotNull(queryOptions);
            Assert.NotNull(queryOptions.Filter);

            Assert.NotNull(queryOptions.Filter.Expression);
            Assert.IsType<BinaryOperatorNode>(queryOptions.Filter.Expression);

            var node = (BinaryOperatorNode)queryOptions.Filter.Expression;

            Assert.IsType<BinaryOperatorNode>(node.Left);
            var nodeLeft = (BinaryOperatorNode)node.Left;
            Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
            Assert.Equal("LegacyId", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
            Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
            Assert.IsType<ConstantNode>(nodeLeft.Right);
            Assert.Equal(2139, ((ConstantNode)nodeLeft.Right).Value);

            Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

            Assert.IsType<BinaryOperatorNode>(node.Right);
            var nodeRight = (BinaryOperatorNode)node.Right;
            Assert.IsType<PropertyAccessNode>(nodeRight.Left);
            Assert.Equal("CompanyName", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
            Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
            Assert.IsType<ConstantNode>(nodeRight.Right);
            Assert.Equal("Pool Farm & Primrose Hill Nursery", ((ConstantNode)nodeRight.Right).Value);
        }

        [Fact]
        public void SkipThrowsODataExceptionForNonNumeric()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$skip=A",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            ODataException odataException = Assert.Throws<ODataException>(() => queryOptions.Skip);

            Assert.Equal(ExceptionMessage.QueryOptionValueMustBePositiveInteger("$skip"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void TopThrowsODataExceptionForNonNumeric()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$top=A",
                EntityDataModel.Current.EntitySets["Customers"],
                Mock.Of<IODataQueryOptionsValidator>());

            ODataException odataException = Assert.Throws<ODataException>(() => queryOptions.Top);

            Assert.Equal(ExceptionMessage.QueryOptionValueMustBePositiveInteger("$top"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void Validate_Calls_IODataQueryOptionsValidator()
        {
            var mockODataQueryOptionsValidator = new Mock<IODataQueryOptionsValidator>();

            ODataValidationSettings validationSettings = ODataValidationSettings.All;

            var queryOptions = new ODataQueryOptions("?$top=1", EntityDataModel.Current.EntitySets["Customers"], mockODataQueryOptionsValidator.Object);
            queryOptions.Validate(validationSettings);

            mockODataQueryOptionsValidator.Verify(x => x.Validate(queryOptions, validationSettings), Times.Once());
        }

        public class WhenConstructedWithAllQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            public WhenConstructedWithAllQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$count=true&$expand=Category&$filter=Name eq 'Milk'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Price&$skip=10&$skiptoken=5&$top=25",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheCountOptionShouldBeSet() => Assert.True(_queryOptions.Count);

            [Fact]
            public void TheEntitySetShouldBeSet()
            {
                Assert.NotNull(_queryOptions.EntitySet);
                Assert.Equal("Products", _queryOptions.EntitySet.Name);
            }

            [Fact]
            public void TheExpandOptionShouldBeSet() => Assert.NotNull(_queryOptions.Expand);

            [Fact]
            public void TheFilterOptionShouldBeSet() => Assert.NotNull(_queryOptions.Filter);

            [Fact]
            public void TheFormatOptionShouldBeSet() => Assert.NotNull(_queryOptions.Format);

            [Fact]
            public void TheOrderByPropertyShouldBeSet() => Assert.NotNull(_queryOptions.OrderBy);

            [Fact]
            public void TheRawValuesPropertyShouldBeSet() => Assert.NotNull(_queryOptions.RawValues);

            [Fact]
            public void TheSameExpandOptionInstanceShouldBeReturnedEachTime() => Assert.Same(_queryOptions.Expand, _queryOptions.Expand);

            [Fact]
            public void TheSameFilterOptionInstanceShouldBeReturnedEachTime() => Assert.Same(_queryOptions.Filter, _queryOptions.Filter);

            [Fact]
            public void TheSameFormatOptionInstanceShouldBeReturnedEachTime() => Assert.Same(_queryOptions.Format, _queryOptions.Format);

            [Fact]
            public void TheSameOrderByOptionInstanceShouldBeReturnedEachTime() => Assert.Same(_queryOptions.OrderBy, _queryOptions.OrderBy);

            [Fact]
            public void TheSameSearchOptionInstanceShouldBeReturnedEachTime() => Assert.Same(_queryOptions.Search, _queryOptions.Search);

            [Fact]
            public void TheSameSelectOptionInstanceShouldBeReturnedEachTime() => Assert.Same(_queryOptions.Select, _queryOptions.Select);

            [Fact]
            public void TheSameSkipTokenOptionInstanceShouldBeReturnedEachTime() => Assert.Same(_queryOptions.SkipToken, _queryOptions.SkipToken);

            [Fact]
            public void TheSearchPropertyShouldBeSet() => Assert.NotNull(_queryOptions.Search);

            [Fact]
            public void TheSelectPropertyShouldBeSet() => Assert.NotNull(_queryOptions.Select);

            [Fact]
            public void TheSkipPropertyShouldBeSet() => Assert.NotNull(_queryOptions.Skip);

            [Fact]
            public void TheSkipTokenPropertyShouldBeSet() => Assert.NotNull(_queryOptions.SkipToken);

            [Fact]
            public void TheTopPropertyShouldBeSet() => Assert.NotNull(_queryOptions.Top);
        }

        public class WhenConstructedWithNoQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            public WhenConstructedWithNoQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheCountOptionShouldNotBeSet() => Assert.False(_queryOptions.Count);

            [Fact]
            public void TheEntitySetShouldBeSet()
            {
                Assert.NotNull(_queryOptions.EntitySet);
                Assert.Equal("Products", _queryOptions.EntitySet.Name);
            }

            [Fact]
            public void TheExpandOptionShouldNotBeSet() => Assert.Null(_queryOptions.Expand);

            [Fact]
            public void TheFilterOptionShouldNotBeSet() => Assert.Null(_queryOptions.Filter);

            [Fact]
            public void TheFormatOptionShouldNotBeSet() => Assert.Null(_queryOptions.Format);

            [Fact]
            public void TheOrderByPropertyShouldBeNotSet() => Assert.Null(_queryOptions.OrderBy);

            [Fact]
            public void TheRawValuesPropertyShouldBeSet() => Assert.NotNull(_queryOptions.RawValues);

            [Fact]
            public void TheSearchPropertyShouldNotBeSet() => Assert.Null(_queryOptions.Search);

            [Fact]
            public void TheSelectPropertyShouldBeNotSet() => Assert.Null(_queryOptions.Select);

            [Fact]
            public void TheSkipPropertyShouldBeNotSet() => Assert.Null(_queryOptions.Skip);

            [Fact]
            public void TheSkipTokenPropertyShouldBeNotSet() => Assert.Null(_queryOptions.SkipToken);

            [Fact]
            public void TheTopPropertyShouldBeNotSet() => Assert.Null(_queryOptions.Top);
        }

        /// <summary>
        /// Issue #58 - Plus character in uri should be treated as a space
        /// </summary>
        public class WhenConstructedWithPlusSignsInsteadOfSpacesInTheUrl
        {
            private readonly ODataQueryOptions _queryOptions;

            public WhenConstructedWithPlusSignsInsteadOfSpacesInTheUrl()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename+eq+'John'&$orderby=Forename+asc",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheFilterOptionExpressionShouldBeCorrect()
            {
                Assert.IsType<BinaryOperatorNode>(_queryOptions.Filter.Expression);

                var node = (BinaryOperatorNode)_queryOptions.Filter.Expression;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);
                Assert.IsType<ConstantNode>(node.Right);
            }

            [Fact]
            public void TheFilterOptionShouldBeSet() => Assert.NotNull(_queryOptions.Filter);

            [Fact]
            public void TheFilterOptionShouldHaveTheUnescapedRawValue() => Assert.Equal("$filter=Forename eq 'John'", _queryOptions.Filter.RawValue);

            [Fact]
            public void TheOrderByOptionShouldBeCorrect()
            {
                Assert.Equal(1, _queryOptions.OrderBy.Properties.Count);
                Assert.Equal(OrderByDirection.Ascending, _queryOptions.OrderBy.Properties[0].Direction);
                Assert.Equal("Forename", _queryOptions.OrderBy.Properties[0].PropertyPath.Property.Name);
                Assert.Equal("Forename asc", _queryOptions.OrderBy.Properties[0].RawValue);
            }

            [Fact]
            public void TheOrderByOptionShouldBeSet() => Assert.NotNull(_queryOptions.OrderBy);

            [Fact]
            public void TheOrderByOptionShouldHaveTheUnescapedRawValue() => Assert.Equal("$orderby=Forename asc", _queryOptions.OrderBy.RawValue);
        }

        /// <summary>
        /// Issue #78 - Cannot send + in the request
        /// </summary>
        public class WhenConstructedWithUrlEncodedPlusSignsAndPlusSignsInsteadOfSpacesInTheUrl
        {
            private readonly ODataQueryOptions _queryOptions;

            public WhenConstructedWithUrlEncodedPlusSignsAndPlusSignsInsteadOfSpacesInTheUrl()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename+eq+'John'+and+ImageData+eq+'TG9yZW0gaXBzdW0gZG9s%2Bb3Igc2l0IGF%3D'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheFilterOptionShouldBeSet() => Assert.NotNull(_queryOptions.Filter);

            [Fact]
            public void TheFilterOptionShouldHaveTheUnescapedRawValue()
                => Assert.Equal("$filter=Forename eq 'John' and ImageData eq 'TG9yZW0gaXBzdW0gZG9s+b3Igc2l0IGF='", _queryOptions.Filter.RawValue);
        }
    }
}
