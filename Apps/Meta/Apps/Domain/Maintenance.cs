namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5ad24730-a81e-4160-9af9-fa25342a5e96")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("Maintenances")]
	public partial class MaintenanceClass : Class
	{

		public static MaintenanceClass Instance {get; internal set;}

		internal MaintenanceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}