namespace Allors.Server
{
    public class PushResponse : ErrorResponse
    {
        public PushResponseNewObject[] NewObjects { get; set; }
    }
}