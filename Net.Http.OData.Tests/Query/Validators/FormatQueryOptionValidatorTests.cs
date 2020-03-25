using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class FormatQueryValidatorTests
    {
        public class WhenTheFormatQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheFormatQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$format=xml",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FormatQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("The query option $format is not implemented by this service", odataException.Message);
            }
        }

        public class WhenTheFormatQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Format
            };

            public WhenTheFormatQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$format=xml",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FormatQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }
    }
}
