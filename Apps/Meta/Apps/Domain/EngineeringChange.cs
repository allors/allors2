namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c6c4537a-21f8-4d62-b584-3c609fb2210f")]
	#endregion
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("EngineeringChanges")]
	public partial class EngineeringChangeClass : Class
	{
		#region Allors
		[Id("1a5edba2-6fda-4eb1-9e37-7a0e368ccff0")]
		[AssociationId("1858c16c-47e0-4707-ba58-acd34378d25e")]
		[RoleId("3cdaec27-9203-4ed3-8b9d-a4995db9210d")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Requestors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Requestor;

		#region Allors
		[Id("1b65b18b-c930-49b4-85e4-bb4b07dfdca2")]
		[AssociationId("a34d8a88-50c9-4ece-920c-a1d95388b5ab")]
		[RoleId("5aa1e795-726a-4459-9c1a-e4efb82e807f")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Authorizers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Authorizer;

		#region Allors
		[Id("4487e364-4c5e-4b84-8847-a6ec1f1a0e6f")]
		[AssociationId("79d6a20e-6bc9-49a4-bc81-c10c73871076")]
		[RoleId("85ad4eb9-58e5-422b-a14d-767c7a07414d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("8d123834-364e-47d7-9d1e-63f4ef19f8c0")]
		[AssociationId("b42a9f7c-5032-44e6-97ed-ac4d1ff48445")]
		[RoleId("b3943779-7867-4d29-b562-f67aeb595512")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Designers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Designer;

		#region Allors
		[Id("9caba64b-4959-43f9-a6a6-c76dff62dc02")]
		[AssociationId("4709e3c9-c5cc-457c-a6ff-5eb981b3ef2e")]
		[RoleId("9b113390-7966-4078-9651-e2c80143cee5")]
		#endregion
		[Indexed]
		[Type(typeof(PartSpecificationInterface))]
		[Plural("PartSpecifications")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PartSpecification;

		#region Allors
		[Id("b076cdcc-7e3f-46c8-b127-98d29a4c9e4e")]
		[AssociationId("d0506d24-4cab-4030-be18-59dd879b4bef")]
		[RoleId("a385c549-0dcf-4ed9-b6c5-f264bba435a9")]
		#endregion
		[Indexed]
		[Type(typeof(PartBillOfMaterialInterface))]
		[Plural("PartBillOfMaterials")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PartBillOfMaterial;

		#region Allors
		[Id("c360a1d9-5d8c-4295-aaae-2d50410dd293")]
		[AssociationId("2e3c4504-2130-45dd-b9bf-4e50abb021c0")]
		[RoleId("91e1016b-0724-420f-9a6f-00294e61314a")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(EngineeringChangeObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("caf244e2-f61d-436e-978c-1d0af118949f")]
		[AssociationId("a77aa2de-44a6-4ee1-aa13-45cf8c4da853")]
		[RoleId("f94b94ed-6f33-43c2-b7e5-241823e59a4f")]
		#endregion
		[Indexed]
		[Type(typeof(EngineeringChangeStatusClass))]
		[Plural("EngineeringChangeStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType EngineeringChangeStatus;

		#region Allors
		[Id("d18955d3-1fce-46c9-bb44-5830bfdc09fd")]
		[AssociationId("078d9017-3d7a-4ba5-9c9b-58f778893a15")]
		[RoleId("11d5ba0d-8c70-40a3-873d-ab27e1b8e4bf")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Testers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Tester;

		#region Allors
		[Id("f56d7ad0-430d-482d-a298-5c41ffb082b4")]
		[AssociationId("30ec7448-b167-4273-bb00-cb87a604bb52")]
		[RoleId("013964b5-022e-4b39-89ba-1cfb466fc3ff")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(EngineeringChangeStatusClass))]
		[Plural("CurrentEngineeringChangeStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentEngineeringChangeStatus;



		public static EngineeringChangeClass Instance {get; internal set;}

		internal EngineeringChangeClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }

    }
}