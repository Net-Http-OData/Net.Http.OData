using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class SkipQueryOptionValidatorTests
    {
        public class WhenTheSkipQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheSkipQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$skip=50",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => SkipQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("The query option $skip is not implemented by this service", odataException.Message);
                Assert.Equal("$skip", odataException.Target);
            }
        }

        public class WhenTheSkipQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Skip
            };

            public WhenTheSkipQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$skip=50",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                SkipQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenValidatingAndTheValueIsAboveZero
        {
            private readonly ODataQueryOptions _queryOptions;

            public WhenValidatingAndTheValueIsAboveZero()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$skip=10",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void NoExceptionIsThrown()
            {
                SkipQueryOptionValidator.Validate(_queryOptions, ODataValidationSettings.All);
            }
        }

        public class WhenValidatingAndTheValueIsBelowZero
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Skip
            };

            public WhenValidatingAndTheValueIsBelowZero()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$skip=-1",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusBadRequest()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => SkipQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
                Assert.Equal("The value for OData query $skip must be a non-negative numeric value", odataException.Message);
                Assert.Equal("$skip", odataException.Target);
            }
        }
    }
}
