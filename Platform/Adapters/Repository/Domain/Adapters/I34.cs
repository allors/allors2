namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("ebc22540-54c8-4601-a43d-2ed6da9f3e79")]
    #endregion
    public partial interface I34 : Object
    {


        #region Allors
        [Id("37e8d764-bfeb-40d8-b7e9-d94e455dcc11")]
        [AssociationId("fd9a1d7e-913e-4fce-88b3-320ab6bbce96")]
        [RoleId("d079cba2-c2af-4dc5-abf3-46abeb8b4928")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal I34AllorsDecimal { get; set; }


        #region Allors
        [Id("4a6db64f-aeeb-4657-a24c-7997129f3efa")]
        [AssociationId("16ce6c74-b457-4a3b-b173-c7fec74b8178")]
        [RoleId("77f678c6-93ee-465a-b252-7e0530ed19ea")]
        #endregion
        bool I34AllorsBoolean { get; set; }


        #region Allors
        [Id("9b774204-37f3-4663-9162-dc801ea200f6")]
        [AssociationId("f45e6b10-2b46-4100-93c3-d76f25526df3")]
        [RoleId("e5b452ba-b29f-47f4-be48-029289e91345")]
        #endregion
        double I34AllorsDouble { get; set; }


        #region Allors
        [Id("cd30dada-24c5-4b94-8f58-ab1018f087ea")]
        [AssociationId("dca9ffc6-4620-4b5f-888d-35ea77ba1ad8")]
        [RoleId("0649567c-2942-4d6e-9fa5-3672f8eb77a3")]
        #endregion
        int I34AllorsInteger { get; set; }


        #region Allors
        [Id("d8125c69-1921-4e16-84bc-d3d174be7b83")]
        [AssociationId("0d74fad0-c0a0-4f3d-b6fc-706c6343f253")]
        [RoleId("ad8c99c8-7628-4f82-8188-287b2dbfbf42")]
        [Size(256)]
        #endregion
        string I34AllorsString { get; set; }

    }
}
