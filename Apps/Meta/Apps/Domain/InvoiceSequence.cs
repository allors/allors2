namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3b8e751c-6778-44cb-93a0-d35b86b724e0")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("InvoiceSequences")]
	public partial class InvoiceSequenceClass : Class
	{

		public static InvoiceSequenceClass Instance {get; internal set;}

		internal InvoiceSequenceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}