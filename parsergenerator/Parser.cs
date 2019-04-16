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

        /// <summary>
        /// Takes an input file containing a list of rules create by a user to generate a list of Rule descripters.
        /// The rule descriptors will tell the parser generator how to behave when parsing each line within the
        /// source file to be parsed by the parser generator.
        /// The function will loop through each line of the file, starting with the first non empty line being the
        /// name of the rule, then a colon, then the rule itself split into as many lines as necessary, then a 
        /// semicolon to indicate the end of the rule. This will repeat for all rules.
        /// </summary>
        /// <param name="inputFile">A file containing all the rules for the given language</param>
        /// <param name="tokens">A dictionary of all tokens to look up whether a token in the input file is valid</param>
        /// <returns>A set of rule descriptors (as many as there are rules within the input file)</returns>
        public List<RuleDescriptor> generateRuleDescriptors(string inputFile, Dictionary<string, TokenDescriptor> tokens)
        {
            List<RuleDescriptor> rules = new List<RuleDescriptor>();

            using(StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    //The descriptor for the current rule being parsed
                    RuleDescriptor r = new RuleDescriptor();

                    //Skip any whitespace 
                    while(String.IsNullOrWhiteSpace(line))   { line = reader.ReadLine(); }
                    //Get name of rule
                    r.name = line; line = reader.ReadLine();

                    //Skip any whitespace 
                    while(String.IsNullOrWhiteSpace(line))   { line = reader.ReadLine(); }

                    //Start parsing contents of the rule once a colon is encountered
                    if(Regex.Match(line, @"^\s*:\s*$").Success)
                    {
                        List<IGrammar> rElements = new List<IGrammar>(); //A set of elements under this rule, can be tokens or other rules
                        bool isOrRule = false; //indicates whether the rule is split into multiple parts using the | operator
                        bool initRun = true; //indicates that it's the first iteration for the current rule
                        int i = 0; //ised to keep track of OR rules, where we create a new rule for each OR-ed rule
                        
                        RuleDescriptor currElement = null;

                        //Parse the rest up to the semicolon
                        while((line = reader.ReadLine()) != null)
                        {
                            //If we're at the end of the rule, break out of the current loop to move onto the next rule
                            if(Regex.Match(line, @"^\s*;\s*$").Success) { break; }

                            if(initRun)
                            {
                                //Check if it's an OR rule
                                if(line.IndexOf('|') == -1)
                                {
                                    //If this is not an OR rule, we don't want any subrules
                                    isOrRule = false;
                                    currElement = r;
                                }
                                else
                                {
                                    //If this is an OR rule, then create a list of subrules, and postfix their names with _i
                                    isOrRule = true;
                                    currElement = new RuleDescriptor();
                                    currElement.name = r.name + "_" + i++;
                                }
                                initRun = false;
                            }

                            line = line.Trim(); //strip any extra spaces at the begining or end of the line

                            var parts = line.Split(' ');
                            foreach (string part in parts)
                            {
                                Pattern patt = Pattern.NONE;
                                //Check if current part is an OR
                                if(part.Equals("|"))
                                {
                                    //Set the appropriate OR rule flag, add the previous rule into the current rule, and then start a new rule
                                    r.rulePattern = RulePattern.ORRULE;
                                    rElements.Add(currElement);
                                    rules.Add(currElement);
                                    currElement = new RuleDescriptor();
                                    currElement.name = r.name + "_" + i++;
                                    continue;
                                }

                                string pCopy = part; //cloned the string to allow for it to be modified within the foreach loop

                                //Check for pattern modifier
                                //If a modifier is found, we want to associate this rule with that modifier by setting the rPattern flag
                                if(pCopy.EndsWith('+'))        { patt = Pattern.ONEORMORE; }
                                else if(pCopy.EndsWith('*'))   { patt = Pattern.ZEROORMORE; }
                                else if(pCopy.EndsWith('?'))   { patt = Pattern.OPTIONAL; }

                                if(patt != Pattern.NONE)
                                {
                                    //Strip the pattern modifier off the string because it has already been read
                                    pCopy = pCopy.Substring(0, pCopy.Length-1);
                                }

                                //Check if the current part of the string is a token
                                TokenDescriptor tMatch;
                                tokens.TryGetValue(pCopy, out tMatch);                                
                                if(tMatch != null)
                                {
                                    //token matched
                                    var tMatchCopy = tMatch.copy();
                                    tMatchCopy.rPattern = patt;
                                    currElement.elements.Add(tMatchCopy);
                                }
                                else
                                {
                                    //If a token isn't matched, assume that it is a rule (even if it hasn't been defined yet, it might be defined in the future)
                                    var nRule = new RuleDescriptor(pCopy, null, patt, RulePattern.NONE);
                                    currElement.elements.Add(nRule);
                                }
                            }
                        }

                        if(isOrRule)
                        {
                            //If this is an OR rule, then we want to add the last element that was created before the loop ended
                            rElements.Add(currElement);
                            rules.Add(currElement);

                            r.elements = rElements;
                        }
                    }

                    rules.Add(r); //Add each new rule to the rule list until all rules are parsed
                }
            }

            return rules;
        }

        /// <summary>
        /// Generates a parse tree based on a given token stream. The start rule can be specified if the user does not wish
        /// to start with the 'program' keyword. The parse tree will be a single rule with subrules under it. This rule can
        /// then be pretty printed in JSON to show the final parse tree. RuleDescriptor.getMatchValue() will be called recursively
        /// to build a final rule to be returned once all the tokens are consumed.
        /// </summary>
        /// <param name="tokens">The input token stream from the user</param>
        /// <param name="rules">A dictionary of all rules that were parsed from the rules file</param>
        /// <param name="startRule">A starting rule if the user does not wish to start from the 'program' rule</param>
        /// <returns>A parse tree in the form of a rule. If improper syntax is found, returns NULL.</returns>
        public Rule generateParseTree(List<Token> tokens, Dictionary<string, RuleDescriptor> rules, string startRule)
        {
            Rule parseTree = new Rule();

            //if a rule match is found, then add it to the tree
            //Start by checking for program rule match
            parseTree = rules[startRule].getMatchedRule(ref tokens, rules);
            
            //We only want to name the tree if the final returned rule is not null
            if(parseTree != null)
            {
                parseTree.name = rules[startRule].name;
            }

            return parseTree;
        }

        /// <summary>
        /// Overloaded version of generateParseTree(List<Token>, Dictionary<string, RuleDescriptor>, string) allowing the
        /// user to have the program automatically start by default at the 'program' rule.
        /// </summary>
        /// <param name="tokens">The input token stream from the user</param>
        /// <param name="rules">A dictionary of all rules that were parsed from the rules file</param>
        /// <returns>A parse tree in the form of a rule. If improper syntax is found, returns NULL.</returns>
        public Rule generateParseTree(List<Token> tokens, Dictionary<string, RuleDescriptor> rules)
        {
            return generateParseTree(tokens, rules, "program");
        }
    }
}