namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("587e017d-eb9a-412c-bd21-8ff91c42765b")]
	#endregion
	[Inherit(typeof(ExternalAccountingTransactionInterface))]

	[Plural("Notes")]
	public partial class NoteClass : Class
	{

		public static NoteClass Instance {get; internal set;}

		internal NoteClass() : base(MetaPopulation.Instance)
        {
        }
	}
}