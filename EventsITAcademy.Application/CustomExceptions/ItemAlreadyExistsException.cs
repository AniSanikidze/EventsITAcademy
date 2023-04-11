namespace EventsITAcademy.Application.CustomExceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public string Code { get; private set; }
        public ItemAlreadyExistsException(string message, string code) : base(message)
        {
            Code = code;
        }
    }

}
