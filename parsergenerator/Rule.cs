using System.Diagnostics;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace parsergenerator
{
    /// <summary>
    /// Represents one rule in the program.
    /// </summary>
    public class Rule : IGrammar
    {
        public string name; //The name of the rule
        public List<IGrammar> elements; //A list of tokens or other rules in this rule
        [JsonIgnore]
        public Pattern rPattern; //The pattern for whether the rule will have a * ? or +
        [JsonIgnore]
        public RulePattern rulePattern; //Whether the rule is an OR rule, meaning that it only has subrules within it and nothing else

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

        /// <summary>
        /// Copy constructor to allow for a deep copy of the object
        /// </summary>
        /// <returns></returns>
        public Rule copy()
        {
            return new Rule(this.name, this.elements, this.rulePattern);
        }

        public bool ShouldSerializename()
        {
            return true;
        }

        /// <summary>
        /// Recursively get the length of this rule so that we can pick the longest matching rule.
        /// </summary>
        /// <returns>The length of the rule</returns>
        public int getRuleMatchLength()
        {
            int length = 0;

            foreach(var element in elements)
            {
                if (element.GetType() == typeof(Token))      { length++; }
                else if (element.GetType() == typeof(Rule))  { Rule nxt = element as Rule; length += nxt.getRuleMatchLength(); }
                else                                         { Debug.Assert(false); /* Should never be here */ }
            }

            return length;
        }
    }
}