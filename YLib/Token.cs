using System;
using System.Collections.Generic;
using System.Text;

namespace YLib
{
    /// <summary>
    /// Tokens represent atomic units of text in assembly and contains properties associated with the token.
    /// </summary>
    public class Token
    {
        private string _text;
        private Dictionary<EnumTokenProperties, Int64> Properties;

        public EnumTokenTypes TokenType
        {
            //TODO Lets do something!
            get
            {
                EnumTokenTypes var;
                if (GetTokenType(out var))
                    return var;
                return EnumTokenTypes.Unkown;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Token other = (Token)obj;
                bool doesMatch = true;
                foreach (var key in this.Properties.Keys)
                {
                    if (!other.Properties.ContainsKey(key))
                    {
                        doesMatch = false;
                        break;
                    }
                    else
                    {
                        if (other.Properties[key] != this.Properties[key])
                        {
                            doesMatch = false;
                            break;
                        }
                    }
                }

                // Need to add a test that other does not have any keys that are in this object.

                return doesMatch && this.Text == other.Text;
            }
        }

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

        public Token(Int64 TokenTypeVal, Int64 RealInstructionVal, Int64 InstructionSize, string str)
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
            _text = str;
            AddProperty(EnumTokenProperties.TokenType, TokenTypeVal);
            AddProperty(EnumTokenProperties.RealInstruction, RealInstructionVal);
            AddProperty(EnumTokenProperties.InstructionSize, (Int64)InstructionSize);
        }

        public string Text
        {
            get { return _text; }
            //set { _text = value; }
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
            //throw new TokenAccessException(this, "Failed on accessing property: " + Enum.GetName(typeof(EnumTokenProperties), key));
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
            //throw new TokenAccessException(this, "Failed on accessing property: " + Enum.GetName(typeof(EnumTokenProperties), key));
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
            throw new TokenAccessException(this, "Failed on accessing Instruction property.");
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
            throw new TokenAccessException(this, "Failed on accessing TokenType property.");
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

        /// <summary>
        /// Gives a friendly output string of the token for debugging.
        /// </summary>
        /// <returns></returns>
        public string TokenInfoString()
        {
            string returnString = "";
            foreach (var val in Properties)
            {
                switch (val.Key)
                {
                    case (EnumTokenProperties.RealInstruction):
                        returnString += "Instruction:" + Enum.GetName(typeof(EnumInstructions), val.Value) + " ";
                        break;
                    case (EnumTokenProperties.RegisterNumber):
                        returnString += "Register:" + Enum.GetName(typeof(EnumRegisters), val.Value) + " ";
                        break;
                    case (EnumTokenProperties.TokenType):
                        returnString += "Type:" + Enum.GetName(typeof(EnumTokenTypes), val.Value) + " ";
                        break;
                    case (EnumTokenProperties.ImmediateValue):
                        returnString += "Literal:" + val.Value + " ";
                        break;
                    case (EnumTokenProperties.LabelId):
                        break;
                    case (EnumTokenProperties.OpSizeBytes):
                        break;
                }
            }

            return returnString + Environment.NewLine;
        }
    }
}
