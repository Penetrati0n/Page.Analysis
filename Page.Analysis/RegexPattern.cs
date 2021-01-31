using System;
using System.Collections.Generic;
using System.Text;

namespace Page.Analysis
{
    public static class RegexPattern
    {
        public static string Url_With_Http { get; } = "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b";
        public static string Url_WithOut_Http { get; } = "[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b";
    }
}
