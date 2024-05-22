using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppUnauthorizedException : AppException
    {
        public AppUnauthorizedException() { }

        public AppUnauthorizedException(string message) : base(message) { }

        protected AppUnauthorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
