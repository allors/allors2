namespace Allors.Domain
{
    public partial class AutomatedAgent
    {
            public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            this.PartyName = this.Name;
        }
    }
}