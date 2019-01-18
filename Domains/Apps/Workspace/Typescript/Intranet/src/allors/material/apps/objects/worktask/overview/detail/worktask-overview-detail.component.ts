import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, NavigationService, PanelService, RefreshService, MetaService } from '../../../../../../angular';
import { WorkTask, Party, WorkEffortState, Priority, WorkEffortPurpose, Person, ContactMechanism, Organisation, PartyContactMechanism, OrganisationContactRelationship } from '../../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'worktask-overview-detail',
  templateUrl: './worktask-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class WorkTaskOverviewDetailComponent implements OnInit, OnDestroy {

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

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    public stateService: StateService) {

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'WorkTask Data';
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
          const fetcher = new Fetcher(this.stateService, this.metaService.pull);
          const id = this.panel.manager.id;

          const pulls = [
            fetcher.internalOrganisation,
            pull.WorkTask({
              object: id,
              include: {
                WorkEffortState: x,
                FullfillContactMechanism: x,
                Priority: x,
                WorkEffortPurposes: x,
                Customer: x,
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

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
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

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public contactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

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

  public customerSelected(customer: Party) {
    this.updateCustomer(customer);
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
                PostalAddress_PostalBoundary: {
                  Country: x,
                }
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
      })
    ];

    this.allors.context.load('Pull', new PullRequest({ pulls })).subscribe(
      (loaded) => {
        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];

        this.contactMechanisms = partyContactMechanisms
          .map((v: PartyContactMechanism) => v.ContactMechanism);

        this.contacts = loaded.collections.CurrentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.handle(error);
      },
    );
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.panel.toggle();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
