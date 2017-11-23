namespace Allors.Meta
{
    public partial class MetaBase
    {
        public static MetaBase Instance { get; internal set;}

		public readonly Domain Domain;

        internal MetaBase(MetaPopulation metaPopulation)
        {
			this.Domain = new Domain(metaPopulation, new System.Guid("770538dd-7b19-4694-bdce-cf04dcf9cf62"))
			{
				Name = "Base"
			};
        }
    }

    public partial class MetaCore
    {
        public static MetaCore Instance { get; internal set;}

		public readonly Domain Domain;

        internal MetaCore(MetaPopulation metaPopulation)
        {
			this.Domain = new Domain(metaPopulation, new System.Guid("fc9bad0e-d70d-4f93-bca7-0c7944ef1abb"))
			{
				Name = "Core"
			};
        }
    }

    public partial class MetaApps
    {
        public static MetaApps Instance { get; internal set;}

		public readonly Domain Domain;

        internal MetaApps(MetaPopulation metaPopulation)
        {
			this.Domain = new Domain(metaPopulation, new System.Guid("5c20e532-a87f-4639-ae3b-67a406a30105"))
			{
				Name = "Apps"
			};
        }
    }
}