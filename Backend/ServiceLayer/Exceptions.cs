using System;

namespace ServiceLayer.Exceptions
{
    public class PasswordPwnedException : Exception
    {
        public PasswordPwnedException() {}

        public PasswordPwnedException(string message) : base(message) {}
    }

    public class PasswordInvalidException : Exception
    {
        public PasswordInvalidException() { }

        public PasswordInvalidException(string message) : base(message) { }
    }

    public class InvalidDobException : Exception
    {
        public InvalidDobException() { }

        public InvalidDobException(string message) : base(message) { }
    }
}
