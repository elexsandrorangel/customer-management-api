using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppForbiddenException : AppException
    {
        public AppForbiddenException()
            : base("Sorry! This resource has restrict access.")
        {
        }

        protected AppForbiddenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
