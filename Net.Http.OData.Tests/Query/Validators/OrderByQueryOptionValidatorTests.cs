using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class OrderByQueryValidatorTests
    {
        public class WhenTheOrderByQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheOrderByQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$orderby=Name desc",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnHttpResponseExceptionExceptionIsThrownWithNotImplemented()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => OrderByQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, exception.StatusCode);
                Assert.Equal("The query option $orderby is not implemented by this service", exception.Message);
            }
        }

        public class WhenTheOrderByQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.OrderBy
            };

            public WhenTheOrderByQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$orderby=Name desc",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                OrderByQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }
    }
}
