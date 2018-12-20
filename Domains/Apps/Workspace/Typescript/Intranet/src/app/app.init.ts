import { Meta } from '../allors/meta';
import { WorkspaceService } from '../allors/angular';

export function appInit(workspaceService: WorkspaceService) {

  const { metaPopulation } = workspaceService;
  const m = metaPopulation as Meta;

  m.Person.list = '/contacts/people';
  m.Person.overview = '/contacts/person/:id';
  m.Organisation.list = '/contacts/organisations';
  m.Organisation.overview = '/contacts/organisation/:id';
  m.CommunicationEvent.list = '/contacts/communicationevents';

  m.EmailCommunication.overview = '/contacts/emailcommunication/:id';
  m.FaceToFaceCommunication.overview = '/contacts/facetofacecommunication/:id';
  m.LetterCorrespondence.overview = '/contacts/lettercorrespondence/:id';
  m.PhoneCommunication.overview = '/contacts/phonecommunication/:id';

  m.RequestForQuote.list = '/sales/requestsforquote';
  m.RequestForQuote.overview = '/sales/requestforquote/:id';
  m.ProductQuote.list = '/sales/productquotes';
  m.ProductQuote.overview = '/sales/productquote/:id';
  m.SalesOrder.list = '/sales/salesorders';
  m.SalesOrder.overview = '/sales/salesorder/:id';

  m.Good.list = '/products/goods';
  m.Good.overview = '/products/good/:id';
  m.Part.list = '/products/parts';
  m.Part.overview = '/products/part/:id';
  m.Catalogue.list = '/products/catalogues';
  m.ProductCategory.list = '/products/productcategories';
  m.SerialisedItemCharacteristic.list = '/products/serialiseditemcharacteristics';
  m.ProductType.list = '/products/producttypes';

  m.PurchaseInvoice.list = '/accounting/purchaseinvoices';
  m.PurchaseInvoice.overview = '/accounting/purchaseinvoice/:id';
  m.SalesInvoice.list = '/accounting/salesinvoices';
  m.SalesInvoice.overview = '/accounting/salesinvoice/:id';

  m.WorkEffort.list = '/workefforts/workefforts';
  m.WorkEffort.overview = '/accounting/workeffort/:id';
}