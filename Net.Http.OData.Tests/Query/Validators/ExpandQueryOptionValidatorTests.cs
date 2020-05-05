using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class ExpandQueryValidatorTests
    {
        public class WhenTheExpandQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheExpandQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$expand=Category",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => ExpandQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("The query option $expand is not implemented by this service", odataException.Message);
                Assert.Equal("$expand", odataException.Target);
            }
        }

        public class WhenTheExpandQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Expand
            };

            public WhenTheExpandQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$expand=Category",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                ExpandQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }
    }
}
