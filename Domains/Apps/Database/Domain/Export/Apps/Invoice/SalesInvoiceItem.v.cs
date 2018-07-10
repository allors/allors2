namespace Allors.Domain
{
    public partial class SalesInvoiceItem
    {
        private bool IsSubTotalItem => this.AppsIsSubTotalItem;
    }
}