namespace Allors.Domain
{
    public partial class Notifications
    {
        protected override void BaseSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantOwner(this.ObjectType, full);
            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
