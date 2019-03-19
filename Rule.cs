using System;

namespace parsergenerator
{
    public class Rule : IGrammar
    {
        public string name;
        public List<IGrammar> tokens;

        public int getRuleMatchLength()
        {
            
        }
    }
}