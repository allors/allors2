namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4f46f32a-04e6-4ccc-829b-68fb3336f870")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Carriers")]
	public partial class CarrierClass : Class
	{
		#region Allors
		[Id("8defc9c0-6cc8-4e8a-b892-dad6ff908b85")]
		[AssociationId("9a0673e4-8c79-4677-a542-e17f4211d74d")]
		[RoleId("cde2981f-9ba6-4c85-a0cc-b98bd3b7a8a2")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static CarrierClass Instance {get; internal set;}

		internal CarrierClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}