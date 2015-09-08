namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ca633037-ba1e-4304-9f2c-3353c287474b")]
	#endregion
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]

	[Plural("PartyContactMechanisms")]
	public partial class PartyContactMechanismClass : Class
	{
		#region Allors
		[Id("2ca2f403-67f8-49e6-9a62-4547d2cc83a1")]
		[AssociationId("b4dea5e8-2fa0-49a4-aed3-6bc32aade7e6")]
		[RoleId("6d98949f-823f-4a66-92c2-8182156efef9")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismPurposeClass))]
		[Plural("ContactPurposes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ContactPurpose;

		#region Allors
		[Id("afd94e13-db8e-45cd-8d6c-d9085054d71f")]
		[AssociationId("55fa72b2-2d47-442b-90a8-03537771df30")]
		[RoleId("d435dd59-d047-4952-bd96-f644d226e975")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("ContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ContactMechanism;

		#region Allors
		[Id("eb412c34-7127-4b37-8831-5280b9ed1885")]
		[AssociationId("d24adc95-f792-4caa-b6f4-de6a0caa8114")]
		[RoleId("a9168214-b208-4a21-905c-da53f9a4619d")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("UseAsDefaults")]
		public RelationType UseAsDefault;

		#region Allors
		[Id("f859fd15-4359-4de1-9927-75b6e443ffab")]
		[AssociationId("0935e3ed-7141-47b2-b4cc-72274b9e7680")]
		[RoleId("1c57da10-ffcb-4b97-a930-ae10c2059b98")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("NonSolicitationIndicators")]
		public RelationType NonSolicitationIndicator;



		public static PartyContactMechanismClass Instance {get; internal set;}

		internal PartyContactMechanismClass() : base(MetaPopulation.Instance)
        {
        }
	}
}