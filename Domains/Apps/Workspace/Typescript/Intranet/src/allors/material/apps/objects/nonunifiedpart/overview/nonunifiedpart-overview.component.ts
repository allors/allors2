import { Component, OnDestroy, Self, Injector, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { Part } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './nonunifiedpart-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class NonUnifiedPartOverviewComponent implements AfterViewInit, OnDestroy {

  title = 'Part';

  part: Part;

  subscription: Subscription;
  serialised: boolean;

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

    // const { pull, x } = this.metaService;

    // const navRoute = new NavigationActivatedRoute(this.route);
    // const id = navRoute.id();

    // const pulls = [
    //   pull.Part({
    //     object: id,
    //     include: {
    //       InventoryItemKind: x
    //     }
    //   }),
    // ];

    // this.panelManager.onPull(pulls);

    // this.panelManager.context
    //   .load('Pull', new PullRequest({ pulls }))
    //   .subscribe((loaded) => {
    //     this.part = loaded.objects.Part as Part;
    //     this.serialised = this.part.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase();
    //   }, this.errorService.handler);
  }

  public ngAfterViewInit(): void {

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.objectType = m.Part;
          this.panelManager.id = navRoute.id();
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.NonUnifiedPart({
              object: this.panelManager.id,
              include: {
                InventoryItemKind: x
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

        this.part = loaded.objects.NonUnifiedPart as Part;
        this.serialised = this.part.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase();

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
