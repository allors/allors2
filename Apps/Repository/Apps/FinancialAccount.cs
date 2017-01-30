namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("27b45d45-459a-43cb-87b0-f8842ec56445")]
    #endregion
	public partial interface FinancialAccount : AccessControlledObject 
    {


        #region Allors
        [Id("f90475c7-4a2d-42fd-bafd-96557c217c19")]
        [AssociationId("5566f06a-feb0-45f0-9a84-673b758b6af9")]
        [RoleId("29bda327-86c2-4fa4-af63-8e870cc736b5")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        FinancialAccountTransaction[] FinancialAccountTransactions { get; set; }

    }
}