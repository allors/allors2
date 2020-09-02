import { Component, Self, AfterViewInit, OnDestroy, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { MetaService, RefreshService,  NavigationService, PanelManagerService, ContextService } from '@allors/angular/services/core';
import { RequestForQuote, Quote } from '@allors/domain/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { NavigationActivatedRoute, TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './requestforquote-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class RequestForQuoteOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  title = 'Request For Quote';

  public requestForQuote: RequestForQuote;
  public quote: Quote;

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
          this.panelManager.objectType = m.RequestForQuote;
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.RequestForQuote(
              {
                object: this.panelManager.id,
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
                object: this.panelManager.id,
                fetch: {
                  QuoteWhereRequest: x
                }
              }
            )
          ];

          this.panelManager.onPull(pulls);

          return this.panelManager.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelManager.context.session.reset();

        this.panelManager.onPulled(loaded);

        this.requestForQuote = loaded.objects.RequestForQuote as RequestForQuote;
        this.quote = loaded.objects.Quote as Quote;

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
