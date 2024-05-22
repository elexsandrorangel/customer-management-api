using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppConflictException : AppException
    {
        public AppConflictException()
            : base("Record already exists at the server")
        {
        }

        protected AppConflictException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
