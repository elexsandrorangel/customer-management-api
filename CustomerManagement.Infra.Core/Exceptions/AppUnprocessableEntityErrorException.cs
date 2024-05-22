using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppUnprocessableEntityErrorException : AppException
    {
        public AppUnprocessableEntityErrorException() { }
        public AppUnprocessableEntityErrorException(string message) : base(message) { }

        protected AppUnprocessableEntityErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
