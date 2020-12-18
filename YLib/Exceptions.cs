using System;
using YLib;

namespace YLib
{
    public class FoundUnexpectedToken : Exception
    {
        public Token token1;
        public FoundUnexpectedToken()
        {
        }

        public FoundUnexpectedToken(Token tkn)
        {
            token1 = tkn;
        }

        public FoundUnexpectedToken(Token tkn, string message) : base(message)
        {

        }

        public FoundUnexpectedToken(string message) : base(message)
        {

        }

        public FoundUnexpectedToken(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public enum EnumAssemblerStages
    {
        Lexer = 0,
        Utility,
        Parser,
        TokenFile,
        BinaryWriter
    }

    public class AssemblerException : Exception
    {
        public EnumAssemblerStages _stage;
        public AssemblerException() : base()
        {
        }

        public AssemblerException(EnumAssemblerStages stage)
        {
            _stage = stage;
        }

        public AssemblerException(EnumAssemblerStages stage, string message) : base(message)
        {
            _stage = stage;
        }
    }



    /// <summary>
    /// Thrown when an expected element is not contained in a token.
    /// </summary>
    public class TokenAccessException : Exception
    {
        public Token _Tkn;
        public EnumTokenProperties _Property;
        public TokenAccessException() : base()
        {

        }

        public TokenAccessException(Token token) : base()
        {
            _Tkn = token.DeepCopy();
        }

        public TokenAccessException(Token token, string str) : base(str)
        {
            _Tkn = token.DeepCopy();
        }

    }
}
