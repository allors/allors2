import { Component, OnDestroy, OnInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { Good, SalesOrder, SalesInvoice, RepeatingSalesInvoice } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './salesinvoice-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class SalesInvoiceOverviewComponent implements OnInit, OnDestroy {

  title = 'Sales Invoice';

  public order: SalesOrder;
  public invoice: SalesInvoice;
  public repeatingInvoices: RepeatingSalesInvoice[];
  public repeatingInvoice: RepeatingSalesInvoice;
  public goods: Good[] = [];

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
        switchMap(([]) => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.Organisation;
          this.panelManager.expanded = navRoute.panel();

          const { id } = this.panelManager;

          const pulls = [
            pull.SalesInvoice({
              object: id,
              include: {
                SalesInvoiceItems: {
                  Product: x,
                  InvoiceItemType: x,
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
                SalesInvoiceState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                SalesOrder: x,
                BillToContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x
                  }
                },
                ShipToAddress: {
                  PostalBoundary: {
                    Country: x
                  }
                },
                BillToEndCustomerContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x
                  }
                },
                ShipToEndCustomerAddress: {
                  PostalBoundary: {
                    Country: x
                  }
                }
              }
            }),
            pull.SalesInvoice({
              object: id,
              fetch: {
                SalesOrder: x
              }
            }),
            pull.Good(
              {
                sort: new Sort(m.Good.Name),
              }),
            pull.RepeatingSalesInvoice(
              {
                predicate: new Equals({ propertyType: m.RepeatingSalesInvoice.Source, object: id }),
                include: {
                  Frequency: x,
                  DayOfWeek: x
                }
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

        this.goods = loaded.collections.Goods as Good[];
        this.order = loaded.objects.SalesOrder as SalesOrder;
        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
        this.repeatingInvoices = loaded.collections.RepeatingSalesInvoices as RepeatingSalesInvoice[];
        if (this.repeatingInvoices.length > 0) {
          this.repeatingInvoice = this.repeatingInvoices[0];
        } else {
          this.repeatingInvoice = undefined;
        }

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
