namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("B1B19B64-A507-435D-9D26-102AC7D97049")]
    #endregion
    public partial interface DerivationCounted : Object
    {
        #region Allors
        [Id("4C66ED2E-6C08-4F9C-9A58-75F71AF9BAD1")]
        [AssociationId("EC166B56-52FC-41A7-88BE-79D4D5883532")]
        [RoleId("A292C1BF-1DB2-4ACA-B639-5A56F58D3796")]
        #endregion
        [Required]
        int DerivationCount { get; set; }
    }
}
