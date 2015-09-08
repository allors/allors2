namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("87fbf592-45a1-4ef2-85ca-f47d4c51abca")]
	#endregion
	[Inherit(typeof(PaymentMethodInterface))]

	[Plural("Cashes")]
	public partial class CashClass : Class
	{
		#region Allors
		[Id("39c8beda-d284-442b-886a-6d6b2fb51cc8")]
		[AssociationId("f90e529a-8303-4a66-9622-144cfaed3bf3")]
		[RoleId("90ee494e-2194-4972-bdde-7a3a30aff736")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("PersonResponsible")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PersonResponsible;



		public static CashClass Instance {get; internal set;}

		internal CashClass() : base(MetaPopulation.Instance)
        {
        }
	}
}