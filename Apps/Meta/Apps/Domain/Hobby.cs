namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2f18f79f-dd13-4e89-b3fa-95d789dd383e")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("Hobbies")]
	public partial class HobbyClass : Class
	{

		public static HobbyClass Instance {get; internal set;}

		internal HobbyClass() : base(MetaPopulation.Instance)
        {
        }
	}
}