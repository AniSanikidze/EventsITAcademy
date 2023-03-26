namespace EventsITAcademy.Application.CustomExceptions
{
    public class ItemNotFoundException : Exception
    {
        public string Code { get; private set; }
        public ItemNotFoundException(string message, string domainClassName) : base(message)
        {
            Code = domainClassName + "NotFound";
        }
    }
}
