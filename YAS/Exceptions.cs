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
}
