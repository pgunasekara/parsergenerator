using System;
using System.Collections.Generic;

namespace parsergenerator
{
    public class Rule : IGrammar
    {
        public string name;
        public List<IGrammar> elements;
        public Pattern rPattern;

        public Rule()
        {
            this.name = "";
            this.elements = new List<IGrammar>();
            this.rPattern = Pattern.NONE;
        }

        public Rule(string name, List<IGrammar> elements)
        {
            this.name = name;
            this.elements = elements;
        }

        public Rule copy()
        {
            return new Rule(this.name, this.elements);
        }

        public int getRuleMatchLength()
        {
            return 0;
        }
    }
}