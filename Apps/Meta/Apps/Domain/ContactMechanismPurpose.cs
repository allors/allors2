namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0c6880e7-b41c-47a6-ab40-83e391c7a025")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("ContactMechanismPurposes")]
	public partial class ContactMechanismPurposeClass : Class
	{

		public static ContactMechanismPurposeClass Instance {get; internal set;}

		internal ContactMechanismPurposeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}