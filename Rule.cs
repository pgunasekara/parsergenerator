using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace parsergenerator
{
    public class Rule : IGrammar
    {
        public string name;
        public List<IGrammar> elements;
        [JsonIgnore]
        public Pattern rPattern;
        [JsonIgnore]
        public RulePattern rulePattern;

        public Rule()
        {
            this.name = "";
            this.elements = new List<IGrammar>();
            this.rPattern = Pattern.NONE;
            this.rulePattern = RulePattern.NONE;
        }

        public Rule(string name, List<IGrammar> elements, RulePattern rulePattern)
        {
            this.name = name;
            this.elements = elements;
            this.rulePattern = rulePattern;
        }

        public Rule copy()
        {
            return new Rule(this.name, this.elements, this.rulePattern);
        }

        public int getRuleMatchLength()
        {
            return 0;
        }
    }
}