namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("c3e82ab0-f586-4913-acb0-838ffd6701f8")]
    #endregion
    public partial class SingleUnit : System.Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("acf7d284-2480-4a09-a13b-ba4ba96e0892")]
        [AssociationId("b15641e3-ad46-4c90-bc58-32758a27e53e")]
        [RoleId("8e0df573-0931-4bf0-a3bb-1cf88a530d98")]
        #endregion
        public int AllorsInteger { get; set; }


        #region inherited methods
        #endregion

    }
}
