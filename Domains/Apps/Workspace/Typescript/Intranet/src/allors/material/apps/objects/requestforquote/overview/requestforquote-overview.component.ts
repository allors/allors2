import { Component, OnDestroy, OnInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { Organisation, RequestForQuote, ProductQuote, InventoryItem, SerialisedInventoryItem, NonSerialisedInventoryItem, Quote } from '../../../../../domain';
import { PullRequest, Pull } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './requestforquote-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class RequestForQuoteOverviewComponent implements OnInit, OnDestroy {

  title = 'Request For Quote';

  public requestForQuote: RequestForQuote;
  public quote: Quote;

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
          this.panelManager.objectType = m.RequestForQuote.objectType;
          this.panelManager.expanded = navRoute.panel();

          const pulls = [
            pull.RequestForQuote(
              {
                object: this.panelManager.id,
                include: {
                  FullfillContactMechanism: {
                    PostalAddress_PostalBoundary: {
                      Country: x,
                    }
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
                object: this.panelManager.id,
                fetch: {
                  QuoteWhereRequest: x
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

        this.requestForQuote = loaded.objects.RequestForQuote as RequestForQuote;
        this.quote = loaded.objects.Quote as Quote;

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
