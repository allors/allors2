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
    title: 'Sales',
    icon: 'business',
    children: [
      { id: ids.RequestForQuote },
      { id: ids.ProductQuote },
      { id: ids.SalesOrder },
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
      { id: ids.SerialisedItemCharacteristic },
      { id: ids.ProductType },
    ]
  },
  {
    title: 'Accounting',
    icon: 'business',
    children: [
      { id: ids.PurchaseInvoice },
      { id: ids.SalesInvoice },
    ]
  },
  {
    title: 'WorkEffort',
    icon: 'business',
    children: [
      { id: ids.WorkEffort },
    ]
  }
];
