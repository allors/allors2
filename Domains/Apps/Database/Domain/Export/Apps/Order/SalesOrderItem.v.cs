namespace Allors.Domain
{
    public partial class SalesOrderItem
    {
        private bool IsSubTotalItem => this.AppsIsSubTotalItem;
    }
}