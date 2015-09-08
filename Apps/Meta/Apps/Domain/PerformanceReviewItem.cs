namespace Allors.Meta
{
    public partial class PerformanceReviewItemClass
	{
	    internal override void AppsExtend()
        {
            this.PerformanceReviewItemType.RoleType.IsRequired = true;
            this.RatingType.RoleType.IsRequired = true;
        }
	}
}