namespace Allors.Meta
{
    #region Allors
	[Id("4ea604af-7fcc-49f8-8b3b-6be712cea6d9")]
    #endregion
    [Inherit(typeof(BaseDomain))]
    public partial class AppsDomain : Domain
	{
		public static AppsDomain Instance { get; internal set; }

		private AppsDomain(MetaPopulation metaPopulation) : base(metaPopulation)
        {
        }
	}
}