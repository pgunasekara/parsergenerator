using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace parsergenerator
{
    /// <summary>
    /// Used to describe a rule with a set of token descriptors and rule descriptors
    /// </summary>
    public class RuleDescriptor : IGrammar
    {
        public string name; //The name of the rule
        public List<IGrammar> elements; //a list of token and rule descriptors that make up this rule
        public Pattern rPattern; //The pattern for whether the rule will have a * ? or +
        public RulePattern rulePattern; //Whether the rule is an OR rule, meaning that it only has subrules within it and nothing else

        public RuleDescriptor()
        {
            name = "";
            elements = new List<IGrammar>();
            rPattern = Pattern.NONE;
            rulePattern = RulePattern.NONE;
        }

        public RuleDescriptor(string name, List<IGrammar> elements, RulePattern rulePattern)
        {
            this.name = name;
            this.elements = elements;
            this.rulePattern = rulePattern;
        }

        public RuleDescriptor(string name, List<IGrammar> elements, Pattern rPattern, RulePattern rulePattern)
        {
            this.name = name;
            this.elements = elements;
            this.rPattern = rPattern;
            this.rulePattern = rulePattern;
        }

        /// <summary>
        /// Copy function to create a deep copy of this object
        /// </summary>
        /// <returns>A deep copy of this object</returns>
        public RuleDescriptor copy()
        {
            return new RuleDescriptor(this.name, this.elements, this.rulePattern);
        }

        /// <summary>
        /// Creates the longest matching Rule while consuming as many tokens as possible. This will be called recursively
        /// on sub rule descriptors to find the longest possible match.
        /// </summary>
        /// <param name="tokens">A list of tokens passed in by reference so that this function may consume and return the smaller list of tokens</param>
        /// <param name="rules">A dictionary of rules to look up the nextrule to be matched.</param>
        /// <returns>A final rule if everything matches, else returns NULL to indicate an error in parsing</returns>
        public Rule getMatchedRule(ref List<Token> tokens, Dictionary<string, RuleDescriptor> rules)
        {
            Rule rule = new Rule(); //The final rule to be returned
            bool notFound = false;
            
            //The first case is if this is an OR rule, which means that all elements within this rule descriptor are other rule descriptors
            //The objective is find the longest matching rule out of the given rules
            if(rulePattern == RulePattern.ORRULE)
            {
                Rule longestRule = null;
                int currentMatchLen = 0;
                int longestMatch = 0;
                foreach(var element in elements)
                {
                    var tnxt = element as RuleDescriptor;
                    //look up the proper rule
                    var nxt = rules[tnxt.name];
                    nxt.rPattern = tnxt.rPattern;

                    //create a copy of the tokens because we're not ready to consume the tokens until the longest match is found
                    var tokenCopy = tokens.Select(t => new Token(t.type, t.val)).ToList(); 
                    Rule res = nxt.getMatchedRule(ref tokenCopy, rules); //recursively call this function until we hit the end of the rule

                    if(res == null) { continue; } //if no matches are found, then this rule was not matched

                    currentMatchLen = res.getRuleMatchLength();
                    
                    //Pick the longest rule
                    if(currentMatchLen > longestMatch || longestRule == null)
                    {
                        longestMatch = currentMatchLen;
                        longestRule = res;
                    }
                }
                

                //if a matching longest rule was found, then add all the rule elements into the final rule and consume the tokens
                if(longestRule != null)
                {
                    foreach(var element in longestRule.elements)
                    {
                        rule.elements.Add(element);
                    }

                    //Remove tokens
                    tokens.RemoveRange(0, longestMatch);
                }
                else
                {
                    rule = null;
                }
            }
            else
            {
                //create a copy of the tokens because we're not ready to consume the tokens until the longest match is found
                var tokenCopy = tokens.Select(t => new Token(t.type, t.val)).ToList();
                
                foreach(var element in elements)
                {
                    //in this case our rule could be made of other rules or tokens, so we want to check that first
                    if (element.GetType() == typeof(TokenDescriptor))
                    {
                        TokenDescriptor nxt = element as TokenDescriptor;
                        
                        //Check if the token needs to loop
                        switch(nxt.rPattern)
                        {
                            case Pattern.ONEORMORE:
                            {
                                //If the pattern is + then there must be at least one match. If no match is found then assert false
                                var tokenMatch = tokenCopy[0].type.Equals(nxt.type);
                                do
                                {
                                    if(tokenMatch)
                                    {
                                        //When found, consume the token and add to the final rule
                                        rule.elements.Add(tokenCopy[0]);
                                        tokenCopy.RemoveAt(0);
                                        tokenMatch = tokenCopy[0].type.Equals(nxt.type);
                                    }
                                    else
                                    {
                                        Debug.Assert(false);
                                    }
                                    
                                } while(tokenMatch); //keep going until we stop matching
                                continue;
                            }
                            case Pattern.ZEROORMORE:
                            {
                                // * token has been found, since it is optional, if no match then we move to the next token/rule
                                var tokenMatch = tokenCopy[0].type.Equals(nxt.type);
                                while(tokenMatch)
                                {
                                    rule.elements.Add(tokenCopy[0]);
                                    tokenCopy.RemoveAt(0);
                                    tokenMatch = tokenCopy[0].type.Equals(nxt.type);
                                }
                                continue;
                            }
                            case Pattern.OPTIONAL:
                                // ? check if the token occurs at least once, if it does then add, else move to the next token/rule
                                if(tokenCopy[0].type.Equals(nxt.type)) { rule.elements.Add(tokenCopy[0]); tokenCopy.RemoveAt(0); }
                                continue;
                            default:
                                //proceed with regular checks
                                break;
                        }

                        //If there is no pattern on the token, try to match it at least once, if no match, then the rule could not be matched and return null
                        if(tokenCopy[0].type.Equals(nxt.type)) { rule.elements.Add(tokenCopy[0]); tokenCopy.RemoveAt(0); }
                        else                                   { notFound = true; rule = null; break; }
                    }
                    else if (element.GetType() == typeof(RuleDescriptor))
                    {
                        //In this case, the next part is a rule, so we need to call the matching getMatchedRule() function recursively to get the longest rule
                        var tnxt = element as RuleDescriptor;
                        //look up the proper rule
                        var nxt = rules[tnxt.name];
                        nxt.rPattern = tnxt.rPattern;

                        Rule res = nxt.getMatchedRule(ref tokenCopy, rules);

                        //Check if the rule needs to loop
                        switch(nxt.rPattern)
                        {
                            case Pattern.ZEROORMORE:
                            case Pattern.ONEORMORE:
                            {
                                //If + then ensure that at least one match happened, if * then whether there is one match or no match doesn't matter.
                                if(res == null) { 
                                    if(nxt.rPattern == Pattern.ONEORMORE) { Debug.Assert(false); } // +
                                    if(nxt.rPattern == Pattern.ZEROORMORE) { continue; } // *
                                }
                                //Keep looking until we can't find more matches
                                while(res != null)
                                {
                                    res.name = nxt.name; rule.elements.Add(res);
                                    res = nxt.getMatchedRule(ref tokenCopy, rules);
                                }
                                continue;
                            }
                            case Pattern.OPTIONAL:
                            {
                                // ? only match once, if matched then add to final rule, else move on
                                if(res == null) { continue; }
                                rule.elements.Add(res);
                                continue;
                            }
                            default:
                                //proceed with regular checks
                                break;
                        }

                        //Match rule at least once
                        if(res != null) { res.name = nxt.name; rule.elements.Add(res); }
                        else            { notFound = true; break; }
                        
                    }
                }

                if (notFound) { rule = null; }
                else          { tokens = tokenCopy; }
            }

            return rule;
        }
    }
}