namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6c255f71-ce18-4d51-b0d9-e402ec0e570e")]
	#endregion
	[Inherit(typeof(ContactMechanismInterface))]

	[Plural("TelecommunicationsNumbers")]
	public partial class TelecommunicationsNumberClass : Class
	{
		#region Allors
		[Id("2eabf6bb-48f9-431a-b05b-b892c88db821")]
		[AssociationId("2260b0c0-3a19-43cb-a2f1-22098d428a35")]
		[RoleId("5d7ad31b-a29d-4b3f-8411-744a172bf6a9")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("AreaCodes")]
		public RelationType AreaCode;

		#region Allors
		[Id("31ccabaf-1d31-4b35-93a4-8c18c813c3cd")]
		[AssociationId("3d5d091c-0b5a-421e-bbe8-1c64b35d19b0")]
		[RoleId("f8c81a88-4d53-461a-960d-32325ebc177a")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("CountryCodes")]
		public RelationType CountryCode;

		#region Allors
		[Id("9b587eba-53ee-417c-8324-5c19ec90b745")]
		[AssociationId("7ea12a2f-a018-422b-8a03-a683e2bad699")]
		[RoleId("fd07dae1-2e46-48a7-956d-7b881e6c271a")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(40)]
		[Plural("ContactNumbers")]
		public RelationType ContactNumber;



		public static TelecommunicationsNumberClass Instance {get; internal set;}

		internal TelecommunicationsNumberClass() : base(MetaPopulation.Instance)
        {
        }
	}
}