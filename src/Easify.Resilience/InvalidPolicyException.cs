using System;

namespace Easify.Resilience
{
    public sealed class InvalidPolicyException : Exception
    {
        public InvalidPolicyException(string message): base(message)
        {
        }
    }
}