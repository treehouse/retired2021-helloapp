using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hello.Utils
{
    public static class TagHelper
    {
        public static string Clean(string tag)
        {
            var cleanTag = tag
                .Trim()
                .ToLower();
            if (cleanTag.StartsWith("#"))
                cleanTag = cleanTag.Substring(1);
            cleanTag = cleanTag
                .Replace("#", "sharp");
            cleanTag = Regex.Replace(
                cleanTag,
                @"[\W_]", "");

            return cleanTag.Length > 20 ? cleanTag.Substring(0, 20) : cleanTag;
        }
    }
}
