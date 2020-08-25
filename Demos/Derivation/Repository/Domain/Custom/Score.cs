using Allors.Repository.Attributes;
using System;

namespace Allors.Repository
{
    #region Allors
    [Id("2df33e72-cb19-4647-9160-ed10a2552729")]
    #endregion

    public class Score : Object, Deletable
    {
        #region Inherited Properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors

        [Id("19b4ccc7-c396-474d-a3b2-ad631932545e")]
        [AssociationId("4c23c03b-92fc-4e2f-bd63-f97394b80a7e")]
        [RoleId("e28c39c1-4288-4533-b997-b481a783295e")]

        #endregion Allors
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public Person Player { get; set; }

        #region Allors

        [Id("e68d4b5c-41c7-41d1-93cb-b00f71bcb6bf")]
        [AssociationId("25863fd8-5850-4bb1-8170-a9efee52af14")]
        [RoleId("33df16a6-915e-4a3f-84e2-78dec18ceba4")]

        #endregion Allors
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public int Value { get; set; }

        #region inherited methods
        public void Delete()
        {
        }
        public void OnBuild()
        {

        }

        public void OnDerive()
        {

        }

        public void OnInit()
        {

        }

        public void OnPostBuild()
        {

        }

        public void OnPostDerive()
        {

        }

        public void OnPreDerive()
        {

        }

        #endregion inherited methods
    }
}