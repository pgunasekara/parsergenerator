using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;

namespace parsergenerator
{
    /// <summary>
    /// The Lexer is responsible for creating a list of Token descriptors from a tokens file, and tokenizing an input source
    /// to be used by the parser.
    /// </summary>
    public class Lexer
    {
        public Lexer() { }

        /// <summary>
        /// Parse input source code and create tokens out of the given source code. Use the token descriptors
        /// to do regular expression matches on each keyword and picks the longest match.
        /// </summary>
        /// <param name="input">An input file to turn into a token stream</param>
        /// <param name="TokenDescriptors">A list of token descriptors to match with the input string</param>
        /// <returns>A final token stream in a list object in the order that they appear in the source code</returns>
        public List<Token> start(String input, List<TokenDescriptor> TokenDescriptors)
        {
            List<Token> tokens = new List<Token>(); //The final list of tokens to be returned
            //Loop until the entire input string is empty
            while(input != "")
            {
                int len = 0;
                //Remove any additional unwanted spaces at the begining of the input
                while(input[len] == ' ' || input[len] == '\n' || input[len] == '\r') { len++; if(len >= input.Length) { break; }}
                input = input.Substring(len);

                //Go through all token descriptors and find the longest possible match and assign it to t
                Token t = new Token("NONE", "");
                foreach(TokenDescriptor d in TokenDescriptors)
                {
                    string val = d.getMatch(input);
                    if(val.Length > t.val.Length)
                    {
                        //Current longest match found is assigned to t
                        t = new Token(d.type, d.match.Value);
                    }
                }

                //As long as some value was assigned to the token in the above loop, add it to the token stream
                if(t.type != "NONE") 
                {
                    tokens.Add(t);
                    input = input.Substring(t.val.Length); //Remove last matched keyword using substring
                }
            }

            return tokens;
        }

        /// <summary>
        /// Generate a list of token descriptors from the inputted token file. Each token will have a name and a regular expression
        /// associated with it.
        /// The file is of the format: TOKENNAME : '[regular expression]' {\n TOKENNAME : '[regular expression]'}
        /// </summary>
        /// <param name="inputFile">The file containing all tokens and their respective regular expressions</param>
        /// <returns>A list of token descriptors generated from the input file</returns>
        public List<TokenDescriptor> generateTokenDescriptors(string inputFile)
        {
            List<TokenDescriptor> tkd = new List<TokenDescriptor>(); //Final list of token descriptors to be returned

            using(StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    Regex rx = new Regex(@"\s*:\s*");
                    string[] splitLine = rx.Split(line, 2); //Split string in two parts by the colon, 0 = token name, 1 = regex for token
                    tkd.Add(new TokenDescriptor()
                    {
                        type = splitLine[0],
                        pattern = splitLine[1].Substring(1, splitLine[1].Length - 2),
                    });
                }
            }

            return tkd;
        }
    }
}