using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppTimeoutException : AppException
    {
        public AppTimeoutException()
            : base("Resouce gets timeout")
        {
        }

        protected AppTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
