namespace Allors.Server
{
    public class PushRequest
    {
        public PushRequestNewObject[] NewObjects { get; set; }

        public PushRequestObject[] Objects { get; set; }
    }
}