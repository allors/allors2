import { Component, OnInit, Self, HostBinding, AfterViewInit, OnDestroy, Injector } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { formatDistance, format, isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, Action, NavigationService, PanelService, PanelManagerService, ContextService, NavigationActivatedRoute, ActionTarget } from '@allors/angular/services/core';
import { CommunicationEvent, ContactMechanism, CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, InventoryItem, NonSerialisedInventoryItem, Part, NonUnifiedPart, Organisation, SupplierOffering, PartyContactMechanism, PartyRate, Person, Employment } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService, ObjectData, ObjectService, OverviewService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';

@Component({
  templateUrl: './person-overview.component.html',
  providers: [PanelManagerService, ContextService]
})
export class PersonOverviewComponent extends TestScope implements AfterViewInit, OnDestroy {

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
        switchMap(([urlSegments, queryParams, date, internalOrganisationId]) => {

          const { m, pull, x } = this.metaService;

          const navRoute = new NavigationActivatedRoute(this.route);
          this.panelManager.id = navRoute.id();
          this.panelManager.objectType = m.Person;
          this.panelManager.expanded = navRoute.panel();

          this.panelManager.on();

          const pulls = [
            pull.Person({
              object: this.panelManager.id,
            }),
            pull.Person({
              object: this.panelManager.id,
              fetch: {
                EmploymentsWhereEmployee: x
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

        this.person = loaded.objects.Person as Person;
        const employments = loaded.collections.Employments as Employment[];
        this.employee = employments.length > 0;
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
