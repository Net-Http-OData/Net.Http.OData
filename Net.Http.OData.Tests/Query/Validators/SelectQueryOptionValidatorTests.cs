﻿using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class SelectQueryValidatorTests
    {
        public class WhenTheSelectQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheSelectQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$select=Name",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnHttpResponseExceptionExceptionIsThrownWithNotImplemented()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => SelectQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, exception.StatusCode);
                Assert.Equal("The query option $select is not implemented by this service", exception.Message);
            }
        }

        public class WhenTheSelectQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Select
            };

            public WhenTheSelectQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$select=Name",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                SelectQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }
    }
}
