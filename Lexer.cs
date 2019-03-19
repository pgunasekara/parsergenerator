using System;
using System.Collections.Generic;

namespace parsergenerator
{
    public class Lexer
    {

        //String input;
        public Lexer() { }

        public void start(String input, List<TokenDescriptor> TokenDescriptors)
        {
            List<Token> tokens = new List<Token>();
            while(input != "")
            {
                int len = 0;
                while(input[len] == ' ') len++;

                input = input.Substring(len);

                Token t = new Token(Symbol.NONE, "");
                foreach(TokenDescriptor d in TokenDescriptors)
                {
                    string val = d.getMatch(input);
                    if(val.Length > t.val.Length)
                    {
                        t = new Token(d.type, d.match.Value);
                    }
                }

                if(t.type != Symbol.NONE) 
                {
                    tokens.Add(t);
                    input = input.Substring(t.val.Length);
                }
            }

            foreach(Token t in tokens)
            {
                Console.WriteLine(t.type + " : " + t.val);
            }
        }
    }
}