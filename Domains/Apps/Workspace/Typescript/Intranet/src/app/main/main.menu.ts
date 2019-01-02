import { ids } from '../../allors/meta/generated';

export interface MenuItem {
  id?: string;
  link?: string;
  title?: string;
  icon?: string;
  children?: MenuItem[];
}

export const menu: MenuItem[] = [
  { title: 'Home', link: '/' },
  {
    title: 'Contacts',
    icon: 'person',
    children: [
      { id: ids.Person },
      { id: ids.Organisation },
      { id: ids.CommunicationEvent },
    ]
  },
  {
    title: 'Products',
    icon: 'business',
    children: [
      { id: ids.Good },
      { id: ids.Part },
      { id: ids.Catalogue },
      { id: ids.ProductCategory },
      { id: ids.SerialisedItemCharacteristic, title: 'Characteristics' },
      { id: ids.ProductType },
    ]
  },
  {
    title: 'Sales',
    icon: 'business',
    children: [
      { id: ids.RequestForQuote },
      { id: ids.ProductQuote },
      { id: ids.SalesOrder },
      { id: ids.SalesInvoice },
    ]
  },
  {
    title: 'Purchasing',
    icon: 'business',
    children: [
      { id: ids.PurchaseInvoice },
    ]
  },
  {
    title: 'WorkEfforts',
    icon: 'business',
    children: [
      { id: ids.WorkEffort },
    ]
  }
];
