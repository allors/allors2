import { Component, OnDestroy, AfterViewInit, Self, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import {  NavigationService, NavigationActivatedRoute, PanelManagerService, RefreshService, MetaService, ContextService } from '../../../../../angular';
import { Good, PurchaseOrder, PurchaseOrderItem } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './purchaseorder-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class PurchaseOrderOverviewComponent implements AfterViewInit, OnDestroy {

  title = 'Purchase Order';

  public order: PurchaseOrder;
  public orderItems: PurchaseOrderItem[] = [];
  public goods: Good[] = [];

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

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.Organisation;
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.PurchaseOrder({
              object: this.panelManager.id,
              include: {
                PurchaseOrderItems: x,
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

        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        this.orderItems = this.order.PurchaseOrderItems;

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
