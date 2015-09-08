namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5cd86f69-e09b-4150-a2a6-2eed4c72b426")]
	#endregion
	[Plural("ElectronicAddresses")]
	[Inherit(typeof(ContactMechanismInterface))]

  	public partial class ElectronicAddressInterface: Interface
	{
		#region Allors
		[Id("90288ea6-cb3b-47ad-9bb1-aa71d7c65926")]
		[AssociationId("8b7e4656-a33b-4d75-8721-106c6f7f2c4e")]
		[RoleId("f04e16be-007f-43dc-974c-92c1423a5426")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ElectronicAddressStrings")]
		public RelationType ElectronicAddressString;



		public static ElectronicAddressInterface Instance {get; internal set;}

		internal ElectronicAddressInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ElectronicAddressString.RoleType.IsRequired = true;
        }
    }
}