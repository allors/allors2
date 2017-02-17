namespace Allors.Repository.Domain
{
    #region Allors
    [Id("268f63d2-17da-4f29-b0d0-76db611598c6")]
    #endregion
    public partial class Place :  Object 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("1bf1cc1e-75bf-4a3f-87bd-a2fae2697855")]
        [AssociationId("dce03fde-fbb1-45e7-b78d-9484fa6487ff")]
        [RoleId("d88eaaa2-2622-48ef-960a-1b506d95f238")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Country Country { get; set; }
        #region Allors
        [Id("d029f486-4bb8-43a1-8356-98b9bee10de4")]
        [AssociationId("1454029b-b016-41e1-b142-cea20c7b36d1")]
        [RoleId("dccca416-913b-406a-9405-c5d037af2fd8")]
        [Size(256)]
        #endregion
        public string City { get; set; }
        #region Allors
        [Id("d80d7c6a-138a-43dd-9748-8ffb89b1dabb")]
        [AssociationId("944c752e-742c-426b-9ac9-c405080d4a8d")]
        [RoleId("b54fcc51-e294-4732-82bf-a1117a4e2219")]
        [Size(256)]
        #endregion
        public string PostalCode { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        #endregion
    }
}