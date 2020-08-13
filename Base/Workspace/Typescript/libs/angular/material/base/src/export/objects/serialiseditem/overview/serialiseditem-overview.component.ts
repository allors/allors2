import { Component, OnInit, Self, HostBinding, AfterViewInit, OnDestroy, Injector, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { formatDistance, format, isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, Action, NavigationService, PanelService, PanelManagerService, ContextService, NavigationActivatedRoute, ActionTarget } from '@allors/angular/core';
import { CommunicationEvent, ContactMechanism, CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, InventoryItem, NonSerialisedInventoryItem, Part, NonUnifiedPart, Organisation, SupplierOffering, PartyContactMechanism, PartyRate, ProductIdentification, PurchaseInvoiceItem, PurchaseInvoice, PurchaseOrderItem, PurchaseOrder, QuoteItem, ProductQuote, RepeatingSalesInvoice, RequestForQuote, Quote, SalesInvoiceItem, SalesOrder, SalesOrderItem, SalesTerm, SerialisedItem, Party } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService, ObjectData, ObjectService, OverviewService, MethodService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { RoleType } from '@allors/meta/system';
import { Pull, Fetch, Step, Sort, Equals } from '@allors/data/system';

@Component({
  templateUrl: './serialiseditem-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class SerialisedItemOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  readonly m: Meta;
  title = 'Asset';

  serialisedItem: SerialisedItem;

  subscription: Subscription;
  part: Part;
  owner: Party;

  constructor(
    @Self() public panelManager: PanelManagerService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private route: ActivatedRoute,
    public injector: Injector,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title,
  ) {
    super();

    this.m = this.metaService.m;
    titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.objectType = m.SerialisedItem;
          this.panelManager.id = navRoute.id();
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.SerialisedItem({
              object: this.panelManager.id,
              include: {
                OwnedBy: x,
              }
            }),
          ];

          this.panelManager.onPull(pulls);

          return this.panelManager.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelManager.context.session.reset();
        this.panelManager.onPulled(loaded);

        this.serialisedItem = loaded.objects.SerialisedItem as SerialisedItem;
        this.owner = this.serialisedItem.OwnedBy;
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
