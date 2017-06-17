namespace Allors.Repository
{
    using System;
    using Attributes;
    
    #region Allors
    [Id("6C726DED-C081-46D7-8DCF-F0A376943531")]
    #endregion
    public partial interface Auditable : AccessControlledObject
    {

        #region Allors
        [Id("4BD26F4D-E85B-415A-B956-3FCBE15D4F58")]
        [AssociationId("F70A0D62-8D3F-4EAE-8FBD-E450468300E5")]
        [RoleId("8EA99925-57E5-4D4E-A6B8-087337038F2F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        User CreatedBy { get; set; }

        #region Allors
        [Id("471CCC05-A48D-47A0-934B-0DD4F8E40C65")]
        [AssociationId("05F66AFA-C12A-46E8-B984-2EF96984281E")]
        [RoleId("4A41F523-F910-4E70-B69B-040E2494A9D4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        User LastModifiedBy { get; set; }

        #region Allors
        [Id("C1BA5015-21DD-49A9-AE0F-E70F4035CCA6")]
        [AssociationId("06A58270-F1C3-4C64-A268-FCEA208EBC29")]
        [RoleId("EC78160B-49E1-4F92-9FA3-79A0CE7EA645")]
        #endregion
        [Workspace]
        DateTime CreationDate { get; set; }


        #region Allors
        [Id("94EB2712-25E1-415B-9657-2DFD460B7969")]
        [AssociationId("994394D7-A4CD-4540-BD26-7EFF0A28EDB1")]
        [RoleId("316D738B-FDB7-4849-B74C-2D3B97C4C58E")]
        #endregion
        [Workspace]
        DateTime LastModifiedDate { get; set; }
    }
}
