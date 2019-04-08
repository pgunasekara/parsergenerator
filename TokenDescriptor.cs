using System.Text.RegularExpressions;
using System;
using Newtonsoft.Json;

namespace parsergenerator
{
    public class TokenDescriptor : IGrammar
    {
        public String type { get; set; }
        [JsonIgnore]
        public string pattern { get; set; }
        [JsonIgnore]
        public Match match { get; set; }
        public Pattern rPattern { get; set; }
        

        public TokenDescriptor() { }

        public TokenDescriptor(String type, string pattern)
        {
            this.type = type;
            this.pattern = pattern;
            this.rPattern = Pattern.NONE;
        }

        public TokenDescriptor copy()
        {
            return new TokenDescriptor(this.type, this.pattern);
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