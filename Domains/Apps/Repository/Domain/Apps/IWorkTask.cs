namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("19616917-9BD3-429F-95AC-9059EF14ECA0")]
    #endregion
    public partial interface IWorkTask : IWorkEffort
    {
        #region Allors
        [Id("A1070CB5-3492-408C-959A-1C0785C774A0")]
        [AssociationId("16CCFA38-D34E-47E8-B73C-4E57FEF7A0BC")]
        [RoleId("4FD10D7B-1006-4D40-A53F-404B4DA2C379")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        bool SendNotification { get; set; }

        #region Allors
        [Id("55229180-203E-4743-B41B-DA4B4FC1B079")]
        [AssociationId("3375EB99-EA3A-4F91-BA7F-ABE1D63B847C")]
        [RoleId("A5752F2D-5B5A-42A8-9CB8-C11AF8C48880")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        bool SendReminder { get; set; }

        #region Allors
        [Id("413541ED-963E-4036-9347-047456F211E6")]
        [AssociationId("DF6788A3-4D2A-44C6-9213-B14B4FCD708F")]
        [RoleId("8186D56F-CEF3-4D94-A34B-A111454F2BB5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        DateTime RemindAt { get; set; }
    }
}