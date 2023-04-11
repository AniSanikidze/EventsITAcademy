namespace EventsITAcademy.Application.CustomExceptions
{
    public class NoTicketsLeftException : Exception
    {
        public string Code { get; private set; }
        public NoTicketsLeftException(string message) : base(message)
        {
            Code = "NotTicketsLeft";
        }
    }
}
