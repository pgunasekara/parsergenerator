namespace parsergenerator
{
    /**
     * Builds a rule by expanding out a rule using it's regular expressions
     */
    public class RuleDescriptor : IGrammar
    {
        public enum Pattern
        {
            NONE = 0,
            ZEROORMORE = 1,
            ONEORMORE = 2,
            OPTIONAL = 3
        }

        string name;
        public List<IGrammar> ruleStructure;

        public RuleDescriptor(List<IGrammar> ruleStructure)
        {
            this.ruleStructure = ruleStructure;
        }

        public Rule generateRule(List<IGrammar> tokenStream)
        {
            Rule rule = new Rule();

            foreach(IGrammar token in tokenStream)
            {
                
            }

            return rule;
        }
    }
}