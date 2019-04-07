using System.Text.RegularExpressions;
using System;

namespace parsergenerator
{
    public class TokenDescriptor
    {
        public String type { get; set; }
        public string pattern { get; set; }
        public Match match { get; set; }

        public TokenDescriptor() { }
        public TokenDescriptor(String type, string pattern)
        {
            this.type = type;
            this.pattern = pattern;
        }

        public string getMatch(string input)
        {
            match = Regex.Match(input, @"^" + pattern);

            if(match.Success)
            {
                return match.Value;
            }

            return "";
        }
    }
}