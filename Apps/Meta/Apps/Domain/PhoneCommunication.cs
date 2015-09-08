namespace Allors.Meta
{
    public partial class PhoneCommunicationClass
	{
	    internal override void AppsExtend()
        {
            this.Caller.RoleType.IsRequired = true;
            this.Receiver.RoleType.IsRequired = true;
            this.IncomingCall.RoleType.IsRequired = true;
        }
	}
}