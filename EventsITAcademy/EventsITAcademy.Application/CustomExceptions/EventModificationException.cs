namespace EventsITAcademy.Application.CustomExceptions
{
    public class EventModificationException : Exception
    {
        public string Code { get; private set; }
        public EventModificationException(string message, string domainClassName) : base(message)
        {
            Code = "EventModificationForbidden";
        }
    }
}
