import { Component, Self, AfterViewInit, OnDestroy, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { MetaService, RefreshService,  NavigationService, PanelManagerService, ContextService,  } from '@allors/angular/services/core';
import { CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState } from '@allors/domain/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { NavigationActivatedRoute, TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './customershipment-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class CustomerShipmentOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  title = 'Customer Shipment';

  public shipment: CustomerShipment;
  public orderItems: ShipmentItem[] = [];
  public goods: Good[] = [];
  public salesInvoice: SalesInvoice;
  public billingProcesses: BillingProcess[];
  public billingForOrderItems: BillingProcess;
  public selectedSerialisedInventoryState: string;
  public inventoryItemStates: SerialisedInventoryItemState[];

  subscription: Subscription;

  constructor(
    @Self() public panelManager: PanelManagerService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private route: ActivatedRoute,
    public injector: Injector,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title,
  ) {
    super();

    titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {

    this.subscription = combineLatest([this.route.url, this.route.queryParams, this.refreshService.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(() => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.Shipment;
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.Shipment({
              object: this.panelManager.id,
              include: {
                ShipmentItems: {
                  Good: x,
                },
                ShipFromParty: x,
                ShipFromAddress: x,
                ShipToParty: x,
                ShipToContactPerson: x,
                ShipmentState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                ShipToAddress: {
                  Country: x,
                },
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

        this.shipment = loaded.objects.Shipment as CustomerShipment;
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
