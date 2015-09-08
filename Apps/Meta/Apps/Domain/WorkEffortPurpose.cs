namespace Allors.Meta
{
	#region Allors
	[Id("8f838542-cc98-4e95-8e60-fb3e774ba92e")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]
	[Plural("WorkEffortPurposes")]
	public partial class WorkEffortPurposeClass : Class
	{
		public static WorkEffortPurposeClass Instance {get; internal set;}

		internal WorkEffortPurposeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}