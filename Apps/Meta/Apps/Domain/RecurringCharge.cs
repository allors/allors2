namespace Allors.Meta
{
	#region Allors
	[Id("a71e670c-f089-4ec1-8295-dda8e7b62a19")]
	#endregion
	[Inherit(typeof(PriceComponentInterface))]
	public partial class RecurringChargeClass : Class
	{
		#region Allors
		[Id("f95e774f-239e-4136-a964-c3d1841a43ba")]
		[AssociationId("46b2864f-5c9b-43b9-a6b0-0bcf907adbc8")]
		[RoleId("97a9949b-6266-4fa2-a33a-3b13eaf21a93")]
		#endregion
		[Indexed]
		[Type(typeof(TimeFrequencyClass))]
		[Plural("TimeFrequencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TimeFrequency;

		public static RecurringChargeClass Instance {get; internal set;}

		internal RecurringChargeClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[PriceComponentInterface.Instance.Price.RoleType].IsRequiredOverride = true;
        }
    }
}