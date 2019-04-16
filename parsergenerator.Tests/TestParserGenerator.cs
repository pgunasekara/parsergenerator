using System;
using System.Collections.Generic;
using Xunit;

namespace parsergenerator.Tests
{
    public class TestParserGenerator
    {
        private Parser parser;
        private Lexer lexer;
        private List<TokenDescriptor> tokenDescriptors;
        private List<RuleDescriptor> ruleDescriptors;
        private Dictionary<string, RuleDescriptor> ruleDictionary;
        public TestParserGenerator()
        {
            lexer = new Lexer();
            parser = new Parser();

            //Generate token descriptors
            tokenDescriptors = lexer.generateTokenDescriptors("tokens");

            var tokenDictionary = new Dictionary<string, TokenDescriptor>();
            foreach(var def in tokenDescriptors)
            {
                tokenDictionary.Add(def.type, def);
            }
            
            //Generate rule descriptors
            ruleDescriptors = parser.generateRuleDescriptors("rules", tokenDictionary);

            ruleDictionary = new Dictionary<string, RuleDescriptor>();
            foreach(var desc in ruleDescriptors)
            {
                ruleDictionary.Add(desc.name, desc);
            }
        }

        //Test working cases
        [Fact]
        public void TestFactorial()
        {
            string code = "program factorial;\nvar y, z: integer;\nprocedure fact(n: integer; var f: integer);\nbegin\n"
        		+ "if n = 0 then f := 1\nelse\nbegin fact(n - 1, f); f := f * n end\nend;\nbegin\nread(y);\nfact(y, z);\n"
        		+ "write(z)\nend";

            var tokens = lexer.start(code, tokenDescriptors);
            var parseTree = parser.generateParseTree(tokens, ruleDictionary);

            Assert.True(parseTree != null, "The factorial program should parse without errors");
        }

        [Fact]
        public void TestArithmetic()
        {
            string code = "program arithmetic;\nvar x, y, q, r: integer;\nprocedure QuotRem(x: integer; var r: integer);\n"
                    + "begin q := 0; r := x;\nwhile r >= y do\nbegin r := r - y; q := q + 1\nend\nend;\nbegin\nread(x);\nread(y);\n"
                    + "QuotRem(x, y, q, r);\nwrite(q); write(r); writeln()\nend";

            var tokens = lexer.start(code, tokenDescriptors);
            var parseTree = parser.generateParseTree(tokens, ruleDictionary);

            Assert.True(parseTree != null, "The factorial program should parse without errors");
        }

        //Test syntax errors

        [Fact]
        public void TestMissingProgramKeyword()
        {
            string code = "factorial;\nvar y, z: integer;\nprocedure fact(n: integer; var f: integer);\nbegin\n"
                + "if n = 0 then f := 1\nelse\nbegin fact(n - 1, f); f := f * n end\nend;\nbegin\nread(y);\nfact(y, z);\n"
                + "write(z)\nend";

            var tokens = lexer.start(code, tokenDescriptors);
            var parseTree = parser.generateParseTree(tokens, ruleDictionary);

            Assert.True(parseTree == null, "program keyword missing");
        }

        [Fact]
        public void TestParseProgramWithMissingSemicolon()
        {
            string code = "program factorial;\nvar y, z: integer;\nprocedure fact(n: integer; var f: integer);\nbegin\n"
                + "if n = 0 then f := 1\nelse\nbegin fact(n - 1, f); f := f * n end\nend;\nbegin\nread(y)\nfact(y, z);\n"
                + "write(z)\nend";

            var tokens = lexer.start(code, tokenDescriptors);
            var parseTree = parser.generateParseTree(tokens, ruleDictionary);

            Assert.True(parseTree == null, "missing semicolon");
        }

        [Fact]
        public void TestParseProgramWithIncorrectDeclarationsStatements()
        {
            string code = "prgoram factorial;\nbegin\nread(y);\nfact(y, z);\nwrite(z)\nend\n"
                + "var y, z: integer;\nprocedure fact(n: integer; var f: integer);\nbegin\n"
                + "if n = 0 then f := 1\nelse\nbegin fact(n - 1, f); f := f * n end\nend;";

            var tokens = lexer.start(code, tokenDescriptors);
            var parseTree = parser.generateParseTree(tokens, ruleDictionary);

            Assert.True(parseTree == null, "improper declarations");
        }
    }
}
