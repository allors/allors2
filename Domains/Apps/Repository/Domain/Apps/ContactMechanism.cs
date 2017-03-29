namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("b033f9c9-c799-485c-a199-914a9e9119d9")]
    #endregion
	public partial interface ContactMechanism : Auditable, Deletable 
    {


        #region Allors
        [Id("3c4ab373-8ff4-44ef-a97d-d8a27513f69c")]
        [AssociationId("0edba6dd-606a-4751-bd86-b98822d4b1f2")]
        [RoleId("ab370eaa-ede5-4bee-a9ab-6f6e52889f77")]
        #endregion
        [Size(256)]

        string Description { get; set; }


        #region Allors
        [Id("e2bd1f50-f891-4e3f-bac0-e9582b89e64c")]
        [AssociationId("9bb26e51-9acf-4fa1-8a24-3d0b1b2f7103")]
        [RoleId("d85aba30-7aee-4bc4-9026-26ba4f355d70")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        ContactMechanism[] FollowTo { get; set; }

    }
}