namespace Allors.Domain
{
    public partial class AutomatedAgent
    {
            public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            this.PartyName = this.Name;
        }
    }
}