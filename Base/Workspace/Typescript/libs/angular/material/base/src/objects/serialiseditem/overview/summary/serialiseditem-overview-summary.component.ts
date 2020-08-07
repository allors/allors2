import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, SalesOrder, RepeatingSalesInvoice, Good, WorkEffort, PurchaseOrder, PurchaseInvoice, Shipment, NonUnifiedGood, BasePrice, PriceComponent, ProductIdentificationType, SerialisedItem, RequestForQuote, ProductQuote, CustomerShipment } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';
import { PrintService } from '../../../../services/actions';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-overview-summary',
  templateUrl: './serialiseditem-overview-summary.component.html',
  providers: [PanelService]
})
export class SerialisedItemOverviewSummaryComponent {

  m: Meta;

  serialisedItem: SerialisedItem;
  part: Part;
  request: RequestForQuote;
  quote: ProductQuote;
  order: SalesOrder;
  shipment: CustomerShipment;
  invoice: SalesInvoice;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const serialisedItemPullName = `${panel.name}_${this.m.SerialisedItem.name}`;
    const partPullName = `${panel.name}_${this.m.Part.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.SerialisedItem({
          name: serialisedItemPullName,
          object: id,
          include: {
            SerialisedItemState: x,
            OwnedBy: x,
            RentedBy: x,
          }
        }),
        pull.SerialisedItem({
          name: partPullName,
          object: id,
          fetch: {
            PartWhereSerialisedItem: x
          }
        }),
        pull.SerialisedItem({
          object: id,
          fetch: {
            RequestItemsWhereSerialisedItem: {
              RequestWhereRequestItem: x
            }
          }
        }),
        pull.SerialisedItem({
          object: id,
          fetch: {
            QuoteItemsWhereSerialisedItem: {
              QuoteWhereQuoteItem: x
            }
          }
        }),
        pull.SerialisedItem({
          object: id,
          fetch: {
            SalesOrderItemsWhereSerialisedItem: {
              SalesOrderWhereSalesOrderItem: x
            }
          }
        }),
        pull.SerialisedItem({
          object: id,
          fetch: {
            ShipmentItemsWhereSerialisedItem: {
              ShipmentWhereShipmentItem: x
            }
          }
        }),
        pull.SerialisedItem({
          object: id,
          fetch: {
            SalesInvoiceItemsWhereSerialisedItem: {
              SalesInvoiceWhereSalesInvoiceItem: x
            }
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.serialisedItem = loaded.objects[serialisedItemPullName] as SerialisedItem;
      this.part = loaded.objects[partPullName] as Part;

      const requests = loaded.collections.Requests as RequestForQuote[] || [];
      if (requests.length > 0) {
        this.request = requests.reduce(function (a, b) { return a.RequestDate > b.RequestDate ? a : b; });
      }

      const quotes = loaded.collections.Quotes as ProductQuote[] || [];
      if (quotes.length > 0) {
        this.quote = quotes.reduce(function (a, b) { return a.IssueDate > b.IssueDate ? a : b; });
      }

      const orders = loaded.collections.SalesOrders as SalesOrder[] || [];
      if (orders.length > 0) {
        this.order = orders.reduce(function (a, b) { return a.OrderDate > b.OrderDate ? a : b; });
      }

      const shipments = loaded.collections.Shipments as CustomerShipment[] || [];
      if (shipments.length > 0) {
        this.shipment = shipments.reduce(function (a, b) { return a.EstimatedShipDate > b.EstimatedShipDate ? a : b; });
      }

      const invoices = loaded.collections.SalesInvoices as SalesInvoice[] || [];
      if (invoices.length > 0) {
        this.invoice = invoices.reduce(function (a, b) { return a.InvoiceDate > b.InvoiceDate ? a : b; });
      }
    };
  }
}
