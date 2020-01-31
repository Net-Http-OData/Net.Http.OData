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
        public void Constructor_ThrowsArgumentNullException_ForNullEntitySet()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(() => new ODataQueryOptions("", null, Mock.Of<IODataQueryOptionsValidator>()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullQuery()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(
                () => new ODataQueryOptions(null, EntityDataModel.Current.EntitySets["Products"], Mock.Of<IODataQueryOptionsValidator>()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullValidator()
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

            var option = new ODataQueryOptions(
                "?$filter=LegacyId+eq+2139+and+CompanyName+eq+'Pool+Farm+%26+Primrose+Hill+Nursery'&$top=1",
                EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());

            Assert.NotNull(option);
            Assert.NotNull(option.Filter);

            Assert.NotNull(option.Filter.Expression);
            Assert.IsType<BinaryOperatorNode>(option.Filter.Expression);

            var node = (BinaryOperatorNode)option.Filter.Expression;

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

            var option = new ODataQueryOptions(
                "?$skip=A",
                EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());

            ODataException exception = Assert.Throws<ODataException>(() => option.Skip);

            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("The value for OData query $skip must be a non-negative numeric value", exception.Message);
        }

        [Fact]
        public void TopThrowsODataExceptionForNonNumeric()
        {
            TestHelper.EnsureEDM();

            var option = new ODataQueryOptions(
                "?$top=A",
                EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());

            ODataException exception = Assert.Throws<ODataException>(() => option.Top);

            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("The value for OData query $top must be a non-negative numeric value", exception.Message);
        }

        public class WhenConstructedWithAllQueryOptions
        {
            private readonly ODataQueryOptions _option;

            public WhenConstructedWithAllQueryOptions()
            {
                TestHelper.EnsureEDM();

                _option = new ODataQueryOptions(
                    "?$count=true&$expand=Category&$filter=Name eq 'Milk'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Price&$skip=10&$skiptoken=5&$top=25",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheCountOptionShouldBeSet()
            {
                Assert.True(_option.Count);
            }

            [Fact]
            public void TheEntitySetShouldBeSet()
            {
                Assert.NotNull(_option.EntitySet);
            }

            [Fact]
            public void TheExpandOptionShouldBeSet()
            {
                Assert.NotNull(_option.Expand);
            }

            [Fact]
            public void TheFilterOptionShouldBeSet()
            {
                Assert.NotNull(_option.Filter);
            }

            [Fact]
            public void TheFormatOptionShouldBeSet()
            {
                Assert.NotNull(_option.Format);
            }

            [Fact]
            public void TheOrderByPropertyShouldBeSet()
            {
                Assert.NotNull(_option.OrderBy);
            }

            [Fact]
            public void TheRawValuesPropertyShouldBeSet()
            {
                Assert.NotNull(_option.RawValues);
            }

            [Fact]
            public void TheSearchPropertyShouldBeSet()
            {
                Assert.NotNull(_option.Search);
                Assert.Equal("blue OR green", _option.Search);
            }

            [Fact]
            public void TheSelectPropertyShouldBeSet()
            {
                Assert.NotNull(_option.Select);
            }

            [Fact]
            public void TheSkipPropertyShouldBeSet()
            {
                Assert.NotNull(_option.Skip);
            }

            [Fact]
            public void TheSkipTokenPropertyShouldBeSet()
            {
                Assert.NotNull(_option.SkipToken);
            }

            [Fact]
            public void TheTopPropertyShouldBeSet()
            {
                Assert.NotNull(_option.Top);
            }
        }

        public class WhenConstructedWithNoQueryOptions
        {
            private readonly ODataQueryOptions _option;

            public WhenConstructedWithNoQueryOptions()
            {
                TestHelper.EnsureEDM();

                _option = new ODataQueryOptions(
                    "",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheCountOptionShouldNotBeSet()
            {
                Assert.False(_option.Count);
            }

            [Fact]
            public void TheEntitySetShouldBeSet()
            {
                Assert.NotNull(_option.EntitySet);
            }

            [Fact]
            public void TheExpandOptionShouldNotBeSet()
            {
                Assert.Null(_option.Expand);
            }

            [Fact]
            public void TheFilterOptionShouldNotBeSet()
            {
                Assert.Null(_option.Filter);
            }

            [Fact]
            public void TheFormatOptionShouldNotBeSet()
            {
                Assert.Null(_option.Format);
            }

            [Fact]
            public void TheOrderByPropertyShouldBeNotSet()
            {
                Assert.Null(_option.OrderBy);
            }

            [Fact]
            public void TheRawValuesPropertyShouldBeSet()
            {
                Assert.NotNull(_option.RawValues);
            }

            [Fact]
            public void TheSearchPropertyShouldNotBeSet()
            {
                Assert.Null(_option.Search);
            }

            [Fact]
            public void TheSelectPropertyShouldBeNotSet()
            {
                Assert.Null(_option.Select);
            }

            [Fact]
            public void TheSkipPropertyShouldBeNotSet()
            {
                Assert.Null(_option.Skip);
            }

            [Fact]
            public void TheSkipTokenPropertyShouldBeNotSet()
            {
                Assert.Null(_option.SkipToken);
            }

            [Fact]
            public void TheTopPropertyShouldBeNotSet()
            {
                Assert.Null(_option.Top);
            }
        }

        /// <summary>
        /// Issue #58 - Plus character in uri should be treated as a space
        /// </summary>
        public class WhenConstructedWithPlusSignsInsteadOfSpacesInTheUrl
        {
            private readonly ODataQueryOptions _option;

            public WhenConstructedWithPlusSignsInsteadOfSpacesInTheUrl()
            {
                TestHelper.EnsureEDM();

                _option = new ODataQueryOptions(
                    "?$filter=Forename+eq+'John'&$orderby=Forename+asc",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheFilterOptionExpressionShouldBeCorrect()
            {
                Assert.IsType<BinaryOperatorNode>(_option.Filter.Expression);

                var node = (BinaryOperatorNode)_option.Filter.Expression;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);
                Assert.IsType<ConstantNode>(node.Right);
            }

            [Fact]
            public void TheFilterOptionShouldBeSet()
            {
                Assert.NotNull(_option.Filter);
            }

            [Fact]
            public void TheFilterOptionShouldHaveTheUnescapedRawValue()
            {
                Assert.Equal("$filter=Forename eq 'John'", _option.Filter.RawValue);
            }

            [Fact]
            public void TheOrderByOptionShouldBeCorrect()
            {
                Assert.Equal(1, _option.OrderBy.Properties.Count);
                Assert.Equal(OrderByDirection.Ascending, _option.OrderBy.Properties[0].Direction);
                Assert.Equal("Forename", _option.OrderBy.Properties[0].PropertyPath.Property.Name);
                Assert.Equal("Forename asc", _option.OrderBy.Properties[0].RawValue);
            }

            [Fact]
            public void TheOrderByOptionShouldBeSet()
            {
                Assert.NotNull(_option.OrderBy);
            }

            [Fact]
            public void TheOrderByOptionShouldHaveTheUnescapedRawValue()
            {
                Assert.Equal("$orderby=Forename asc", _option.OrderBy.RawValue);
            }
        }

        /// <summary>
        /// Issue #78 - Cannot send + in the request
        /// </summary>
        public class WhenConstructedWithUrlEncodedPlusSignsAndPlusSignsInsteadOfSpacesInTheUrl
        {
            private readonly ODataQueryOptions _option;

            public WhenConstructedWithUrlEncodedPlusSignsAndPlusSignsInsteadOfSpacesInTheUrl()
            {
                TestHelper.EnsureEDM();

                _option = new ODataQueryOptions(
                    "?$filter=Forename+eq+'John'+and+ImageData+eq+'TG9yZW0gaXBzdW0gZG9s%2Bb3Igc2l0IGF%3D'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void TheFilterOptionShouldBeSet()
            {
                Assert.NotNull(_option.Filter);
            }

            [Fact]
            public void TheFilterOptionShouldHaveTheUnescapedRawValue()
            {
                Assert.Equal("$filter=Forename eq 'John' and ImageData eq 'TG9yZW0gaXBzdW0gZG9s+b3Igc2l0IGF='", _option.Filter.RawValue);
            }
        }
    }
}
