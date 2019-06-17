import { Component, OnDestroy, Self, Injector, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, tap, delay, map } from 'rxjs/operators';

import { NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { Part, NonUnifiedPart } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';

@Component({
  templateUrl: './nonunifiedpart-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class NonUnifiedPartOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  title = 'Part';

  part: Part;

  subscription: Subscription;
  serialised: boolean;

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
        switchMap(([, ]) => {

          const { pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();

          const pulls = [
            pull.NonUnifiedPart({
              object: id,
              include: {
                InventoryItemKind: x
              }
            }),
          ];

          return this.panelManager.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              tap((loaded) => {
                const part = loaded.objects.NonUnifiedPart as NonUnifiedPart;
                this.serialised = part.InventoryItemKind.UniqueId === '2596e2dd3f5d4588a4a2167d6fbe3fae';
              }),
              delay(1)
            );
        }),
        switchMap(([, ]) => {

          const { m } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.objectType = m.Part;
          this.panelManager.id = navRoute.id();
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [];
          this.panelManager.onPull(pulls);

          return this.panelManager.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.panelManager.context.session.reset();
        this.panelManager.onPulled(loaded);

        this.part = loaded.objects.Part as Part;

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
