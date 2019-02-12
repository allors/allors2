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
  m.NonUnifiedGood.overview = '/products/nonunifiedgood/:id';
  m.Part.list = '/products/parts';
  m.NonUnifiedPart.overview = '/products/nonunifiedpart/:id';
  m.Catalogue.list = '/products/catalogues';
  m.ProductCategory.list = '/products/productcategories';
  m.SerialisedItemCharacteristic.list = '/products/serialiseditemcharacteristics';
  m.ProductType.list = '/products/producttypes';
  m.SerialisedItem.list = '/products/serialiseditems';
  m.SerialisedItem.overview = '/products/serialisedItem/:id';
  m.UnifiedGood.list = '/products/unifiedgoods';
  m.UnifiedGood.overview = '/products/unifiedgood/:id';

  m.SalesInvoice.list = '/sales/salesinvoices';
  m.SalesInvoice.overview = '/sales/salesinvoice/:id';

  m.PurchaseInvoice.list = '/purchasing/purchaseinvoices';
  m.PurchaseInvoice.overview = '/purchasing/purchaseinvoice/:id';

  m.WorkEffort.list = '/workefforts/workefforts';
  m.WorkTask.overview = '/workefforts/worktask/:id';
}
