using System.Text.RegularExpressions;
using System;
using Newtonsoft.Json;

namespace parsergenerator
{
    //Describes a token using a regular expression
    public class TokenDescriptor : IGrammar
    {
        public String type { get; set; } //name of the descriptor
        [JsonIgnore]
        public string pattern { get; set; } //The regular expression to be used to match this token
        [JsonIgnore]
        public Match match { get; set; }
        public Pattern rPattern { get; set; } //The pattern for whether the rule will have a * ? or +
        public RulePattern rulePattern { get; set; } //Unused for token descriptor
        

        public TokenDescriptor() { }

        public TokenDescriptor(String type, string pattern)
        {
            this.type = type;
            this.pattern = pattern;
            this.rPattern = Pattern.NONE;
            this.rulePattern = RulePattern.NONE;
        }

        public TokenDescriptor copy()
        {
            return new TokenDescriptor(this.type, this.pattern);
        }

        /// <summary>
        /// Check if a given string matches this regular expression from the begining of the string
        /// </summary>
        /// <param name="input">A string to match to the token</param>
        /// <returns>The value of the match if found, or an emptry string if no match</returns>
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