using System;
using System.Collections.Generic;

namespace parsergenerator
{
    public class Rule : IGrammar
    {
        public string name;
        public string returnValue;
        public List<string> locals;
        public List<string> tRules; //temporary
        public List<IGrammar> rules;
        public Pattern rPattern;

        public Rule()
        {
            this.name = "";
            this.returnValue = "";
            this.locals = new List<string>();
            this.tRules = new List<string>();
            this.rules = new List<IGrammar>();
            this.rPattern = Pattern.NONE;
        }

        public int getRuleMatchLength()
        {
            return 0;
        }
    }
}