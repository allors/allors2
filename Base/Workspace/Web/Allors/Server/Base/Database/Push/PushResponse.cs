namespace Allors.Web.Database
{
    public class PushResponse : ErrorResponse
    {
        public PushResponseNewObject[] NewObjects { get; set; }
    }
}