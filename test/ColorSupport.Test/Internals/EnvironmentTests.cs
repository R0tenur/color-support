using ColorSupport.Internals;
using Shouldly;
using Xunit;
namespace ColorSupport.Test.Internals {
    public class EnvironmentTests {
        [Theory]
        [InlineData ("yo", new string[] { "--foo", "--yo", "--bar" }, true)]
        [InlineData ("--yo", new string[] { "--foo", "--yo", "--bar" }, true)]
        [InlineData ("--yo", new string[] { "--foo", "--", "--yo", "--bar" }, false)]
        [InlineData ("--yo", new string[] { "--foo" }, false)]
        public void HasFlag_ParsesValidParameters (string hasFlag, string[] flags, bool expected) {
            var handler = new Environment (flags);

            var result = handler.HasFlag (hasFlag);

            result.ShouldBe (expected);
        }
    }
}