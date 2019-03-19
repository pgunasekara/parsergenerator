using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace parsergenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer l = new Lexer();
            List<TokenDescriptor> def = new List<TokenDescriptor>();

            def.Add(new TokenDescriptor(Symbol.NUMBER, "[0-9]+"));
            def.Add(new TokenDescriptor(Symbol.DIV, "div"));
            def.Add(new TokenDescriptor(Symbol.MOD, "mod"));
            def.Add(new TokenDescriptor(Symbol.AND, "and"));
            def.Add(new TokenDescriptor(Symbol.OR, "or"));
            def.Add(new TokenDescriptor(Symbol.OF, "of"));
            def.Add(new TokenDescriptor(Symbol.THEN, "then"));
            def.Add(new TokenDescriptor(Symbol.DO, "do"));
            def.Add(new TokenDescriptor(Symbol.NOT, "not"));
            def.Add(new TokenDescriptor(Symbol.END, "end"));
            def.Add(new TokenDescriptor(Symbol.ELSE, "else"));
            def.Add(new TokenDescriptor(Symbol.IF, "if"));
            def.Add(new TokenDescriptor(Symbol.WHILE, "while"));
            def.Add(new TokenDescriptor(Symbol.ARRAY, "array"));
            def.Add(new TokenDescriptor(Symbol.RECORD, "record"));
            def.Add(new TokenDescriptor(Symbol.CONST, "const"));
            def.Add(new TokenDescriptor(Symbol.TYPE, "type"));
            def.Add(new TokenDescriptor(Symbol.VAR, "var"));
            def.Add(new TokenDescriptor(Symbol.PROCEDURE, "procedure"));
            def.Add(new TokenDescriptor(Symbol.BEGIN, "begin"));
            def.Add(new TokenDescriptor(Symbol.PROGRAM, "program"));
            def.Add(new TokenDescriptor(Symbol.TIMES, Regex.Escape(@"*")));
            def.Add(new TokenDescriptor(Symbol.PLUS, @"\+"));
            def.Add(new TokenDescriptor(Symbol.MINUS, "-"));
            def.Add(new TokenDescriptor(Symbol.EQ, "="));
            def.Add(new TokenDescriptor(Symbol.LT, "<"));
            def.Add(new TokenDescriptor(Symbol.LE, "<="));
            def.Add(new TokenDescriptor(Symbol.NE, "<>"));
            def.Add(new TokenDescriptor(Symbol.GT, ">"));
            def.Add(new TokenDescriptor(Symbol.GE, ">="));
            def.Add(new TokenDescriptor(Symbol.SEMICOLON, ";"));
            def.Add(new TokenDescriptor(Symbol.COMMA, ","));
            def.Add(new TokenDescriptor(Symbol.COLON, ":"));
            def.Add(new TokenDescriptor(Symbol.BECOMES, "="));
            def.Add(new TokenDescriptor(Symbol.PERIOD, @"\."));
            def.Add(new TokenDescriptor(Symbol.LPAREN, @"\("));
            def.Add(new TokenDescriptor(Symbol.RPAREN, @"\)"));
            def.Add(new TokenDescriptor(Symbol.LBRAK, @"\["));
            def.Add(new TokenDescriptor(Symbol.RBRAK, @"\]"));
            def.Add(new TokenDescriptor(Symbol.IDENT, "[a-z]+"));


            // l.start("begin 0123 begin begin 2222 end begin 22 end", def);

            l.start("2 + 2 * abcd begin", def);
        }
    }
}