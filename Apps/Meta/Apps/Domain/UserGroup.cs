namespace Allors.Meta
{
	public partial class UserGroupClass : Class
	{
		#region Allors
		[Id("6b3e1fa8-5718-4a60-91c6-c6bb42be26fd")]
		[AssociationId("35c67ad6-c5ff-4e53-82a7-c457323b02b3")]
		[RoleId("5c5e8dd8-4277-4be0-a59b-cc92dc8dde97")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;
	}
}