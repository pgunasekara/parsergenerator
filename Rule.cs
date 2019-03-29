using System;

namespace parsergenerator
{
    public class Rule : IGrammar
    {
        public string name;
        public List<IGrammar> rule;

        public int getRuleMatchLength()
        {
            
        }
    }
}