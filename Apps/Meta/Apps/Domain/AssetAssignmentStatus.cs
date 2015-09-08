namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("644660d4-d5d0-4bd9-8cba-17696af0b9ed")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("AssetAssignmentStatuses")]
	public partial class AssetAssignmentStatusClass : Class
	{

		public static AssetAssignmentStatusClass Instance {get; internal set;}

		internal AssetAssignmentStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}