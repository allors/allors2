import { Component, OnDestroy, OnInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, SessionService, NavigationService, NavigationActivatedRoute, AllorsPanelsService, AllorsRefreshService } from '../../../../../angular';
import { Person } from '../../../../../domain';
import { PullRequest, Pull } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './person-overview.component.html',
  providers: [SessionService, AllorsPanelsService]
})
export class PersonOverviewComponent implements OnInit, OnDestroy {

  title = 'Person Overview';

  person: Person;

  subscription: Subscription;

  constructor(
    @Self() private allors: SessionService,
    @Self() public panelsService: AllorsPanelsService,
    public refreshService: AllorsRefreshService,
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

          const { pull } = this.allors;

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const panel = navRoute.panel();

          this.panelsService.id = id;
          this.panelsService.maximized = panel;

          const pulls: Pull[] = [];
          this.panelsService.prePull(pulls);

          pulls.push(
            pull.Person({
              object: id,
            })
          );

          return this.allors
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();

        this.panelsService.postPull(loaded);

        this.person = loaded.objects.Person as Person;

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
