using System;

namespace ServiceLayer.Exceptions
{
    public class PasswordPwnedException : Exception
    {
        public PasswordPwnedException() {}

        public PasswordPwnedException(string message) : base(message) {}
    }
}
