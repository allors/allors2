import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, SessionService, NavigationService, MetaService } from '../../../../../angular';
import { ContactMechanism, InternalOrganisation, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, Priority, Singleton, WorkEffortPartyAssignment, WorkEffortPurpose, WorkEffortState, WorkTask } from '../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { switchMap, map } from 'rxjs/operators';
import { Title } from '@angular/platform-browser';

@Component({
  templateUrl: './worktask-edit.component.html',
  providers: [SessionService]
})
export class WorkTaskEditComponent implements OnInit, OnDestroy {
  m: MetaDomain;

  title = 'Work Task';
  add: boolean;
  edit: boolean;

  party: Party;

  workTask: WorkTask;

  workEffortStates: WorkEffortState[];
  priorities: Priority[];
  workEffortPurposes: WorkEffortPurpose[];
  singleton: Singleton;
  employees: Person[];
  workEffortPartyAssignments: WorkEffortPartyAssignment[];
  assignees: Party[] = [];
  existingAssignees: Party[] = [];
  contactMechanisms: ContactMechanism[];
  contacts: Person[];
  addContactPerson = false;
  addContactMechanism: boolean;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: SessionService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private titleService: Title,
    public stateService: StateService
  ) {
    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
    this.titleService.setTitle(this.title);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(
      this.route.url,
      this.refresh$,
      this.stateService.internalOrganisationId$
    )
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {
          const id: string = this.route.snapshot.paramMap.get('id');

          const add = !id;

          let pulls = [
            this.fetcher.internalOrganisation,

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

          if (!add) {
            pulls = [
              ...pulls,
              pull.WorkTask({
                object: id,
                include: {
                  FullfillContactMechanism: x,
                  ContactPerson: x,
                  CreatedBy: x,
                }
              }),
              pull.WorkTask({
                object: id,
                fetch: {
                  WorkEffortPartyAssignmentsWhereAssignment: {
                    include: {
                      Party: x,
                      Assignment: x,
                    }
                  }
                }
              })
            ];
          }

          return this.allors
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {
        this.workEffortStates = loaded.collections.WorkEffortStates as WorkEffortState[];
        this.priorities = loaded.collections.Priorities as Priority[];
        this.workEffortPurposes = loaded.collections.WorkEffortPurposes as WorkEffortPurpose[];
        const internalOrganisation: Organisation = loaded.objects.InternalOrganisation as Organisation;
        this.employees = internalOrganisation.ActiveEmployees;

        if (add) {
          this.workTask = this.allors.session.create('WorkTask') as WorkTask;
          this.workTask.TakenBy = internalOrganisation;

        } else {
          this.workTask = loaded.objects.WorkTask as WorkTask;

          this.workEffortPartyAssignments = loaded.collections.WorkEffortPartyAssignments as WorkEffortPartyAssignment[];
          this.assignees = this.workEffortPartyAssignments.map((v: WorkEffortPartyAssignment) => v.Party);
        }

        this.existingAssignees = this.assignees;

      }, this.errorService.handler,
      );
  }

  public customerSelected(customer: Party) {
    this.updateCustomer(customer);
  }

  public contactPersonAdded(id: string): void {

    this.addContactPerson = false;

    const contact: Person = this.allors.session.get(id) as Person;

    const organisationContactRelationship = this.allors.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.workTask.Customer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.workTask.ContactPerson = contact;
  }

  public contactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addContactMechanism = false;

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.workTask.Customer.AddPartyContactMechanism(partyContactMechanism);
    this.workTask.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public contactMechanismCancelled() {
    this.addContactMechanism = false;
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    if (this.assignees) {
      this.assignees.forEach((assignee: Person) => {
        if (this.existingAssignees.indexOf(assignee) < 0) {
          const workEffortAssignment: WorkEffortPartyAssignment = this.allors.session.create(
            'WorkEffortAssignment',
          ) as WorkEffortPartyAssignment;
          workEffortAssignment.Assignment = this.workTask;
          workEffortAssignment.Party = assignee;
        }
      });
    }
    this.allors.save().subscribe(
      (saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.handle(error);
      },
    );
  }

  public goBack(): void {
    window.history.back();
  }

  private updateCustomer(party: Party) {

    const { m, pull, x } = this.metaService;

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

    this.allors.load('Pull', new PullRequest({ pulls })).subscribe(
      (loaded) => {
        const partyContactMechanisms: PartyContactMechanism[] = loaded
          .collections.partyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map(
          (v: PartyContactMechanism) => v.ContactMechanism,
        );
        this.contacts = loaded.collections.currentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
  }
}
