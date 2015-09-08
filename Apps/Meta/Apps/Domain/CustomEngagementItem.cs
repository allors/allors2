namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("78022da7-d11c-4ab7-96f5-099d6608c4bb")]
	#endregion
	[Inherit(typeof(EngagementItemInterface))]

	[Plural("CustomEngagementItems")]
	public partial class CustomEngagementItemClass : Class
	{
		#region Allors
		[Id("71a3ed63-922f-44ae-8e89-6425759b3eb3")]
		[AssociationId("00621849-ee7b-4a7e-b5c3-7ca2e2d40b3a")]
		[RoleId("2b2d9ceb-cce9-4edd-bbaa-2829b3e5e32f")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("DescriptionsOfWork")]
		public RelationType DescriptionOfWork;

		#region Allors
		[Id("f0b91526-924e-4f11-b27c-187010e1dff7")]
		[AssociationId("21f41aa4-9417-4822-afba-6e424dd936f2")]
		[RoleId("9f787d7c-663d-4856-a3cb-8d65b4802744")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AgreedUponPrices")]
		public RelationType AgreedUponPrice;



		public static CustomEngagementItemClass Instance {get; internal set;}

		internal CustomEngagementItemClass() : base(MetaPopulation.Instance)
        {
        }
	}
}