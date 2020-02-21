using System;
using System.Collections.Generic;
using System.Text;

namespace YAS
{
    /// <summary>
    /// Tokens represent atomic units of text in assembly and contains properties associated with the token.
    /// </summary>
    public class Token
    {
        private string _text;
        private Dictionary<EnumTokenProperties, Int64> Properties;

        public Token()
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
        }

        public Token(string str)
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
            _text = str;
        }

        /// <summary>
        /// Creates a token and automatically adds TokenType.
        /// </summary>
        public Token(Int64 TokenTypeVal, string str)
        {
            _text = str;
            Properties = new Dictionary<EnumTokenProperties, long>();
            AddProperty(EnumTokenProperties.TokenType, TokenTypeVal);
        }

        /// <summary>
        /// Creates a token and automatically adds TokenType and RealInstruction Properties.
        /// </summary>
        public Token(Int64 TokenTypeVal, Int64 RealInstructionVal, string str)
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
            _text = str;
            AddProperty(EnumTokenProperties.TokenType, TokenTypeVal);
            AddProperty(EnumTokenProperties.RealInstruction, RealInstructionVal);
        }

        public Token(Int64 TokenTypeVal, Int64 RealInstructionVal, int InstructionSize, string str)
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
            _text = str;
            AddProperty(EnumTokenProperties.TokenType, TokenTypeVal);
            AddProperty(EnumTokenProperties.RealInstruction, RealInstructionVal);
            AddProperty(EnumTokenProperties.OpSizeBytes, (Int64)InstructionSize);
        }

        public string Text
        {
            get { return _text; }
        }

        public bool AddProperty(EnumTokenProperties key, Int64 val)
        {
            if (!Properties.ContainsKey(key))
            {
                //Perhaps add some key/value checking here
                Properties.Add(key, val);
                return true;
            }
            //Invalid Property Exception
            return false;
        }

        public bool GetProperty(EnumTokenProperties key, out Int64 val)
        {
            val = -1;
            if (Properties.ContainsKey(key))
            {
                val = Properties[key];
                return true;
            }
            throw new NullReferenceException("Could not get property on token: " + _text);
            return false;
        }

        public bool GetProperty(EnumTokenProperties key, out int val)
        {
            val = -1;
            if (Properties.ContainsKey(key))
            {
                val = (int)Properties[key];
                return true;
            }
            throw new NullReferenceException("Could not get property on token: " + _text);
            return false;
        }

        public bool GetInstruction(out EnumInstructions val)
        {
            val = EnumInstructions.add;
            if (Properties.ContainsKey(EnumTokenProperties.RealInstruction))
            {
                val = (EnumInstructions)Properties[EnumTokenProperties.RealInstruction];
                return true;
            }
            throw new NullReferenceException("Could not get instruction on token: " + _text);
            return false;
        }

        public bool GetTokenType(out EnumTokenTypes val)
        {
            val = EnumTokenTypes.Unkown;
            if (Properties.ContainsKey(EnumTokenProperties.TokenType))
            {
                val = (EnumTokenTypes)Properties[EnumTokenProperties.TokenType];
                return true;
            }
            throw new NullReferenceException("Could not get token type on token: " + _text);
            return false;
        }

        public Token DeepCopy()
        {
            //Ensure a new instance of _text is created
            char[] s = _text.ToCharArray();
            Token temp = new Token(new string(s));
            foreach (var prop in Properties)
            {
                temp.AddProperty(prop.Key, prop.Value);
            }
            return temp;
        }
    }
}
