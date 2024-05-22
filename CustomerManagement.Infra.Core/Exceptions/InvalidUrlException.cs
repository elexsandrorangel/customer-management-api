namespace CustomerManagement.Infra.Core.Exceptions
{
    [Serializable]
    public class InvalidUrlException : AppException
    {
        public InvalidUrlException() { }

        public InvalidUrlException(string message) : base(message) { }
    }
}
