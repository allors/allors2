namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d551887b-8520-478d-bf2c-b0f26e3bc356")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PackageQuantityBreaks")]
	public partial class PackageQuantityBreakClass : Class
	{
		#region Allors
		[Id("0df167e7-e1b7-4c2a-9de4-f06fc359600f")]
		[AssociationId("66f59599-97de-44a7-908a-a86a43e332e0")]
		[RoleId("7d181c6a-e465-4e2e-a789-82f2c956b0c2")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Froms")]
		public RelationType From;

		#region Allors
		[Id("c282c1db-d9a5-40b8-aed1-ddbd060cdbcd")]
		[AssociationId("edc54775-b7d9-4d2c-94a3-93e8974f5da8")]
		[RoleId("2c753aa2-9ee7-4b06-9851-ce992a3545e3")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Throughs")]
		public RelationType Through;



		public static PackageQuantityBreakClass Instance {get; internal set;}

		internal PackageQuantityBreakClass() : base(MetaPopulation.Instance)
        {
        }
	}
}