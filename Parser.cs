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

        public List<Rule> generateRuleDescriptors(string inputFile, List<TokenDescriptor> allTokens)
        {
            List<Rule> rules = new List<Rule>();

            using(StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    Rule r = new Rule();
                    int start = 0;
                    int end = 0;
                    //Skip any whitespace 
                    while(String.IsNullOrWhiteSpace(line))   { line = reader.ReadLine(); }
                    //Get name of rule
                    r.name = line; line = reader.ReadLine();

                    //Skip any whitespace 
                    while(String.IsNullOrWhiteSpace(line))   { line = reader.ReadLine(); }
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

                    //Store contents of rule
                    if(Regex.Match(line, @"^\s*:\s*$").Success)
                    {
                        //Parse the rest up to the semicolon
                        while((line = reader.ReadLine()) != null)
                        {
                            if(Regex.Match(line, @"^\s*;\s*$").Success)   break;

                            //parse rule
                            var parts = line.Split(' ');
                            List<IGrammar> rul;
                            foreach (string part in parts)
                            {
                                if(part.Equals("|"))
                                {
                                    continue;
                                }
                            }

                            r.tRules.Add(line);
                            r.rules.Add(null);
                        }
                    }

                    rules.Add(r);
                }
            }

            return rules;
        }

        public List<Rule> generateRules(List<RuleDescriptor> rd)
        {
            List<Rule> rules = new List<Rule>();

            return rules;
        }
    }
}