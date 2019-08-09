namespace Allors.Domain
{
    public partial class Notifications
    {
        protected override void CoreSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantOwner(this.ObjectType, full);
            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
