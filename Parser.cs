using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

namespace parsergenerator
{
    public class Parser
    {
        public Parser() {}

        public List<RuleDescriptor> generateRuleDescriptors(string inputFile, List<TokenDescriptor> allTokens)
        {
            List<RuleDescriptor> rules = new List<RuleDescriptor>();

            using(StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    RuleDescriptor r = new RuleDescriptor();
                    int start = 0;
                    int end = 0;
                    //Skip any whitespace 
                    while(String.IsNullOrWhiteSpace(line))   { line = reader.ReadLine(); }
                    //Get name of rule
                    r.name = line; line = reader.ReadLine();

                    //Skip any whitespace 
                    while(String.IsNullOrWhiteSpace(line))   { line = reader.ReadLine(); }

                    #region old 
                    /*if(line.StartsWith("returns"))
                    {
                        //Get return values
                        start = line.IndexOf('[') + 1;
                        end = line.IndexOf(']');
                        r.returnValue = line.Substring(start, end - start); line = reader.ReadLine();
                    }

                    //Skip any whitespace 
                    while(String.IsNullOrWhiteSpace(line))   { line = reader.ReadLine(); }
                    //Get locals
                    if(line.StartsWith("locals"))
                    {
                        start = line.IndexOf('[') + 1;
                        end = line.IndexOf(']');
                        r.locals = Regex.Split(line.Substring(start, end - start), @"\s*,\s*").ToList(); line = reader.ReadLine();
                    }*/
                    #endregion

                    //Store contents of rule
                    if(Regex.Match(line, @"^\s*:\s*$").Success)
                    {
                        List<IGrammar> rElements = new List<IGrammar>();
                        bool isOrRule = false, initRun = true;
                        int i = 0;
                        
                        RuleDescriptor currElement = null;

                        //Parse the rest up to the semicolon
                        while((line = reader.ReadLine()) != null)
                        {
                            if(initRun)
                            {
                            //Check if it's an OR rule
                                if(line.IndexOf('|') == -1)
                                {
                                    isOrRule = false;
                                    currElement = r;
                                }
                                else
                                {
                                    isOrRule = true;
                                    currElement = new RuleDescriptor();
                                    currElement.name = r.name + "_" + i++;
                                }
                                initRun = false;
                            }

                            if(Regex.Match(line, @"^\s*;\s*$").Success) { break; }
                            //strip any spaces
                            line = line.Trim();

                            var parts = line.Split(' ');
                            foreach (string part in parts)
                            {
                                
                                Pattern patt = Pattern.NONE;
                                //Check if current part is an OR
                                if(part.Equals("|"))
                                {
                                    r.rulePattern = RulePattern.ORRULE;
                                    rElements.Add(currElement);
                                    currElement = new RuleDescriptor();
                                    currElement.name = r.name + "_" + i++;
                                }

                                string pCopy = part;

                                //Check for pattern modifier
                                if(pCopy.EndsWith('+'))
                                {
                                    patt = Pattern.ONEORMORE;
                                }
                                else if(pCopy.EndsWith('*'))
                                {
                                    patt = Pattern.ZEROORMORE;
                                }
                                else if(pCopy.EndsWith('?'))
                                {
                                    patt = Pattern.ZEROORONE;
                                }

                                if(patt != Pattern.NONE)
                                {
                                    pCopy = pCopy.Substring(0, pCopy.Length-1);
                                }

                                var tMatch = allTokens.Find(n => n.type.Equals(pCopy)); //correctly matched token
                                if(tMatch != null)
                                {
                                    var tMatchCopy = tMatch.copy();
                                    tMatchCopy.rPattern = patt;
                                    currElement.elements.Add(tMatchCopy);
                                }
                                else if (pCopy.Equals(r.name))
                                {
                                    //handle this case
                                    var rCopy = r.copy();
                                    rCopy.rPattern = patt;
                                    currElement.elements.Add(rCopy);
                                }
                                else 
                                {
                                    //check if it's a rule that exists
                                    var rMatch = rules.Find(n => n.name.Equals(pCopy));
                                    if(rMatch != null)
                                    {
                                        var rMatchCopy = rMatch.copy();
                                        rMatchCopy.rPattern = patt;
                                        currElement.elements.Add(rMatchCopy);
                                    }
                                }
                            }
                        }

                        if(isOrRule)
                        {
                            //Add last rule
                            rElements.Add(currElement);

                            r.elements = rElements;
                        }
                    }

                    rules.Add(r);
                }
            }

            return rules;
        }

        public Rule generateParseTree(List<Token> tokens, List<RuleDescriptor> rules)
        {
            Rule parseTree = new Rule();

            //if a rule match is found, then add it to the tree
            //Start by checking for program rule match
            //foreach(RuleDescriptor rule in rules)
            //{
            parseTree = rules[rules.Count-1].getMatchedRule(ref tokens);
            //}


            return parseTree;
        }
    }
}