using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace parsergenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = "";
            bool testMode = false;
            int n = 1;
            if(args.Length == 0 || args.Length > 2)
            {
                code = "program factorial;\nvar y, z: integer;\nprocedure fact(n: integer; var f: integer);\nbegin\n"
        		+ "if n = 0 then f := 1\nelse\nbegin fact(n - 1, f); f := f * n end\nend;\nbegin\nread(y);\nfact(y, z);\n"
        		+ "write(z)\nend";
            }
            else if (args.Length == 1)
            {
                try
                {
                //read the input file
                    code = File.ReadAllText(args[0]);
                } 
                catch(Exception e)
                {
                    Console.WriteLine("Unable to read input file");
                    Console.WriteLine(e.ToString());
                    return;
                }
            }
            else if (args.Length == 2)
            {
                testMode = true;
                if(!Int32.TryParse(args[1], out n))
                {
                    Console.WriteLine("Invalid argument given for number of iterations");
                    return;
                }
            }

            Lexer l = new Lexer();

            //Create all possible tokens
            var tokenDefinitions = l.generateTokenDescriptors("tokens");
            
            var tokenDictionary = new Dictionary<string, TokenDescriptor>(); //put tokens into dictionary for quick lookup
            foreach(var def in tokenDefinitions)
            {
                tokenDictionary.Add(def.type, def);
            }

            //Create all possible rules
            Parser p = new Parser();
            var ruleDefinitions = p.generateRuleDescriptors("rules", tokenDictionary);
            var ruleDictionary = new Dictionary<string, RuleDescriptor>(); //put rule descriptors into dictionary for quick lookup

            foreach(var desc in ruleDefinitions)
            {
                ruleDictionary.Add(desc.name, desc);
            }
            
            var tokens = l.start(code, tokenDefinitions);

            if(testMode)
            {
                runTest(n, tokenDefinitions, ruleDictionary, p, l);
            }
            else
            {
                //Match input into rules to create abstract syntax tree
                var parseTree = p.generateParseTree(tokens, ruleDictionary);

                Console.WriteLine(JsonConvert.SerializeObject(parseTree, Formatting.None,
                            new JsonSerializerSettings()
                            { 
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));
            }
        }

        /// <summary>
        /// Benchmarking function used to run 4 sample programs through the parser and print out a timer
        /// </summary>
        /// <param name="n">Number of iterations to run the test</param>
        /// <param name="tokenDefinitions">A list of token descriptors to be used to match with the text</param>
        /// <param name="ruleDictionary">A dictionary of all rules</param>
        /// <param name="p">The parser to be used to parse the text</param>
        /// <param name="l">A lexer to tokenize the input</param>
        public static void runTest(int n, List<TokenDescriptor> tokenDefinitions, Dictionary<string, RuleDescriptor> ruleDictionary, Parser p, Lexer l)
        {
            // int n = Int32.Parse(args[1]);
            //Sample testing code strings
            string p1 = "program p;\nconst N = 10;\ntype T = array [1 .. N] of integer;\nvar x: T;\nvar y: boolean;\n"
                + "var z: record f: integer; g: boolean end;\nprocedure q(var v: boolean);\nvar z: boolean;\n"
                + "begin z := false end;\nbegin y := true\nend\n";
            string p2 = "program p;\nvar x: integer;\nvar y: array [1..10] of integer;\nbegin\nread(x);\nif x > 0 then\n"
                + "while y[x] < 7 do\nx := x + 1\nelse write(x);\nwriteln()\nend\n";
            string p3 = "program p;\nvar a: integer;\nvar b: integer;\nvar x: record f, g: integer end;\nbegin"
                + "a := 7;\nb := 9;\nx.g := 3;\nx.f := 5\nend\n";
            string p4 = "program p;\nvar i: integer;\nvar x: array [1..10] of integer;\nbegin\nx[5] := 3;\nx[i] := 5;\n"
                + "x[i + 7] := i + 9\nend";

            double total = 0;
            TimeSpan start, end;
            List<Token> tokens;

            for (int i = 0; i < n; i++){
                start = DateTime.UtcNow - new DateTime(1970, 1, 1);
                tokens = l.start(p1, tokenDefinitions);
                p.generateParseTree(tokens, ruleDictionary);
                tokens = l.start(p2, tokenDefinitions);
                p.generateParseTree(tokens, ruleDictionary);
                tokens = l.start(p3, tokenDefinitions);
                p.generateParseTree(tokens, ruleDictionary);
                tokens = l.start(p4, tokenDefinitions);
                p.generateParseTree(tokens, ruleDictionary);
                end = DateTime.UtcNow - new DateTime(1970, 1, 1);
                total += (end - start).TotalSeconds;
            }

            Console.WriteLine($"Average Runtime for running test programs {n}-times: {total/ n}");
        }
    }
}