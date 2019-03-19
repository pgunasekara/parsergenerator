using System;
using System.Text.RegularExpressions;

namespace parsergenerator
{
    public class Token : IGrammar
    {
        public Symbol type { get; set; }
        public String val { get; set; }
        
        public Token(Symbol type, String val)
        {
            this.type = type;
            this.val = val;
        }
    }
}