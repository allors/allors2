namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("f135770b-7228-4e4b-b7ea-9307b6317fd2")]
    #endregion
    public partial interface InventoryItemConfiguration : Commentable, Object
    {
        #region Allors
        [Id("92a85a6b-4f65-4ba4-bd5e-bf44d5a9ca56")]
        [AssociationId("e7e7fef5-a973-42b7-8c96-5ede712a353c")]
        [RoleId("6beb2ac6-0319-4524-80a2-54393ba77e69")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        InventoryItem InventoryItem { get; set; }

        #region Allors
        [Id("f041b297-e2bb-4ada-ab89-08ec9bcd6513")]
        [AssociationId("6ec9252a-817c-4e39-a2f7-809c86888b9c")]
        [RoleId("0454d8e0-ab31-4907-85b1-41103091a08f")]
        #endregion
        [Required]
        int Quantity { get; set; }

        #region Allors
        [Id("f1d4ceeb-f859-4996-babc-dc55837489e0")]
        [AssociationId("a0cb4a4e-322e-4f8c-b7b8-b171b1b0aaa5")]
        [RoleId("df3d337b-4998-4604-961c-3c074f91cd1b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        InventoryItem ComponentInventoryItem { get; set; }
    }
}