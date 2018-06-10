using System;

namespace ADarkBlazor.Exceptions
{
    public class ResourceException : Exception
    {
        public ResourceException() { }
        public ResourceException(string message) : base(message) { }
    }

    public class BuilderException : Exception
    {
        public BuilderException() { }
        public BuilderException(string message) : base(message) { }
    }
}
