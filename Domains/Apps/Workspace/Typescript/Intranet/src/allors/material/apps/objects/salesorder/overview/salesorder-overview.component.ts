import { Component, OnDestroy, OnInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { ProductQuote, Quote, Good, SalesOrder, SalesOrderItem, SalesInvoice, BillingProcess, SerialisedInventoryItemState } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './salesorder-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class SalesOrderOverviewComponent implements OnInit, OnDestroy {

  title = 'Quote';

  public quote: ProductQuote;
  public order: SalesOrder;
  public orderItems: SalesOrderItem[] = [];
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
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService,
    public injector: Injector,
    titleService: Title,
  ) {

    titleService.setTitle(this.title);
  }

  public ngOnInit(): void {

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, queryParams, date, internalOrganisationId]) => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.Organisation;
          this.panelManager.expanded = navRoute.panel();

          const pulls = [
            pull.SalesOrder({
              object: this.panelManager.id,
              include: {
                SalesOrderItems: {
                  Product: x,
                  InvoiceItemType: x,
                  SalesOrderItemState: x,
                  SalesOrderItemShipmentState: x,
                  SalesOrderItemPaymentState: x,
                  SalesOrderItemInvoiceState: x,
                },
                SalesTerms: {
                  TermType: x,
                },
                BillToCustomer: x,
                BillToContactPerson: x,
                ShipToCustomer: x,
                ShipToContactPerson: x,
                ShipToEndCustomer: x,
                ShipToEndCustomerContactPerson: x,
                BillToEndCustomer: x,
                BillToEndCustomerContactPerson: x,
                SalesOrderState: x,
                SalesOrderShipmentState: x,
                SalesOrderInvoiceState: x,
                SalesOrderPaymentState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                Quote: x,
                ShipToAddress: {
                  PostalBoundary: {
                    Country: x,
                  }
                },
                BillToEndCustomerContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                },
                ShipToEndCustomerAddress: {
                  PostalBoundary: {
                    Country: x,
                  }
                },
                BillToContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }),
            pull.SalesOrder({
              object: this.panelManager.id,
              fetch: { SalesInvoicesWhereSalesOrder: x }
            }),
            pull.Good({ sort: new Sort(m.Good.Name) }),
            pull.BillingProcess({ sort: new Sort(m.BillingProcess.Name) }),
            pull.SerialisedInventoryItemState({
              predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name)
            }),
          ];

          this.panelManager.onPull(pulls);

          return this.panelManager.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelManager.context.session.reset();

        this.panelManager.onPulled(loaded);

        this.order = loaded.objects.SalesOrder as SalesOrder;
        this.goods = loaded.collections.Goods as Good[];
        this.salesInvoice = loaded.objects.SalesInvoice as SalesInvoice;
        this.inventoryItemStates = loaded.collections.SerialisedInventoryItemStates as SerialisedInventoryItemState[];
        this.billingProcesses = loaded.collections.BillingProcesses as BillingProcess[];
        this.billingForOrderItems = this.billingProcesses.find((v: BillingProcess) => v.UniqueId.toUpperCase() === 'AB01CCC2-6480-4FC0-B20E-265AFD41FAE2');
        this.orderItems = this.order.SalesOrderItems;

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
