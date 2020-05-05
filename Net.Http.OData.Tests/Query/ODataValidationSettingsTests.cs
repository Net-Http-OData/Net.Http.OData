using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class ODataValidationSettingsTests
    {
        [Fact]
        public void Equals_ReturnsFalse_ForDifferentValue()
        {
            Assert.False(ODataValidationSettings.None.Equals(ODataValidationSettings.All));
            Assert.False(ODataValidationSettings.None.Equals(default));
            Assert.False(ODataValidationSettings.None.Equals("foo"));

            Assert.False(ODataValidationSettings.None == ODataValidationSettings.All);
            Assert.False(ODataValidationSettings.None == default);
        }

        [Fact]
        public void Equals_ReturnsTrue_ForSameInstance()
        {
            Assert.True(ODataValidationSettings.None.Equals(ODataValidationSettings.None));
            Assert.True(ODataValidationSettings.All.Equals(ODataValidationSettings.All));

            Assert.True(ODataValidationSettings.None == ODataValidationSettings.None);
            Assert.True(ODataValidationSettings.All == ODataValidationSettings.All);

            Assert.True(default(ODataValidationSettings) == default);
        }

        [Fact]
        public void NotEquals_ReturnsFalse_ForSameValue() => Assert.False(ODataValidationSettings.None != ODataValidationSettings.None);

        [Fact]
        public void NotEquals_ReturnsTrue_ForDifferentValue() => Assert.True(ODataValidationSettings.None != ODataValidationSettings.All);

        public class All
        {
            private readonly ODataValidationSettings _all = ODataValidationSettings.All;

            [Fact]
            public void SetsAllowedArithmeticOperatorsToAll() => Assert.Equal(AllowedArithmeticOperators.All, _all.AllowedArithmeticOperators);

            [Fact]
            public void SetsAllowedFunctionsTAllFunctions() => Assert.Equal(AllowedFunctions.AllFunctions, _all.AllowedFunctions);

            [Fact]
            public void SetsAllowedLogicalOperatorsToAll() => Assert.Equal(AllowedLogicalOperators.All, _all.AllowedLogicalOperators);

            [Fact]
            public void SetsAllowedQueryOptionsToAll() => Assert.Equal(AllowedQueryOptions.All, _all.AllowedQueryOptions);

            [Fact]
            public void SetsMaxTopToOneHundred() => Assert.Equal(100, _all.MaxTop);
        }

        public class None
        {
            private readonly ODataValidationSettings _none = ODataValidationSettings.None;

            [Fact]
            public void SetsAllowedArithmeticOperatorsToNone() => Assert.Equal(AllowedArithmeticOperators.None, _none.AllowedArithmeticOperators);

            [Fact]
            public void SetsAllowedFunctionsToNone() => Assert.Equal(AllowedFunctions.None, _none.AllowedFunctions);

            [Fact]
            public void SetsAllowedLogicalOperatorsToNone() => Assert.Equal(AllowedLogicalOperators.None, _none.AllowedLogicalOperators);

            [Fact]
            public void SetsAllowedQueryOptionsToNone() => Assert.Equal(AllowedQueryOptions.None, _none.AllowedQueryOptions);

            [Fact]
            public void SetsMaxTopToZero() => Assert.Equal(0, _none.MaxTop);
        }
    }
}
