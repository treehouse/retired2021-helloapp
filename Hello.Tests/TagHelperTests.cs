using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using Hello.Utils;

namespace Hello.Tests
{
    public class TagHelperTests
    {
        [Theory]
        [InlineData("#asdf", "asdf")]
        [InlineData("#c#", "csharp")]
        [InlineData("#f#", "fsharp")]
        [InlineData("#ASDF", "asdf")]
        [InlineData("#AsdF", "asdf")]
        [InlineData("#a)s@df(", "asdf")]
        [InlineData("#asdf", "asdf")]
        [InlineData("#a[s-d]f_", "asdf")]
        public void CleanTest(string tag, string cleanTag)
        {
            Assert.Equal(cleanTag, TagHelper.Clean(tag));
        }

        [Theory]
        [InlineData("#01234567890123456789", "01234567890123456789")]
        [InlineData("#01;234567890123456[789", "01234567890123456789")]
        [InlineData("#012345'678901234]56789", "01234567890123456789")]
        [InlineData("#012345678@901_23456789", "01234567890123456789")]
        [InlineData("#0123456@789012[3456789", "01234567890123456789")]
        [InlineData("#01234567890123456789asdfsdfsadfa", "01234567890123456789")]
        [InlineData("#01234567890123456789423523532452", "01234567890123456789")]
        [InlineData("#012345#67890123456789423523532452", "012345sharp678901234")]
        public void CleanLengthTest(string tag, string cleanTag)
        {
            Assert.Equal(cleanTag, TagHelper.Clean(tag));
        }
    }
}
