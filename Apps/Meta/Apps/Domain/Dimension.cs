namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("26981f3f-f683-4a59-91ad-7a0e4243aea6")]
	#endregion
	[Inherit(typeof(ProductFeatureInterface))]

	[Plural("Dimensions")]
	public partial class DimensionClass : Class
	{
		#region Allors
		[Id("6901f550-4470-4acf-8234-96c1b1bd0bc6")]
		[AssociationId("094356ad-e8d6-4f6b-b1c6-910a3d9fc518")]
		[RoleId("1863b99e-415e-42a0-acef-613f7b3e3315")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Units")]
		public RelationType Unit;

		#region Allors
		[Id("c4fa3792-9784-43ea-91f1-1533f1d12765")]
		[AssociationId("ea393d05-73c8-4b52-b578-c02cc718f076")]
		[RoleId("fae40aa7-15ea-4b37-8d33-86df26b14b54")]
		#endregion
		[Indexed]
		[Type(typeof(UnitOfMeasureClass))]
		[Plural("UnitsOfMeasure")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType UnitOfMeasure;



		public static DimensionClass Instance {get; internal set;}

		internal DimensionClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.UnitOfMeasure.RoleType.IsRequired = true;
        }
    }
}