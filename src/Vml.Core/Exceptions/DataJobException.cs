namespace Vml.Core.Exceptions
{
    public class DataJobException : Exception
    {
        public DataJobException()
        {

        }

        public DataJobException(string message) : base(message)
        {

        }

        public DataJobException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    public class LinkException : Exception
    {
        public LinkException()
        {

        }

        public LinkException(string message) : base(message)
        {

        }

        public LinkException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
