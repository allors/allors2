import { ids } from '@allors/meta/generated';

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
    title: 'Contacts',
    icon: 'group',
    children: [{ id: ids.Person }, { id: ids.Organisation }, { id: ids.CommunicationEvent }],
  },
  {
    title: 'Products',
    icon: 'label',
    children: [
      { id: ids.Good, title: 'Goods' },
      { id: ids.Part, title: 'Parts' },
      { id: ids.Catalogue },
      { id: ids.ProductCategory },
      { id: ids.SerialisedItemCharacteristic, title: 'Characteristics' },
      { id: ids.ProductType },
      { id: ids.SerialisedItem, title: 'Serialised Assets' },
      { id: ids.UnifiedGood, title: 'Unified Goods' },
    ],
  },
  {
    title: 'Sales',
    icon: 'credit_card',
    children: [{ id: ids.RequestForQuote }, { id: ids.ProductQuote }, { id: ids.SalesOrder }, { id: ids.SalesInvoice }],
  },
  {
    title: 'Purchasing',
    icon: 'local_shipping',
    children: [{ id: ids.PurchaseOrder }, { id: ids.PurchaseInvoice }],
  },
  {
    title: 'Shipments',
    icon: 'local_shipping',
    children: [{ id: ids.Shipment }, { id: ids.Carrier }],
  },
  {
    title: 'WorkEfforts',
    icon: 'schedule',
    children: [{ id: ids.WorkEffort }],
  },
  {
    title: 'HR',
    icon: 'group',
    children: [{ id: ids.PositionType }, { id: ids.PositionTypeRate }],
  },
  {
    title: 'Accounting',
    icon: 'money',
    children: [{ id: ids.ExchangeRate }],
  },
];
