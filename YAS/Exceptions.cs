using System;

namespace YAS
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
        public AssemblerException()
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
}
