using System.Runtime.Serialization;

namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class AppUnsupportedMediaTypeException : AppException
    {
        public AppUnsupportedMediaTypeException()
            : base("Unsupported media type")
        {
        }

        protected AppUnsupportedMediaTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
