import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { MetaService, RefreshService, NavigationService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, PartyContactMechanism, Party, Person, OrganisationContactRelationship, WorkTask, WorkEffortState, Priority, WorkEffortPurpose, ContactMechanism, WorkEffort } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { Filters, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { ISessionObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'worktask-overview-detail',
  templateUrl: './worktask-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class WorkTaskOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  workTask: WorkTask;
  party: Party;
  workEffortStates: WorkEffortState[];
  priorities: Priority[];
  workEffortPurposes: WorkEffortPurpose[];
  employees: Person[];
  contactMechanisms: ContactMechanism[];
  contacts: Person[];
  addContactPerson = false;
  addContactMechanism: boolean;

  private subscription: Subscription;
  workEfforts: WorkEffort[];
  customersFilter: SearchFactory;
  subContractorsFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
    ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'WorkTask Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.WorkTask.name}`;

    panel.onPull = (pulls) => {

      this.workTask = undefined;

      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.WorkTask({
            name: pullName,
            object: id,
            include: {
              WorkEffortState: x,
              FullfillContactMechanism: x,
              ContactPerson: x,
              CreatedBy: x,
              ElectronicDocuments: x,
            }
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.workTask = loaded.objects[pullName] as WorkTask;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.workTask = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.WorkTask({
              object: id,
              include: {
                WorkEffortState: x,
                FullfillContactMechanism: x,
                Priority: x,
                WorkEffortPurposes: x,
                Customer: x,
                ExecutedBy: x,
                ContactPerson: x,
                CreatedBy: x,
              }
            }),
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
            pull.WorkEffortState({
              sort: new Sort(m.WorkEffortState.Name)
            }),
            pull.Priority({
              predicate: new Equals({ propertyType: m.Priority.IsActive, value: true }),
              sort: new Sort(m.Priority.Name),
            }),
            pull.WorkEffortPurpose({
              predicate: new Equals({ propertyType: this.m.WorkEffortPurpose.IsActive, value: true }),
              sort: new Sort(m.WorkEffortPurpose.Name),
            })
          ];

          this.customersFilter = Filters.customersFilter(m, this.internalOrganisationId.value);
          this.subContractorsFilter = Filters.subContractorsFilter(m, this.internalOrganisationId.value);

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const internalOrganisation: Organisation = loaded.objects.InternalOrganisation as Organisation;
        this.employees = internalOrganisation.ActiveEmployees;

        this.workTask = loaded.objects.WorkTask as WorkTask;
        this.workEffortStates = loaded.collections.WorkEffortStates as WorkEffortState[];
        this.priorities = loaded.collections.Priorities as Priority[];
        this.workEffortPurposes = loaded.collections.WorkEffortPurposes as WorkEffortPurpose[];

        this.updateCustomer(this.workTask.Customer);

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public contactPersonAdded(contact: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.workTask.Customer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.workTask.ContactPerson = contact;
  }

  public contactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.workTask.Customer.AddPartyContactMechanism(partyContactMechanism);
    this.workTask.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public customerSelected(customer: ISessionObject) {
    this.updateCustomer(customer as Party);
  }

  private updateCustomer(party: Party) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_Country: x
              }
            }
          }
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          WorkEffortsWhereCustomer: {
            include: {
              WorkEffortState: x
            }
          }
        }
      })
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe(
      (loaded) => {
        this.workEfforts = loaded.collections.WorkEfforts as WorkEffort[];
        const indexMyself = this.workEfforts.indexOf(this.workTask, 0);
        if (indexMyself > -1) {
          this.workEfforts.splice(indexMyself, 1);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];

        this.contactMechanisms = partyContactMechanisms
          .map((v: PartyContactMechanism) => v.ContactMechanism);

        this.contacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
        this.panel.toggle();
      },
        this.saveService.errorHandler
      );
  }
}
