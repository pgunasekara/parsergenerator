using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace parsergenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer l = new Lexer();
            /*List<TokenDescriptor> def = new List<TokenDescriptor>();

            def.Add(new TokenDescriptor("NUMBER", "[0-9]+"));
            def.Add(new TokenDescriptor("DIV", "div"));
            def.Add(new TokenDescriptor("MOD", "mod"));
            def.Add(new TokenDescriptor("AND", "and"));
            def.Add(new TokenDescriptor("OR", "or"));
            def.Add(new TokenDescriptor("OF", "of"));
            def.Add(new TokenDescriptor("THEN", "then"));
            def.Add(new TokenDescriptor("DO", "do"));
            def.Add(new TokenDescriptor("NOT", "not"));
            def.Add(new TokenDescriptor("END", "end"));
            def.Add(new TokenDescriptor("ELSE", "else"));
            def.Add(new TokenDescriptor("IF", "if"));
            def.Add(new TokenDescriptor("WHILE", "while"));
            def.Add(new TokenDescriptor("ARRAY", "array"));
            def.Add(new TokenDescriptor("RECORD", "record"));
            def.Add(new TokenDescriptor("CONST", "const"));
            def.Add(new TokenDescriptor("TYPE", "type"));
            def.Add(new TokenDescriptor("VAR", "var"));
            def.Add(new TokenDescriptor("PROCEDURE", "procedure"));
            def.Add(new TokenDescriptor("BEGIN", "begin"));
            def.Add(new TokenDescriptor("PROGRAM", "program"));
            def.Add(new TokenDescriptor("TIMES", Regex.Escape(@"*")));
            def.Add(new TokenDescriptor("PLUS", @"\+"));
            def.Add(new TokenDescriptor("MINUS", "-"));
            def.Add(new TokenDescriptor("EQ", "="));
            def.Add(new TokenDescriptor("LT", "<"));
            def.Add(new TokenDescriptor("LE", "<="));
            def.Add(new TokenDescriptor("NE", "<>"));
            def.Add(new TokenDescriptor("GT", ">"));
            def.Add(new TokenDescriptor("GE", ">="));
            def.Add(new TokenDescriptor("SEMICOLON", ";"));
            def.Add(new TokenDescriptor("COMMA", ","));
            def.Add(new TokenDescriptor("COLON", ":"));
            def.Add(new TokenDescriptor("BECOMES", "="));
            def.Add(new TokenDescriptor("PERIOD", @"\."));
            def.Add(new TokenDescriptor("LPAREN", @"\("));
            def.Add(new TokenDescriptor("RPAREN", @"\)"));
            def.Add(new TokenDescriptor("LBRAK", @"\["));
            def.Add(new TokenDescriptor("RBRAK", @"\]"));
            def.Add(new TokenDescriptor("IDENT", "[a-z]+"));*/


            //Create all possible tokens
            var tDef = l.generateTokenDescriptors("tokens.tk");
            Console.WriteLine(JsonConvert.SerializeObject(tDef));

            //Create all possible rules
            Parser p = new Parser();
            var rDesc = p.generateRuleDescriptors("rules3.rl", tDef);
            Console.WriteLine(JsonConvert.SerializeObject(rDesc, Formatting.None,
                        new JsonSerializerSettings()
                        { 
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));

            //Tokenize input
            var tokens = l.start("var x, y: integer", tDef);

            //Match input into rules to create abstract syntax tree
            p.generateParseTree(tokens, rDesc);
        }
    }
}