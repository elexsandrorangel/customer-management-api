using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppNotFoundException : AppException
    {
        public AppNotFoundException()
            : base("Resource not found")
        {
        }

        public AppNotFoundException(string message) : base(message)
        {
        }

        protected AppNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
