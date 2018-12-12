import { Component, OnDestroy, OnInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { ProductQuote, Quote, Good, SalesOrder } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './productquote-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class ProductQuoteOverviewComponent implements OnInit, OnDestroy {

  title = 'Quote';

  public productQuote: ProductQuote;
  public goods: Good[] = [];
  public salesOrder: SalesOrder;

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
          this.panelManager.objectType = m.Organisation.objectType;
          this.panelManager.expanded = navRoute.panel();

          const pulls = [
            pull.ProductQuote(
              {
                object: this.panelManager.id,
                include: {
                  QuoteItems: {
                    Product: x,
                    QuoteItemState: x,
                  },
                  Receiver: x,
                  ContactPerson: x,
                  QuoteState: x,
                  CreatedBy: x,
                  LastModifiedBy: x,
                  Request: x,
                  FullfillContactMechanism: {
                    PostalAddress_PostalBoundary: {
                      Country: x,
                    }
                  }
                }
              }),
            pull.ProductQuote(
              {
                object: this.panelManager.id,
                fetch: {
                  SalesOrderWhereQuote: x,
                }
              }
            )
          ];

          this.panelManager.onPull(pulls);

          return this.panelManager.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelManager.context.session.reset();

        this.panelManager.onPulled(loaded);

        this.productQuote = loaded.objects.ProductQuote as ProductQuote;
        this.goods = loaded.collections.Goods as Good[];
        this.salesOrder = loaded.objects.SalesOrder as SalesOrder;

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
