import { Component, OnDestroy, OnInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, NavigationService, NavigationActivatedRoute, PanelContainerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { Organisation } from '../../../../../domain';
import { PullRequest, Pull } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './organisation-overview.component.html',
  providers: [PanelContainerService, ContextService]
})
export class OrganisationDetailComponent implements OnInit, OnDestroy {

  title = 'Organisation';

  organisation: Organisation;
  subscription: Subscription;

  constructor(
    @Self() public panelsService: PanelContainerService,
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

          const { pull } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelsService.id = navRoute.id();
          this.panelsService.maximized = navRoute.panel();

          const pulls = [
            pull.Organisation({
              object: this.panelsService.id,
            })
          ];

          this.panelsService.onPull(pulls);

          return this.panelsService.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelsService.context.session.reset();
        this.panelsService.onPulled(loaded);

        this.organisation = loaded.objects.Organisation as Organisation;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
