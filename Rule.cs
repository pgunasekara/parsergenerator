using System;
using System.Collections.Generic;

namespace parsergenerator
{
    public class Rule : IGrammar
    {
        public string name;
        public List<IGrammar> rules;
        public Pattern rPattern;

        public Rule()
        {
            this.name = "";
            this.rules = new List<IGrammar>();
            this.rPattern = Pattern.NONE;
        }

        public Rule(string name, List<IGrammar> rules)
        {
            this.name = name;
            this.rules = rules;
        }

        public Rule copy()
        {
            return new Rule(this.name, this.rules);
        }

        public int getRuleMatchLength()
        {
            return 0;
        }
    }
}