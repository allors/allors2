namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6b666a30-7a55-4986-8411-b6179768e70b")]
	#endregion
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("EngagementRates")]
	public partial class EngagementRateClass : Class
	{
		#region Allors
		[Id("0c2c005b-f652-47b2-a42b-7cd511382dd3")]
		[AssociationId("653e795b-52b5-4f76-b1a7-dd34dcc7fc0e")]
		[RoleId("9770c0a9-f8bb-4fd2-ae33-8513e9dcd24b")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("BillingRate")]
		public RelationType BillingRate;

		#region Allors
		[Id("1df6f7fe-6cb9-4c1b-b664-e7ee1e2cec6f")]
		[AssociationId("62d1d3a9-cda9-4036-8cf9-eb0d58bbc29e")]
		[RoleId("5d912d73-b973-40f6-931d-9689674c7e55")]
		#endregion
		[Indexed]
		[Type(typeof(RatingTypeClass))]
		[Plural("RatingTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RatingType;

		#region Allors
		[Id("a920a2c5-021e-4fc9-b38b-21be0003e40f")]
		[AssociationId("6004b01b-26e0-44de-8e2f-6e90532d5070")]
		[RoleId("ffc748e3-9e10-4ad6-bd60-9b747ee5ad93")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Costs")]
		public RelationType Cost;

		#region Allors
		[Id("c54c15ad-0b9b-490c-bdbb-90a49c728b94")]
		[AssociationId("35b7e6dd-5cd0-4aa3-b12c-db10c44b0606")]
		[RoleId("c3e184bf-e863-45d5-b991-b7274757a28e")]
		#endregion
		[Indexed]
		[Type(typeof(PriceComponentInterface))]
		[Plural("GoverningPriceComponents")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType GoverningPriceComponent;

		#region Allors
		[Id("d030a71e-10ba-48cc-9964-456518b705de")]
		[AssociationId("a25d3578-feb9-4eb6-853f-673b2300dc7e")]
		[RoleId("4d655601-37c1-4c31-83a7-406cce05ed4c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ChangeReasons")]
		public RelationType ChangeReason;

		#region Allors
		[Id("e7dafa85-712a-4ea4-abe9-82ddd9afc80c")]
		[AssociationId("e4462db8-7b15-473f-9aca-3fd01d9dba2e")]
		[RoleId("17f43a3b-5772-4413-9df4-5c3250c94bf8")]
		#endregion
		[Indexed]
		[Type(typeof(UnitOfMeasureClass))]
		[Plural("UnitsOfMeasure")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType UnitOfMeasure;



		public static EngagementRateClass Instance {get; internal set;}

		internal EngagementRateClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.RatingType.RoleType.IsRequired = true;
            this.BillingRate.RoleType.IsRequired = true;
        }
    }
}