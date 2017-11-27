namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("2a9b5a77-6065-4f2a-bbc3-655426f0f97b")]
    #endregion
    public partial class C3 :  System.Object, I3, I23, I34 
    {
        #region inherited properties
        public C4[] I3C4many2manies { get; set; }

        public string I3AllorsString { get; set; }

        public I4[] I3I4one2manies { get; set; }

        public C4[] I3C4one2manies { get; set; }

        public I4[] I3I4many2manies { get; set; }

        public I4 I3I4many2one { get; set; }

        public C4 I3C4one2one { get; set; }

        public I4 I3I4one2one { get; set; }

        public C4 I3C4many2one { get; set; }

        public string I3StringEquals { get; set; }

        public C1 C1one2one { get; set; }

        public double S1234AllorsDouble { get; set; }

        public decimal S1234AllorsDecimal { get; set; }

        public int S1234AllorsInteger { get; set; }

        public S1234 S1234many2one { get; set; }

        public C2 S1234C2one2one { get; set; }

        public C2[] S1234C2many2manies { get; set; }

        public S1234[] S1234one2manies { get; set; }

        public C2[] S1234C2one2manies { get; set; }

        public S1234[] S1234many2manies { get; set; }

        public string ClassName { get; set; }

        public DateTime S1234AllorsDateTime { get; set; }

        public S1234 S1234one2one { get; set; }

        public C2 S1234C2many2one { get; set; }

        public string S1234AllorsString { get; set; }

        public bool S1234AllorsBoolean { get; set; }

        public string I23AllorsString { get; set; }

        public decimal I34AllorsDecimal { get; set; }

        public bool I34AllorsBoolean { get; set; }

        public double I34AllorsDouble { get; set; }

        public int I34AllorsInteger { get; set; }

        public string I34AllorsString { get; set; }

        #endregion

        #region Allors
        [Id("02a07b71-a40d-4600-ae12-370be7e973f5")]
        [AssociationId("590d3c5a-1732-48db-ab12-d194a8cb94a9")]
        [RoleId("f7e26d33-558d-4e5e-8b12-3116c110cf1f")]
        [Size(256)]
        #endregion
        public string C3AllorsString { get; set; }
        #region Allors
        [Id("0e06c403-2a29-4f40-b7b6-3e4fed28aeba")]
        [AssociationId("e64ba775-20d8-46f7-9777-e5f754d58428")]
        [RoleId("8221f87c-a4b0-49fa-88cc-47aa9814d4af")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public C2[] C3C2many2manies { get; set; }
        #region Allors
        [Id("29e76785-f3eb-48b9-a9bf-c44e64762631")]
        [AssociationId("09a88684-7e1c-4aab-9636-bc00e90d80bc")]
        [RoleId("cd5e8d50-2aa8-4604-8e9d-28d9b29dece4")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public I4 C3I4one2one { get; set; }
        #region Allors
        [Id("39313684-8ea1-4f15-aada-2a16feb148ea")]
        [AssociationId("1835fdd6-314c-4fa3-8fb1-e48076f3ad2a")]
        [RoleId("64491eca-3962-419b-847b-f7da095a8637")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public C4 C3C4many2one { get; set; }
        #region Allors
        [Id("5e6c2802-3dc5-405a-a2f7-03c9361d4562")]
        [AssociationId("710ae2d8-711b-4122-9b57-946fd3d815c2")]
        [RoleId("6011796c-9fc6-4e40-be46-b5f937267057")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        public C4[] C3C4many2manies { get; set; }
        #region Allors
        [Id("8f2225b7-8c15-414a-a9be-50c757f80b3e")]
        [AssociationId("b75bb087-63c3-475f-8e47-07d2d63ac499")]
        [RoleId("a3f58a75-df00-4c97-8124-2a002864bdb4")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public I4[] C3I4many2manies { get; set; }
        #region Allors
        [Id("92505f70-3611-4ed6-bd27-71030299e176")]
        [AssociationId("c817ff5f-b31f-43e5-b04d-72d28c666085")]
        [RoleId("7a9e571f-beea-47a9-9bdc-d498d5bef2ae")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public C2[] C3C2one2manies { get; set; }
        #region Allors
        [Id("958bc7c6-d609-4407-ba92-50726c9af5d5")]
        [AssociationId("6ec989ea-a41e-46ac-b754-617c204a314c")]
        [RoleId("7e9f33f4-28a2-4abd-bb10-6acab5c6ab94")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public C2 C3C2many2one { get; set; }
        #region Allors
        [Id("b7745909-a63a-448a-b4bd-6caf614c4b12")]
        [AssociationId("7d073606-bcb1-4bd8-a4ee-5f7c24712638")]
        [RoleId("9e354367-0938-489f-a5df-4d6c1dd95875")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public I4 C3I4many2one { get; set; }
        #region Allors
        [Id("d1601926-ae62-4592-b15b-6511e0d98355")]
        [AssociationId("4a8dd1f7-a02f-49cb-a078-77ad93e3887d")]
        [RoleId("91874a28-56b7-4d5d-8fc9-33a59cabab95")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public C4[] C3C4one2manies { get; set; }
        #region Allors
        [Id("d81da318-f954-42b4-b605-e011a92726ba")]
        [AssociationId("afd34195-4149-4070-9fb3-5e6509b5e503")]
        [RoleId("5e68d25d-e630-4807-aab6-8bb015009cbe")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public C2 C3C2one2one { get; set; }
        #region Allors
        [Id("da44bf79-b72e-4565-bd33-0eb278a6f4ec")]
        [AssociationId("f13c448a-e101-4da0-b79b-5e0efc6462b9")]
        [RoleId("d833c0b7-bd97-44bc-a8f2-07d509d82a09")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public C4 C3C4one2one { get; set; }
        #region Allors
        [Id("dd006700-a00c-4c67-819e-1d63df26a5b6")]
        [AssociationId("5d1441a6-f665-470d-8f7f-03d794e0ee06")]
        [RoleId("3ad6a27f-4604-402a-a504-8ef3a3e7ccee")]
        [Size(256)]
        #endregion
        public string C3StringEquals { get; set; }
        #region Allors
        [Id("ed3267fb-fbc4-4e38-87f5-8e2ee91b1bac")]
        [AssociationId("9411ef61-3a6d-41bd-a9db-3d0f81db6382")]
        [RoleId("8fdf96a7-73e7-418d-90c2-a6d72a1629a9")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public I4[] C3I4one2manies { get; set; }


        #region inherited methods
        #endregion

    }
}