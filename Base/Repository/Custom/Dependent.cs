namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("0cb8d2a7-4566-432f-9882-893b05a77f44")]
    #endregion
    public partial class Dependent :  Object, Deletable 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("8859af04-ba38-42ce-8ac9-f428c3f92f31")]
        [AssociationId("cd3972e6-8ad4-4b01-9381-4d18718c7538")]
        [RoleId("d6b1d6b6-539b-4b12-9363-18e7e9ab632c")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        public Dependee Dependee { get; set; }
        #region Allors
        [Id("9884955e-74ed-4f9d-9362-8e0274c53bf9")]
        [AssociationId("5b97e356-9bcd-4c4e-be7a-ef577eef5f14")]
        [RoleId("d067129b-8440-4fc7-80d3-832ce569fe54")]
        #endregion
        public int Counter { get; set; }
        #region Allors
        [Id("e971733a-c381-4b5e-8e62-6bbd6d285bd7")]
        [AssociationId("6269351a-5e08-4b10-a895-ff2f669b259f")]
        [RoleId("2b916cdb-93a6-42f1-b4e6-625b941c1874")]
        #endregion
        public int Subcounter { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Delete(){}
        #endregion

    }
}