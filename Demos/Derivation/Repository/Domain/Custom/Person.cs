namespace Allors.Repository
{
    using Attributes;

    public partial class Person : Deletable
    {
        #region inherited methods

        public void Delete()
        {
        }

        #endregion inherited methods

        #region Allors
        [Id("105CF367-F076-45F8-8E2A-2431BB2D65C7")]
        [AssociationId("{8CF12010-73B8-4106-B6AA-425E3D596102}")]
        [RoleId("F53C8D25-74E3-4715-9AA8-C64AEA23D3D8")]
        [Size(256)]
        #endregion
        [Workspace]
        public string DomainFullName { get; set; }

        #region Allors
        [Id("0DDC847A-713D-4A19-9C6F-E8FE9175301D")]
        [AssociationId("4B60BE54-5832-48A9-9A48-6A494C7C0ABA")]
        [RoleId("F2C08EDD-56DF-4058-AF1E-D956AFC1D617")]
        [Size(256)]
        #endregion
        [Workspace]

        public string DomainGreeting { get; set; }
    }
}
