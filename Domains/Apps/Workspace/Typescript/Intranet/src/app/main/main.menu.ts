import { ids } from '../../allors/meta/generated';

export interface MenuItem {
  id?: string;
  link?: string;
  title?: string;
  icon?: string;
  children?: MenuItem[];
}

export const menu: MenuItem[] = [
  { title: 'Home', icon: 'home', link: '/' },
  {
    title: 'Contacts', icon: 'group',
    children: [
      { id: ids.Person },
      { id: ids.Organisation },
      { id: ids.CommunicationEvent },
    ]
  },
  {
    title: 'Products', icon: 'label',
    children: [
      { id: ids.NonUnifiedGood, title: 'Goods' },
      { id: ids.NonUnifiedPart, title: 'Parts' },
      { id: ids.Catalogue },
      { id: ids.ProductCategory },
      { id: ids.SerialisedItemCharacteristic, title: 'Characteristics' },
      { id: ids.ProductType },
      { id: ids.SerialisedItem, title: 'Serialised Assets' },
    ]
  },
  {
    title: 'Sales', icon: 'credit_card',
    children: [
      { id: ids.RequestForQuote },
      { id: ids.ProductQuote },
      { id: ids.SalesOrder },
      { id: ids.SalesInvoice },
    ]
  },
  {
    title: 'Purchasing', icon: 'local_shipping',
    children: [
      { id: ids.PurchaseInvoice },
    ]
  },
  {
    title: 'WorkEfforts', icon: 'schedule',
    children: [
      { id: ids.WorkEffort },
    ]
  }
];
