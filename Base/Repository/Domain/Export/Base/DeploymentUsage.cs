namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("ca0f0654-3974-4e5e-a57e-593216c05e16")]
    #endregion
    public partial interface DeploymentUsage : Commentable, Period, Object
    {


        #region Allors
        [Id("50c6bc05-83ff-4d40-b476-51418355eb0c")]
        [AssociationId("e8aa74ab-d70a-43f4-9cac-de0160e3f257")]
        [RoleId("cc27af60-5ddd-4cce-bcc1-d68b3d5c6ab4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        TimeFrequency Frequency { get; set; }

    }
}
