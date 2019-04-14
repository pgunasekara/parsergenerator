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
        public List<IGrammar> elements;
        public Pattern rPattern;
        public RulePattern rulePattern;

        public RuleDescriptor()
        {
            name = "";
            elements = new List<IGrammar>();
            rPattern = Pattern.NONE;
            rulePattern = RulePattern.NONE;
        }

        public RuleDescriptor(string name, List<IGrammar> elements)
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

        public Rule getMatchedRule(ref List<Token> tokens)
        {
            Rule rule = new Rule();
            bool notFound = false;
            
            if(rulePattern == RulePattern.ORRULE)
            {
                Rule longestRule = null;
                int currentMatchLen = 0;
                int matchLength = 0;
                foreach(var element in elements)
                {
                    RuleDescriptor nxt = element as RuleDescriptor;
                    Rule res = nxt.getMatchedRule(ref tokens);

                    if(res == null) { continue; }
                    
                    //Pick the longest rule
                    //only have rules to go through
                    if(currentMatchLen > matchLength || longestRule == null)
                    {
                        matchLength = currentMatchLen;

                        longestRule = res;
                    }
                }
                

                if(longestRule != null)
                {
                    foreach(var element in longestRule.elements)
                    {
                        rule.elements.Add(element);
                    }

                    //Remove tokens
                    tokens.RemoveRange(0, matchLength);
                }
                
            }
            else
            {
                //run the rule normally
                int currentTokenIndex = 0;
                var tokenCopy = tokens.Select(t => new Token(t.type, t.val)).ToList();
                
                foreach(var element in elements)
                {
                    if (element.GetType() == typeof(TokenDescriptor))
                    {
                        TokenDescriptor nxt = element as TokenDescriptor;

                        if(tokenCopy[0].type.Equals(nxt.type)) { rule.elements.Add(tokenCopy[0]); tokenCopy.RemoveAt(0); }
                        else                                   { notFound = true; rule = null; break; }
                    }
                    else if (element.GetType() == typeof(RuleDescriptor))
                    {
                        RuleDescriptor nxt = element as RuleDescriptor;
                        Rule res = nxt.getMatchedRule(ref tokenCopy);

                        if(res != null) { rule.elements.Add(res); }
                        else            { notFound = true; break; }
                        
                    }
                }

                if (notFound) { rule = null; }
                else          { tokens = tokenCopy; }
            }

            return rule;

            /*


            List<Token> smallestTokenList;

            bool notFound = false;

            int currentTokensConsumed;

            //if we match, create a rule from this rule descriptor
            foreach (var element in elements)
            {

                if (element.GetType() == typeof(TokenDescriptor))
                {
                    TokenDescriptor nxt = element as TokenDescriptor;

                    if(tokenCopy[0].type.Equals(nxt.type)) { rule.elements.Add(tokenCopy[0]); tokenCopy.RemoveAt(0); }
                    else                                   { notFound = true; break; }
                }
                else if (element.GetType() == typeof(RuleDescriptor))
                {
                    RuleDescriptor nxt = element as RuleDescriptor;
                    Rule res = nxt.getMatchedRule(ref tokenCopy);

                    if(res != null) { rule.elements.Add(res); tokenCopy.RemoveAt(0); }
                    else            { notFound = true; break; }
                    
                }
            }

            if (notFound) { rule = null; }
            else          { tokens = tokenCopy; }

            return rule;*/
        }
    }
}