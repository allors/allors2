import { Component, Self, AfterViewInit, OnDestroy, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { MetaService, RefreshService,  NavigationService, PanelManagerService, ContextService } from '@allors/angular/services/core';
import { Good, SalesInvoice, RepeatingSalesInvoice } from '@allors/domain/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { NavigationActivatedRoute, TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './salesinvoice-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class SalesInvoiceOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  title = 'Sales Invoice';

  public invoice: SalesInvoice;
  public repeatingInvoices: RepeatingSalesInvoice[];
  public repeatingInvoice: RepeatingSalesInvoice;

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

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.SalesInvoice;
          this.panelManager.expanded = navRoute.panel();

          const { id } = this.panelManager;

          this.panelManager.on();

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
                DerivedBillToContactMechanism: {
                  PostalAddress_Country: x
                },
                DerivedShipToAddress: {
                  Country: x
                },
                DerivedBillToEndCustomerContactMechanism: {
                  PostalAddress_Country: x
                },
                DerivedShipToEndCustomerAddress: {
                  Country: x
                }
              }
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
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelManager.context.session.reset();

        this.panelManager.onPulled(loaded);

        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
        this.repeatingInvoices = loaded.collections.RepeatingSalesInvoices as RepeatingSalesInvoice[];
        if (this.repeatingInvoices.length > 0) {
          this.repeatingInvoice = this.repeatingInvoices[0];
        } else {
          this.repeatingInvoice = undefined;
        }

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
