namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7107db4e-8406-4fe3-8136-271077c287f8")]
	#endregion
	[Inherit(typeof(InternalAccountingTransactionInterface))]

	[Plural("Depreciations")]
	public partial class DepreciationClass : Class
	{
		#region Allors
		[Id("83ae8e4e-c4cd-4f27-b5fd-b468e4603295")]
		[AssociationId("031bc098-9f75-4ced-bcca-0f35519887b2")]
		[RoleId("9e2be493-0100-474c-a49b-00b69c8d8ce1")]
		#endregion
		[Indexed]
		[Type(typeof(FixedAssetInterface))]
		[Plural("FixedAssets")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType FixedAsset;



		public static DepreciationClass Instance {get; internal set;}

		internal DepreciationClass() : base(MetaPopulation.Instance)
        {
        }
        internal override void AppsExtend()
        {
            this.FixedAsset.RoleType.IsRequired = true;
        }
    }
}