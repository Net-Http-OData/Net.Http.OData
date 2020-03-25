using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class SkipTokenQueryOptionValidatorTests
    {
        public class WhenTheSkipTokenQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheSkipTokenQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$skiptoken=5",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => SkipTokenQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("The query option $skiptoken is not implemented by this service", odataException.Message);
            }
        }

        public class WhenTheSkipTokenQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.SkipToken
            };

            public WhenTheSkipTokenQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$skiptoken=5",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                SkipTokenQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }
    }
}
