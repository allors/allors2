namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("68a6803d-0e65-4141-ac51-25f4c2e49914")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Deliverables")]
	public partial class DeliverableClass : Class
	{
		#region Allors
		[Id("d7322009-e68f-4635-bc0e-1c0b5a46de62")]
		[AssociationId("953cd640-51dd-4543-a751-242c7e39b596")]
		[RoleId("38bd223e-54ee-455d-8da5-3106029e1fbe")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("dfd5fb95-50ee-48a5-942b-75752f78a615")]
		[AssociationId("fea5e2c3-b8fa-488d-aba6-641176652430")]
		[RoleId("50499eba-a2b0-4ad2-8dc6-72eb2d1997a7")]
		#endregion
		[Indexed]
		[Type(typeof(DeliverableTypeClass))]
		[Plural("DeliverableTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DeliverableType;



		public static DeliverableClass Instance {get; internal set;}

		internal DeliverableClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}