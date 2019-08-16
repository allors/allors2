namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("E30A6B10-069B-45CB-9D74-4DA9E77DE465")]
    #endregion
    public partial class Dimension : Enumeration
    {
        #region inherited properties

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("FF77D3D9-E425-4261-944C-1B0EC6C61B68")]
        [AssociationId("D4C56919-AC49-465D-95B6-390FBA1E1869")]
        [RoleId("30F12C0F-0B79-44F6-BCC0-A316A22CE221")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public IUnitOfMeasure UnitOfMeasure { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("0F9165C3-32FE-48C0-A62E-8277592314B9")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}