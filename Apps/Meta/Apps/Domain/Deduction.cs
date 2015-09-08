namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c04ccfcf-ae3f-4e7f-9e19-503ba547b678")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Deductions")]
	public partial class DeductionClass : Class
	{
		#region Allors
		[Id("0deb347e-22c7-4b48-b461-aa579e156398")]
		[AssociationId("aced036b-04ba-41da-a3fd-fb3d0782b8c6")]
		[RoleId("b09c7a91-bcca-4f80-b68c-309fdf1e80b0")]
		#endregion
		[Indexed]
		[Type(typeof(DeductionTypeClass))]
		[Plural("DeductionTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DeductionType;

		#region Allors
		[Id("abaece2a-d56d-4af9-8421-1d587cd9dda2")]
		[AssociationId("b8d4b48b-292a-4348-8dba-15f89d573dd5")]
		[RoleId("1077f672-905b-4198-ada5-e52fb34c986e")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;



		public static DeductionClass Instance {get; internal set;}

		internal DeductionClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Amount.RoleType.IsRequired = true;
            this.DeductionType.RoleType.IsRequired = true;
        }
    }
}