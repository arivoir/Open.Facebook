using System;

namespace Open.Facebook
{
    public class FacebookException : Exception
    {
        public FacebookException(Exception exc)
            : base(exc.Message, exc)
        {
            Error = new Error();
        }

        public FacebookException(Error error)
            : base(error.Message)
        {
            Error = error;
        }

        public Error Error { get; private set; }
    }
}
