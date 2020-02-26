using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class TopQueryValidatorTests
    {
        public class WhenTheTopQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheTopQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$top=50",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => TopQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("The query option $top is not implemented by this service", odataException.Message);
            }
        }

        public class WhenTheTopQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Top,
                MaxTop = 100
            };

            public WhenTheTopQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$top=50",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                TopQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenValidatingAndNoMaxTopIsSetButTheValueIsBelowZero
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Top
            };

            public WhenValidatingAndNoMaxTopIsSetButTheValueIsBelowZero()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$top=-1",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusBadRequest()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => TopQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
                Assert.Equal("The integer value for $top is invalid, it must be an integer greater than zero and below the max value of 0 allowed by this service", odataException.Message);
            }
        }

        public class WhenValidatingAndTheQueryOptionDoesNotExceedTheSpecifiedMaxTopValue
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Top,
                MaxTop = 100
            };

            public WhenValidatingAndTheQueryOptionDoesNotExceedTheSpecifiedMaxTopValue()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$top=25",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void NoExceptionIsThrown()
            {
                TopQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenValidatingAndTheQueryOptionDoesNotSpecifyATopValue
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                MaxTop = 100
            };

            public WhenValidatingAndTheQueryOptionDoesNotSpecifyATopValue()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void NoExceptionIsThrown()
            {
                TopQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenValidatingAndTheQueryOptionExceedsTheSpecifiedMaxTopValue
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Top,
                MaxTop = 100
            };

            public WhenValidatingAndTheQueryOptionExceedsTheSpecifiedMaxTopValue()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$top=150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusBadRequest()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => TopQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
                Assert.Equal("The integer value for $top is invalid, it must be an integer greater than zero and below the max value of 100 allowed by this service", odataException.Message);
            }
        }
    }
}
