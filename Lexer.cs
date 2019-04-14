using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;

namespace parsergenerator
{
    public class Lexer
    {
        public Lexer() { }

        public List<Token> start(String input, List<TokenDescriptor> TokenDescriptors)
        {
            List<Token> tokens = new List<Token>();
            while(input != "")
            {
                int len = 0;
                while(input[len] == ' ' || input[len] == '\n') len++;

                input = input.Substring(len);

                Token t = new Token("NONE", "");
                foreach(TokenDescriptor d in TokenDescriptors)
                {
                    string val = d.getMatch(input);
                    if(val.Length > t.val.Length)
                    {
                        t = new Token(d.type, d.match.Value);
                    }
                }

                if(t.type != "NONE") 
                {
                    tokens.Add(t);
                    input = input.Substring(t.val.Length);
                }
            }

            foreach(Token t in tokens)
            {
                Console.WriteLine(t.type + " : " + t.val);
            }

            return tokens;
        }

        public List<TokenDescriptor> generateTokenDescriptors(string inputFile)
        {
            List<TokenDescriptor> tkd = new List<TokenDescriptor>();

            using(StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    Regex rx = new Regex(@"\s*:\s*");
                    string[] splitLine = rx.Split(line, 2);
                    tkd.Add(new TokenDescriptor()
                    {
                        type = splitLine[0],
                        pattern = splitLine[1].Substring(1, splitLine[1].Length - 2),
                    });
                }
            }

            return tkd;
        }
    
        public Rule matchRules(List<Token> tokens, List<IGrammar> rules)
        {
            Rule parseTree = new Rule();

            while(tokens.Count > 0)
            {
                int consume = 0;
                int longestRule = 0;
                //find a match with the first token, and then keep going
                foreach (IGrammar rule in rules)
                {
                    
                }

            }

            return parseTree;
        }
    }
}