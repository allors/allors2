namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("34d08c66-6d7a-4089-b862-c93feda67ef1")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("BillingAccounts")]
	public partial class BillingAccountClass : Class
	{
		#region Allors
		[Id("408019e5-6b8a-4a50-be0a-0b3de3cd55d9")]
		[AssociationId("af54fecc-d537-4611-8324-fbe426063dd0")]
		[RoleId("ef0e1a32-4873-4d22-b037-16afb00e7fce")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("8a550d4b-d881-495b-9326-f2494a50fb5f")]
		[AssociationId("3562b1e4-0acc-4a94-a111-f1afb8d889d4")]
		[RoleId("7746a7a0-3dee-4279-8114-639c5f106d4d")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("ContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ContactMechanism;



		public static BillingAccountClass Instance {get; internal set;}

		internal BillingAccountClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}