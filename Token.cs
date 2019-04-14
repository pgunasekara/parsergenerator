using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace parsergenerator
{
    public class Token : IGrammar
    {
        public String type { get; set; }
        public String val { get; set; }
        [JsonIgnore]
        public Pattern rPattern { get; set; }
        [JsonIgnore]
        public RulePattern rulePattern { get; set; }

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