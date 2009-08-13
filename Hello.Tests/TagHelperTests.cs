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
        [InlineData("#asdf")]
        [InlineData("#a#sdf")]
        [InlineData("#ASDF")]
        [InlineData("#AsdF")]
        [InlineData("#a)s@d#f(")]
        [InlineData("####asdf")]
        [InlineData("#a[s-d]f_")]
        public void CleanTest(string tag)
        {
            Assert.Equal("asdf", TagHelper.Clean(tag));
        }

        [Theory]
        [InlineData("#01234567890123456789")]
        [InlineData("#01;234567890123456[789")]
        [InlineData("#012345'678901234]56789")]
        [InlineData("#012345678@#901_23456789")]
        [InlineData("#0123456@789012[3456789")]
        [InlineData("#01234567890123456789asdfsdfsadfa")]
        [InlineData("#01234567890123456789423523532452")]
        public void CleanLengthTest(string tag)
        {
            Assert.Equal("01234567890123456789", TagHelper.Clean(tag));
        }
    }
}
