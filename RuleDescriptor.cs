using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace parsergenerator
{
    /**
     * Builds a rule by expanding out a rule using it's regular expressions
     */
    
    public class RuleDescriptor : IGrammar
    {
        public string name;
        public List<List<IGrammar>> elements;
        public Pattern rPattern;

        public RuleDescriptor()
        {
            name = "";
            elements = new List<List<IGrammar>>();
            rPattern = Pattern.NONE;
        }

        public RuleDescriptor(string name, List<List<IGrammar>> elements)
        {
            this.name = name;
            this.elements = elements;
        }

        public RuleDescriptor copy()
        {
            return new RuleDescriptor(this.name, this.elements);
        }

        public RuleDescriptor(string input, List<Token> tokens, List<Rule> rules)
        {
            //Create a descriptor based on given string
            var lines = input.Split(",");
            var lineNum = 0;
            //Get name from first line
            this.name = lines[lineNum++];
            Regex endRule = new Regex(@"^[\s]*:[\s]*$");
            //Parse rule elements up to colon
            while (!endRule.Match(lines[lineNum]).Success)
            {
                //Add rule elements
                //Check if existing rule or token

                //Check if Pattern

            }

            /*while(lineNum < lines.Count)
            {
                var currentLine = lines[lineNum++];
                var elements = currentLine.Split(@"\s+");

                
            }*/


            //this.ruleElements = ruleElements;
        }

        public Rule generateRule(List<IGrammar> tokenStream)
        {
            Rule rule = new Rule();

            foreach (IGrammar token in tokenStream)
            {

            }

            return rule;
        }
    }
}