using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace parsergenerator
{
    //Represents a single token within the program
    public class Token : IGrammar
    {
        public String type { get; set; } //The user defined name of the token
        public String val { get; set; } //The actual value of the token
        [JsonIgnore]
        public Pattern rPattern { get; set; } //The pattern for whether the token will have a * ? or +
        [JsonIgnore]
        public RulePattern rulePattern { get; set; } //Unused for token

        public Token(String type, String val)
        {
            this.type = type;
            this.val = val;
            rPattern = Pattern.NONE;
            this.rulePattern = RulePattern.NONE;
        }

        public Token copy()
        {
            return new Token(this.type, this.val);
        }
    }
}