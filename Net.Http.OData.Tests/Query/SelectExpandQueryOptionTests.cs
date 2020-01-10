using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class SelectExpandQueryOptionTests
    {
        public class WhenConstructedWithExpandSingleValue
        {
            private readonly SelectExpandQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithExpandSingleValue()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Products"].EdmType;

                _rawValue = "$expand=Category";
                _option = new SelectExpandQueryOption(_rawValue, model);
            }

            [Fact]
            public void ThePropertiesShouldContainSpecifiedNavigationProperty()
            {
                Assert.Equal(1, _option.PropertyPaths.Count);
                Assert.All(_option.PropertyPaths, p => Assert.Null(p.Next));

                Assert.Equal("Category", _option.PropertyPaths[0].Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }

        public class WhenConstructedWithExpandStarValue
        {
            private readonly SelectExpandQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithExpandStarValue()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Products"].EdmType;

                _rawValue = "$expand=*";
                _option = new SelectExpandQueryOption(_rawValue, model);
            }

            [Fact]
            public void ThePropertiesShouldContainAllNavigationProperties()
            {
                Assert.Equal(1, _option.PropertyPaths.Count);
                Assert.All(_option.PropertyPaths, p => Assert.Null(p.Next));

                Assert.Equal("Category", _option.PropertyPaths[0].Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }

        public class WhenConstructedWithSelectMultipleValues
        {
            private readonly SelectExpandQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithSelectMultipleValues()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Products"].EdmType;

                _rawValue = "$select=Name,Price,ReleaseDate";
                _option = new SelectExpandQueryOption(_rawValue, model);
            }

            [Fact]
            public void ThePropertiesShouldContainSpecifiedProperties()
            {
                Assert.Equal(3, _option.PropertyPaths.Count);
                Assert.All(_option.PropertyPaths, p => Assert.Null(p.Next));

                Assert.Equal("Name", _option.PropertyPaths[0].Property.Name);
                Assert.Equal("Price", _option.PropertyPaths[1].Property.Name);
                Assert.Equal("ReleaseDate", _option.PropertyPaths[2].Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }

        public class WhenConstructedWithSelectPropertyPathValue
        {
            private readonly SelectExpandQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithSelectPropertyPathValue()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Products"].EdmType;

                _rawValue = "$select=Category/Name";
                _option = new SelectExpandQueryOption(_rawValue, model);
            }

            [Fact]
            public void ThePropertiesShouldContainSpecifiedProperty()
            {
                Assert.Equal(1, _option.PropertyPaths.Count);

                PropertyPathSegment propertyPathSegment = _option.PropertyPaths[0];

                Assert.Equal("Category", propertyPathSegment.Property.Name);
                Assert.Equal("Product", propertyPathSegment.Property.DeclaringType.Name);
                Assert.NotNull(propertyPathSegment.Next);

                propertyPathSegment = propertyPathSegment.Next;

                Assert.Equal("Name", propertyPathSegment.Property.Name);
                Assert.Equal("Category", propertyPathSegment.Property.DeclaringType.Name);
                Assert.Null(propertyPathSegment.Next);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }

        public class WhenConstructedWithSelectSingleValue
        {
            private readonly SelectExpandQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithSelectSingleValue()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Products"].EdmType;

                _rawValue = "$select=Name";
                _option = new SelectExpandQueryOption(_rawValue, model);
            }

            [Fact]
            public void ThePropertiesShouldContainSpecifiedProperty()
            {
                Assert.Equal(1, _option.PropertyPaths.Count);
                Assert.All(_option.PropertyPaths, p => Assert.Null(p.Next));

                Assert.Equal("Name", _option.PropertyPaths[0].Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }

        public class WhenConstructedWithSelectStarValue
        {
            private readonly EdmComplexType _model;
            private readonly SelectExpandQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithSelectStarValue()
            {
                TestHelper.EnsureEDM();

                _model = EntityDataModel.Current.EntitySets["Products"].EdmType;
                _rawValue = "$select=*";
                _option = new SelectExpandQueryOption(_rawValue, _model);
            }

            [Fact]
            public void ThePropertiesShouldContainAllProperties()
            {
                Assert.Equal(8, _option.PropertyPaths.Count);
                Assert.All(_option.PropertyPaths, p => Assert.Null(p.Next));

                Assert.Equal("Colour", _option.PropertyPaths[0].Property.Name);
                Assert.Equal("Deleted", _option.PropertyPaths[1].Property.Name);
                Assert.Equal("Description", _option.PropertyPaths[2].Property.Name);
                Assert.Equal("Name", _option.PropertyPaths[3].Property.Name);
                Assert.Equal("Price", _option.PropertyPaths[4].Property.Name);
                Assert.Equal("ProductId", _option.PropertyPaths[5].Property.Name);
                Assert.Equal("Rating", _option.PropertyPaths[6].Property.Name);
                Assert.Equal("ReleaseDate", _option.PropertyPaths[7].Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }
    }
}
