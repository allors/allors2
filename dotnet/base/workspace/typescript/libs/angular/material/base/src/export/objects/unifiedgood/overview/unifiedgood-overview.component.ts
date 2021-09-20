import { Component, Self, AfterViewInit, OnDestroy, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { TestScope,  NavigationActivatedRoute } from '@allors/angular/core';
import { Good } from '@allors/domain/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { UnifiedGood } from 'libs/domain/generated/src/UnifiedGood.g';
import { PanelManagerService, ContextService, RefreshService, NavigationService, MetaService } from '@allors/angular/services/core';

@Component({
  templateUrl: './unifiedgood-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class UnifiedGoodOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

  title = 'Good';

  good: Good;

  subscription: Subscription;
  serialised: boolean;

  constructor(
    @Self() public panelManager: PanelManagerService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private route: ActivatedRoute,
    private internalOrganisationId: InternalOrganisationId,
    public injector: Injector,
    titleService: Title,
  ) {
    super();

    titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const { pull, x, m } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.UnifiedGood;
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.UnifiedGood({
              object: this.panelManager.id,
              include: {
                InventoryItemKind: x
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

        const unifiedGood = loaded.objects.UnifiedGood as UnifiedGood;
        this.serialised = unifiedGood.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
