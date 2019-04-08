namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("15c8c72b-f551-41b0-86c8-80f02424ec4c")]
    #endregion
    public partial class WorkEffortBilling : Object 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("3c83ca1d-b20e-4e8c-aa23-3bb03f421ba7")]
        [AssociationId("506b220c-7965-4d51-8413-feabfef71c07")]
        [RoleId("4d2f7ed8-881f-49e4-944a-ba291ec671d0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public WorkEffort WorkEffort { get; set; }

        #region Allors
        [Id("91d38ce9-bf06-4272-bdd8-13401084223d")]
        [AssociationId("d0189269-2f90-46c5-a1ff-48bad8712b34")]
        [RoleId("e2a7d998-78bb-4d21-b4a8-d6fbddc4b089")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Percentage { get; set; }

        #region Allors
        [Id("c6ed6a42-6889-4ad9-b76a-22bd45e02e75")]
        [AssociationId("99eb5187-9c6b-48bf-a587-81a5d1603cb1")]
        [RoleId("977e55a2-1592-42ff-b7a2-9f1630b36714")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InvoiceItem InvoiceItem { get; set; }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}