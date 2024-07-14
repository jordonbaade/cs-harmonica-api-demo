namespace HarmonicaApi
{
    // Define the NoSongsFoundException
    public class NoSongsFoundException : Exception
    {
        public NoSongsFoundException()
        {
            // Log or alert
        }

        public NoSongsFoundException(string message)
            : base(message)
        {
        }

        public NoSongsFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}