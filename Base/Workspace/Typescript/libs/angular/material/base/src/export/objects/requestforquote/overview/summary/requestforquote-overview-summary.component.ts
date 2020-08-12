import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, SalesOrder, RepeatingSalesInvoice, Good, WorkEffort, PurchaseOrder, PurchaseInvoice, Shipment, NonUnifiedGood, BasePrice, PriceComponent, ProductIdentificationType, SerialisedItem, RequestForQuote, ProductQuote, CustomerShipment, Quote } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'requestforquote-overview-summary',
  templateUrl: './requestforquote-overview-summary.component.html',
  providers: [PanelService]
})
export class RequestForQuoteOverviewSummaryComponent {

  m: Meta;

  requestForQuote: RequestForQuote;
  quote: Quote;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public snackBar: MatSnackBar,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const requestForQuotePullName = `${panel.name}_${this.m.RequestForQuote.name}`;
    const productQuotePullName = `${panel.name}_${this.m.ProductQuote.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      pulls.push(
        pull.RequestForQuote(
          {
            name: requestForQuotePullName,
            object: this.panel.manager.id,
            include: {
              FullfillContactMechanism: {
                                  PostalAddress_Country: x

              },
              RequestItems: {
                Product: x,
              },
              Originator: x,
              ContactPerson: x,
              RequestState: x,
              Currency: x,
              CreatedBy: x,
              LastModifiedBy: x,
            }
          }),
        pull.RequestForQuote(
          {
            name: productQuotePullName,
            object: this.panel.manager.id,
            fetch: {
              QuoteWhereRequest: x
            }
          }
        )
      );
    };

    panel.onPulled = (loaded) => {
      this.requestForQuote = loaded.objects[requestForQuotePullName] as RequestForQuote;
      this.quote = loaded.objects.Quote as Quote;
    };
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Reject)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public submit(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Submit)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public hold(): void {

    this.panel.manager.context.invoke(this.requestForQuote.Hold)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public createQuote(): void {

    this.panel.manager.context.invoke(this.requestForQuote.CreateQuote)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully created a quote.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }
}

