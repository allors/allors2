import { Component, OnDestroy, Self, Injector, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, tap, delay } from 'rxjs/operators';

import {  NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { Person, Employment } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './person-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class PersonOverviewComponent implements AfterViewInit, OnDestroy {

  title = 'Person';

  person: Person;
  employee: boolean;

  subscription: Subscription;

  constructor(
    @Self() public panelManager: PanelManagerService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    
    private route: ActivatedRoute,
    private stateService: StateService,
    public injector: Injector,
    titleService: Title,
  ) {

    titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const { pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();

          const pulls = [
            pull.Person({
              object: id,
              fetch: {
                EmploymentsWhereEmployee: x
              }
            }),
          ];

          return this.panelManager.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              tap((loaded) => {
                const employments = loaded.collections.Employments as Employment[];
                this.employee = employments.length > 0;
              }),
              delay(1)
            );
        }),
        switchMap(([]) => {

          const { m, pull } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.objectType = m.Person;
          this.panelManager.id = navRoute.id();
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.Person({
              object: this.panelManager.id,
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

        this.person = loaded.objects.Person as Person;
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
