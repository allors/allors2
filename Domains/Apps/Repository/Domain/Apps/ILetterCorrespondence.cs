namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("ABFB354F-F7CD-4346-9877-35AA487F3E00")]
    #endregion
    public partial interface ILetterCorrespondence : ICommunicationEvent 
    {
        #region Allors
        [Id("3e0f1be5-0685-48d6-922f-6e971110b414")]
        [AssociationId("d063c86e-bbee-41b9-9823-10e96c69c5a0")]
        [RoleId("14ca37a9-7ce4-4d2a-b7ba-1a43bccc1664")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        PostalAddress[] PostalAddresses { get; set; }

        #region Allors
        [Id("e8fd2c39-bcb7-4914-8cd3-6dcc6a7a9997")]
        [AssociationId("d5ed6948-f657-4d47-89c8-d860e2971138")]
        [RoleId("b65552b5-99c7-4b91-b9b6-a70ec35c3ae2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        Party[] Originators { get; set; }

        #region Allors
        [Id("ece02647-000a-4373-8f01-f4b7d1c75dd5")]
        [AssociationId("e580ed8f-a7a4-40c3-9c0a-4cdbe95354a6")]
        [RoleId("dde368dc-c198-4744-b3b2-1a2e0d2976e4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        Party[] Receivers { get; set; }

        #region Allors
        [Id("87B8680D-4161-420B-82B9-3A4E08A572B3")]
        [AssociationId("9F127485-9CDE-41A2-8B71-36F8BD1652EE")]
        [RoleId("22158DC4-A38D-4D48-AA48-19FA6B35B6AC")]
        #endregion
        [Required]
        [Workspace]
        bool IncomingLetter { get; set; }
    }
}