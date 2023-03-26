namespace EventsITAcademy.Application.CustomExceptions
{
    public class InvalidLoginException : Exception
    {
        public string Code { get; private set; }
        public InvalidLoginException(string message, string domainClassName) : base(message)
        {
            Code = "InvalidLogIn";
        }
    }
}
