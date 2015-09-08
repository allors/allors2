namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("11214660-3c3a-42e9-8f12-f475d823da64")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]

	[Plural("CostCenterCategories")]
	public partial class CostCenterCategoryClass : Class
	{
		#region Allors
		[Id("15eade6f-f540-4916-9d66-30f4bd0f260a")]
		[AssociationId("f67c26e6-73e2-490a-aaf5-b66cd8e30972")]
		[RoleId("b0767ddc-1b97-4289-afec-0519182982d0")]
		#endregion
		[Indexed]
		[Type(typeof(CostCenterCategoryClass))]
		[Plural("Parents")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Parent;

		#region Allors
		[Id("45b0b049-e047-4490-9dde-c48fb1e7bfc3")]
		[AssociationId("130462ef-9d1d-48d9-b0f5-40c82ccea0a2")]
		[RoleId("1fd21431-34f6-4f5c-ad54-abecb5e717e1")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CostCenterCategoryClass))]
		[Plural("Ancestors")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Ancestor;

		#region Allors
		[Id("b20dc3d5-5067-4697-becf-0e8d44f117c7")]
		[AssociationId("d88647c8-b367-48e0-aef9-2af923a17b6f")]
		[RoleId("0a65a2da-f091-4ed1-9af9-80ff63123adf")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CostCenterCategoryClass))]
		[Plural("Children")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Child;

		#region Allors
		[Id("fcb56761-342b-4d62-ba5b-27e0a0f405dd")]
		[AssociationId("4804ef05-ddb6-4f15-940a-cd663a7bef55")]
		[RoleId("c1d56a33-314d-4aa7-a202-77ae675092ab")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static CostCenterCategoryClass Instance {get; internal set;}

		internal CostCenterCategoryClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}