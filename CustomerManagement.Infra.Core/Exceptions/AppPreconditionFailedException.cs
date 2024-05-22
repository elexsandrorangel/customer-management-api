using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppPreconditionFailedException : AppException
    {
        public AppPreconditionFailedException() { }
        
        public AppPreconditionFailedException(string message) : base(message) { }

        protected AppPreconditionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
