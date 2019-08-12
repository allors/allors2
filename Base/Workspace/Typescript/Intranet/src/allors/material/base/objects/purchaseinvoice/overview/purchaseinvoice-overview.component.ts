import { Component, OnDestroy, AfterViewInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService, TestScope } from '../../../../../angular';
import { Good, PurchaseOrder, PurchaseInvoice } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { InternalOrganisationId } from '../../../../../angular/base/state';

@Component({
  templateUrl: './purchaseinvoice-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class PurchasInvoiceOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  title = 'Purchase Invoice';

  order: PurchaseOrder;
  invoice: PurchaseInvoice;
  goods: Good[] = [];

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
                BillToEndCustomerContactMechanism: {
                  PostalAddress_Country: {
                  }
                },
                ShipToEndCustomerAddress: {
                  Country: x
                }
              },
            }),
            pull.Good({
              sort: new Sort(m.Good.Name)
            })
          ];

          this.panelManager.onPull(pulls);

          return this.panelManager.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelManager.context.session.reset();

        this.panelManager.onPulled(loaded);

        this.goods = loaded.collections.Goods as Good[];
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
