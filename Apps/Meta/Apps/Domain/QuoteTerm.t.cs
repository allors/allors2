namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("cd60cf6d-65ba-4e31-b85d-16c19fc0978b")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("QuoteTerms")]
	public partial class QuoteTermClass : Class
	{
		#region Allors
		[Id("ce70acf3-9bc4-4572-9487-ef1ab900b488")]
		[AssociationId("df24f334-df05-48b2-95c8-dc69bafbdf06")]
		[RoleId("c64eb6c1-0bf8-4504-8c35-e4753f050911")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TermValues")]
		public RelationType TermValue;

		#region Allors
		[Id("e53203f0-1d8f-45ea-bcc2-627c9440e66f")]
		[AssociationId("8319e551-dc5c-461e-bbf2-6c37b50becce")]
		[RoleId("88fc03e5-6ab5-4b95-9027-282a595ca3f7")]
		#endregion
		[Indexed]
		[Type(typeof(TermTypeClass))]
		[Plural("TermTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TermType;



		public static QuoteTermClass Instance {get; internal set;}

		internal QuoteTermClass() : base(MetaPopulation.Instance)
        {
        }
	}
}