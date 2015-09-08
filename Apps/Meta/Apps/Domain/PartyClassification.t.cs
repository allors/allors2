namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3bb83aa5-e58a-4421-bdbc-3c9fa0b2324f")]
	#endregion
	[Plural("PartyClassifications")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class PartyClassificationInterface: Interface
	{
		#region Allors
		[Id("4f35ae7e-fe06-4a3b-abe1-adb78fcf2e6b")]
		[AssociationId("fd171d61-90ae-4169-8286-6054b82569a1")]
		[RoleId("654f2aca-2eb7-495c-a739-82c38a629130")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;



		public static PartyClassificationInterface Instance {get; internal set;}

		internal PartyClassificationInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}