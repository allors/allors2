import { Component, OnDestroy, Self, Injector, AfterViewInit } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import {  NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService, InternalOrganisationId } from '../../../../../angular';
import { WorkTask } from '../../../../../domain';
import { PullRequest, Pull } from '../../../../../framework';

@Component({
  templateUrl: './worktask-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class WorkTaskOverviewComponent implements AfterViewInit, OnDestroy {

  readonly m: Meta;
  title = 'WorkTask';

  workTask: WorkTask;

  subscription: Subscription;

  constructor(
    @Self() public panelManager: PanelManagerService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private route: ActivatedRoute,
    public injector: Injector,
    private internalOrganistationId: InternalOrganisationId,
    titleService: Title,
  ) {

    titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.internalOrganistationId.observable$)
      .pipe(
        switchMap(([urlSegments, queryParams, date, internalOrganisationId]) => {

          const { m, pull } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.objectType = m.WorkTask;
          this.panelManager.id = navRoute.id();
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.WorkTask({
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

        this.workTask = loaded.objects.WorkTask as WorkTask;
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
