using System;
using System.Text.RegularExpressions;

namespace parsergenerator
{
    public class Token : IGrammar
    {
        public String type { get; set; }
        public String val { get; set; }
        public Pattern rPattern { get; set; }
        
        public Token(String type, String val)
        {
            this.type = type;
            this.val = val;
            rPattern = Pattern.NONE;
        }

        public Token copy()
        {
            return new Token(this.type, this.val);
        }
    }
}