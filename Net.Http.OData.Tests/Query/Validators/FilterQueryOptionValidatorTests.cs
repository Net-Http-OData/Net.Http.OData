using System.Net;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Validators;
using Xunit;

namespace Net.Http.OData.Tests.Query.Validators
{
    public class FilterQueryOptionValidatorTests
    {
        public class WhenTheFilterQueryOptionContainsTheAddOperatorAddItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheAddOperatorAddItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price add 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator add", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheAddOperatorAddItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.Add,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheAddOperatorAddItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price add 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheAndOperatorAndItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheAndOperatorAndItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename eq 'John' and Surname eq 'Smith'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator and", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheAndOperatorAndItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.And | AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheAndOperatorAndItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename eq 'John' and Surname eq 'Smith'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheCastAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheCastAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=cast(Price, 'Edm.Int64') eq 20",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function cast", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheCastFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Cast,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheCastFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=cast(Price, 'Edm.Int64') eq 20",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheCeilingFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheCeilingFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ceiling(Freight) eq 32",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function ceiling", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheCeilingFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Ceiling,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheCeilingFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ceiling(Freight) eq 32",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheConcatAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheConcatAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=concat(concat(City, ', '), Country) eq 'Berlin, Germany'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function concat", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheConcatFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Concat,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheConcatFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=concat(concat(City, ', '), Country) eq 'Berlin, Germany'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheContainsFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheContainsFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=contains(CompanyName,'Alfreds')",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function contains", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheContainsFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Contains,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheContainsFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=contains(CompanyName,'Alfreds')",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheDateFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheDateFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=date(Date) eq 2019-04-17",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function date", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheDateFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Date,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheDateFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "date(Date) eq 2019-04-17",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheDayFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheDayFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=day(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function day", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheDayFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Day,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheDayFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=day(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheDivideOperatorDivideItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheDivideOperatorDivideItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price div 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator div", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheDivideOperatorDivideItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.Divide,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheDivideOperatorDivideItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price div 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheEndsWithFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheEndsWithFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=endswith(Surname, 'yes') eq true",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function endswith", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheEndsWithFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.EndsWith,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheEndsWithFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=endswith(Surname, 'yes') eq true",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheEqualsOperatorEqualsItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheEqualsOperatorEqualsItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename eq 'John'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator eq", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheEqualsOperatorEqualsItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheEqualsOperatorEqualsItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename eq 'John'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheFloorFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheFloorFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=floor(Freight) eq 32",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function floor", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheFloorFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Floor,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheFloorFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=floor(Freight) eq 32",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheFractionalSecondsAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheFractionalSecondsAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=fractionalseconds(BirthDate) lt 0.1m",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function fractionalseconds", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheFractionalSecondsFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.FractionalSeconds,
                AllowedLogicalOperators = AllowedLogicalOperators.LessThan
            };

            public WhenTheFilterQueryOptionContainsTheFractionalSecondsFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=fractionalseconds(BirthDate) lt 0.1m",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheGreaterThanOperatorGreaterThanItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheGreaterThanOperatorGreaterThanItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price gt 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator gt", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheGreaterThanOperatorGreaterThanItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.GreaterThan
            };

            public WhenTheFilterQueryOptionContainsTheGreaterThanOperatorGreaterThanItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price gt 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheGreaterThanOrEqualsOperatorGreaterThanOrEqualsItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheGreaterThanOrEqualsOperatorGreaterThanOrEqualsItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price ge 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator ge", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheGreaterThanOrEqualsOperatorGreaterThanOrEqualsItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.GreaterThanOrEqual
            };

            public WhenTheFilterQueryOptionContainsTheGreaterThanOrEqualsOperatorGreaterThanOrEqualsItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price ge 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheHasOperatorHasItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheHasOperatorHasItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=AccessLevel has NorthwindModel.AccessLevel'Write'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator has", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheHasOperatorHasItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.Has
            };

            public WhenTheFilterQueryOptionContainsTheHasOperatorHasItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=AccessLevel has NorthwindModel.AccessLevel'Write'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheHourFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheHourFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=hour(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function hour", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheHourFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Hour,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheHourFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=hour(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheIndexOfFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheIndexOfFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=indexof(Surname, 'Hayes') eq 1",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function indexof", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheIndexOfFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.IndexOf,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheIndexOfFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=indexof(Surname, 'Hayes') eq 1",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheIsOfAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheIsOfAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=isof(Price, 'Edm.Int64')",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function isof", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheIsOfFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.IsOf,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheIsOfFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=isof(Price, 'Edm.Int64')",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheLengthFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheLengthFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=length(CompanyName) eq 19",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function length", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheLengthFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Length,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheLengthFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=length(CompanyName) eq 19",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheLessThanOperatorLessThanItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheLessThanOperatorLessThanItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price lt 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator lt", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheLessThanOperatorLessThanItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.LessThan
            };

            public WhenTheFilterQueryOptionContainsTheLessThanOperatorLessThanItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price lt 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheLessThanOrEqualOperatorLessThanOrEqualItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheLessThanOrEqualOperatorLessThanOrEqualItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price le 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator le", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheLessThanOrEqualOperatorLessThanOrEqualItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.LessThanOrEqual
            };

            public WhenTheFilterQueryOptionContainsTheLessThanOrEqualOperatorLessThanOrEqualItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price le 35",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMaxDateTimeAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheMaxDateTimeAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ReleaseDate eq maxdatetime()",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function maxdatetime", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMaxDateTimeFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.MaxDateTime,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheMaxDateTimeFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ReleaseDate eq maxdatetime()",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMinDateTimeAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheMinDateTimeAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ReleaseDate eq mindatetime()",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function mindatetime", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMinDateTimeFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.MinDateTime,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheMinDateTimeFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ReleaseDate eq mindatetime()",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMinuteFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheMinuteFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=minute(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function minute", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMinuteFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Minute,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheMinuteFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=minute(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheModuloOperatorModuloItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheModuloOperatorModuloItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price mod 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator mod", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheModuloOperatorModuloItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.Modulo,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheModuloOperatorModuloItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price mod 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMonthFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheMonthFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=month(BirthDate) eq 5",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function month", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMonthFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Month,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheMonthFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=month(BirthDate) eq 5",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMultiplyOperatorMultiplyItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheMultiplyOperatorMultiplyItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price mul 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator mul", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheMultiplyOperatorMultiplyItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.Multiply,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheMultiplyOperatorMultiplyItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price mul 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheNotEqualsOperatorNotEqualsItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheNotEqualsOperatorNotEqualsItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename ne 'John'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator ne", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheNotEqualsOperatorNotEqualsItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.NotEqual
            };

            public WhenTheFilterQueryOptionContainsTheNotEqualsOperatorNotEqualsItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename ne 'John'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheNotOperatorNotItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.EndsWith,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheNotOperatorNotItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=not endswith(Description, 'ilk')",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator not", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheNotOperatorNotItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.EndsWith,
                AllowedLogicalOperators = AllowedLogicalOperators.Not
            };

            public WhenTheFilterQueryOptionContainsTheNotOperatorNotItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=not endswith(Description, 'ilk')",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheNowAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators = AllowedLogicalOperators.GreaterThanOrEqual
            };

            public WhenTheFilterQueryOptionContainsTheNowAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ReleaseDate eq now()",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function now", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheNowFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Now,
                AllowedLogicalOperators = AllowedLogicalOperators.GreaterThanOrEqual
            };

            public WhenTheFilterQueryOptionContainsTheNowFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=ReleaseDate ge now()",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheOrOperatorOrItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.None
            };

            public WhenTheFilterQueryOptionContainsTheOrOperatorOrItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename eq 'John' or Surname eq 'Smith'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator or", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheOrOperatorOrItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.Or | AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheOrOperatorOrItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Forename eq 'John' or Surname eq 'Smith'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheRoundFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheRoundFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=round(Freight) eq 32",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function round", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheRoundFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Round,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheRoundFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=round(Freight) eq 32",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheSecondFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheSecondFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=second(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function second", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheSecondFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Second,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheSecondFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=second(BirthDate) eq 8",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheStartsWithFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheStartsWithFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=startswith(Surname, 'Hay') eq true",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function startswith", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheStartsWithFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.StartsWith,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheStartsWithFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=startswith(Surname, 'Hay') eq true",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheSubstringFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheSubstringFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=substring(CompanyName, 1) eq 'lfreds Futterkiste'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function substring", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheSubstringFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Substring,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheSubstringFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=substring(CompanyName, 1) eq 'lfreds Futterkiste'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheSubtractOperatorSubtractItIsNotSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheSubtractOperatorSubtractItIsNotSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price sub 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported operator sub", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheSubtractOperatorSubtractItIsSpecifiedInAllowedLogicalOperators
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.Subtract,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal,
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };

            public WhenTheFilterQueryOptionContainsTheSubtractOperatorSubtractItIsSpecifiedInAllowedLogicalOperators()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Price sub 100 eq 150",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTimeAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheTimeAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=time(ReleaseDate) eq 12:00:00",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function time", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTimeFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Time,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheTimeFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=time(ReleaseDate) eq 12:00:00",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheToLowerFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheToLowerFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=tolower(CompanyName) eq 'alfreds futterkiste'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function tolower", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheToLowerFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.ToLower,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheToLowerFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=tolower(CompanyName) eq 'alfreds futterkiste'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTotalOffsetMinutesFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheTotalOffsetMinutesFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=totaloffsetminutes(Date) eq 321541354",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function totaloffsetminutes", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTotalOffsetMinutesFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.TotalOffsetMinutes,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheTotalOffsetMinutesFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=totaloffsetminutes(Date) eq 321541354",
                    EntityDataModel.Current.EntitySets["Orders"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTotalSecondsAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheTotalSecondsAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=totalseconds(ReleaseDate) eq 23487645.224",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function totalseconds", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTotalSecondsFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.TotalSeconds,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheTotalSecondsFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=totalseconds(ReleaseDate) eq 23487645.224",
                    EntityDataModel.Current.EntitySets["Products"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheToUpperFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheToUpperFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=toupper(CompanyName) eq 'ALFREDS FUTTERKISTE'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function toupper", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheToUpperFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.ToUpper,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheToUpperFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=toupper(CompanyName) eq 'ALFREDS FUTTERKISTE'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTrimFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheTrimFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=trim(CompanyName) eq 'Alfreds Futterkiste'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function trim", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheTrimFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Trim,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheTrimFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=trim(CompanyName) eq 'Alfreds Futterkiste'",
                    EntityDataModel.Current.EntitySets["Customers"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheYearFunctionAndItIsNotSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.None
            };

            public WhenTheFilterQueryOptionContainsTheYearFunctionAndItIsNotSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=year(BirthDate) eq 1971",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("Unsupported function year", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionContainsTheYearFunctionAndItIsSpecifiedInAllowedFunctions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedFunctions = AllowedFunctions.Year,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionContainsTheYearFunctionAndItIsSpecifiedInAllowedFunctions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=year(BirthDate) eq 1971",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }

        public class WhenTheFilterQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.None
            };

            public WhenTheFilterQueryOptionIsSetAndItIsNotSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Surname eq 'Smith'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void An_ODataException_IsThrown_WithStatusNotImplemented()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings));

                Assert.Equal(HttpStatusCode.NotImplemented, odataException.StatusCode);
                Assert.Equal("The query option $filter is not implemented by this service", odataException.Message);
                Assert.Equal("$filter", odataException.Target);
            }
        }

        public class WhenTheFilterQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions
        {
            private readonly ODataQueryOptions _queryOptions;

            private readonly ODataValidationSettings _validationSettings = new ODataValidationSettings
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter,
                AllowedLogicalOperators = AllowedLogicalOperators.Equal
            };

            public WhenTheFilterQueryOptionIsSetAndItIsSpecifiedInAllowedQueryOptions()
            {
                TestHelper.EnsureEDM();

                _queryOptions = new ODataQueryOptions(
                    "?$filter=Surname eq 'Smith'",
                    EntityDataModel.Current.EntitySets["Employees"],
                    Mock.Of<IODataQueryOptionsValidator>());
            }

            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                FilterQueryOptionValidator.Validate(_queryOptions, _validationSettings);
            }
        }
    }
}
