import { Component, Self, AfterViewInit, OnDestroy, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { MetaService, RefreshService,  NavigationService, PanelManagerService, ContextService } from '@allors/angular/services/core';
import { Good, PurchaseOrder, PurchaseInvoice } from '@allors/domain/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort } from '@allors/data/system';
import { NavigationActivatedRoute, TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './purchaseinvoice-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class PurchaseInvoiceOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  title = 'Purchase Invoice';

  order: PurchaseOrder;
  invoice: PurchaseInvoice;

  subscription: Subscription;

  constructor(
    @Self() public panelManager: PanelManagerService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private route: ActivatedRoute,
    private internalOrganisationId: InternalOrganisationId,
    public injector: Injector,
    titleService: Title,
  ) {
    super();

    titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.PurchaseInvoice;
          this.panelManager.expanded = navRoute.panel();

          const { id } = this.panelManager;

          this.panelManager.on();

          const pulls = [
            pull.PurchaseInvoice({
              object: id,
              include: {
                PurchaseInvoiceItems: {
                  InvoiceItemType: x
                },
                BilledFrom: x,
                BilledFromContactPerson: x,
                BillToEndCustomer: x,
                BillToEndCustomerContactPerson: x,
                ShipToEndCustomer: x,
                ShipToEndCustomerContactPerson: x,
                PurchaseInvoiceState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                PurchaseOrders: x,
                DerivedBillToEndCustomerContactMechanism: {
                  PostalAddress_Country: {
                  }
                },
                DerivedShipToEndCustomerAddress: {
                  Country: x
                }
              },
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

        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
