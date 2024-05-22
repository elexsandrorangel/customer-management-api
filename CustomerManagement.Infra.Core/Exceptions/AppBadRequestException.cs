using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppBadRequestException : AppException
    {
        public AppBadRequestException() : base() { }

        public AppBadRequestException(string message) : base(message)
        {
        }

        // Without this constructor, deserialization will fail
        protected AppBadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
