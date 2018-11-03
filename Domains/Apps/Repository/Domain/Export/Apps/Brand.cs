namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0a7ac589-946b-4d49-b7e0-7e0b9bc90111")]
    #endregion
    public partial class Brand : Enumeration
    {
        #region inherited properties

        public string Name { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("0DA86868-CD0A-4370-BD47-34790A22860F")]
        [AssociationId("0D13E3AF-A29D-4E96-9F0E-341DCF2B6AB6")]
        [RoleId("B36BF943-2FA0-4D67-B2D5-50C3E94BA79A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Model[] Models { get; set; }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}