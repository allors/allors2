import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import {
  ErrorService,
  FilterFactory,
  Loaded,
  Saved,
  Scope,
  WorkspaceService,
} from '../../../../../angular';
import {
  ContactMechanism,
  InternalOrganisation,
  Organisation,
  OrganisationContactRelationship,
  Party,
  PartyContactMechanism,
  Person,
  Priority,
  Singleton,
  WorkEffortAssignment,
  WorkEffortPurpose,
  WorkEffortState,
  WorkTask,
} from '../../../../../domain';
import {
  Fetch,
  Path,
  PullRequest,
  Query,
  TreeNode,
  Sort,
  Equals,
} from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './worktask.component.html',
})
export class WorkTaskEditComponent implements OnInit, OnDestroy {
  public title = 'Work Task';
  public subTitle: string;

  public m: MetaDomain;

  public workTask: WorkTask;

  public workEffortStates: WorkEffortState[];
  public priorities: Priority[];
  public workEffortPurposes: WorkEffortPurpose[];
  public singleton: Singleton;
  public employees: Person[];
  public workEffortAssignments: WorkEffortAssignment[];
  public assignees: Person[] = [];
  public existingAssignees: Person[] = [];
  public contactMechanisms: ContactMechanism[];
  public contacts: Person[];
  public addContactPerson = false;
  public addContactMechanism: boolean;
  public scope: Scope;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public stateService: StateService,
  ) {
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {
    this.subscription = Observable.combineLatest(
      this.route.url,
      this.refresh$,
      this.stateService.internalOrganisationId$,
    )
      .switchMap(([urlSegments, date, internalOrganisationId]) => {
        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.WorkTask.FullfillContactMechanism }),
              new TreeNode({ roleType: m.WorkTask.ContactPerson }),
              new TreeNode({ roleType: m.WorkTask.CreatedBy }),
            ],
            name: 'worktask',
          }),
        ];

        const queries: Query[] = [
          new Query({
            name: 'workEffortStates',
            objectType: this.m.WorkEffortState,
            sort: [
              new Sort({ roleType: m.WorkEffortState.Name, direction: 'Asc' }),
            ],
        }),
          new Query({
            name: 'priorities',
            objectType: this.m.Priority,
            predicate: new Equals({ roleType: m.Priority.IsActive, value: true }),
            sort: [
              new Sort({ roleType: m.Priority.Name, direction: 'Asc' }),
            ],
        }),
          new Query({
            name: 'workEffortPurposes',
            objectType: this.m.WorkEffortPurpose,
            predicate: new Equals({ roleType: this.m.WorkEffortPurpose.IsActive, value: true }),
            sort: [
              new Sort({ roleType: m.WorkEffortPurpose.Name, direction: 'Asc' }),
            ],
        }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }))
          .switchMap((loaded) => {
            this.subTitle = 'edit work task';
            this.workTask = loaded.objects.worktask as WorkTask;

            const addMode: boolean = !this.workTask;

            if (addMode) {
              this.subTitle = 'add a new work task';
              this.workTask = this.scope.session.create('WorkTask') as WorkTask;
            }

            this.workEffortStates = loaded.collections
              .workEffortStates as WorkEffortState[];
            this.priorities = loaded.collections.priorities as Priority[];
            this.workEffortPurposes = loaded.collections
              .workEffortPurposes as WorkEffortPurpose[];
            const internalOrganisation: InternalOrganisation = loaded.objects
              .internalOrganisation as InternalOrganisation;
            this.employees = internalOrganisation.ActiveEmployees;

            const assignmentsFetch: Fetch[] = [
              new Fetch({
                id,
                name: 'workEffortAssignments',
                path: new Path({
                  step: m.WorkEffort.WorkEffortAssignmentsWhereAssignment,
                }),
              }),
            ];

            if (this.workTask.Customer) {
              this.updateCustomer(this.workTask.Customer);
            }

            if (addMode) {
              return this.scope.load('Pull', new PullRequest({}));
            } else {
              return this.scope.load(
                'Pull',
                new PullRequest({ fetches: assignmentsFetch }),
              );
            }
          });
      })
      .subscribe(
        (loaded) => {
          this.workEffortAssignments = loaded.collections
            .workEffortAssignments as WorkEffortAssignment[];

          if (this.workEffortAssignments) {
            this.assignees = this.workEffortAssignments.map(
              (v: WorkEffortAssignment) => v.Professional,
            );
          }

          this.existingAssignees = this.assignees;
        },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
    );
  }

  public customerSelected(customer: Party) {
    this.updateCustomer(customer);
  }

  public contactPersonAdded(id: string): void {
    this.addContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create(
      'OrganisationContactRelationship',
    ) as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.workTask
      .Customer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.workTask.ContactPerson = contact;
  }

  public contactMechanismAdded(
    partyContactMechanism: PartyContactMechanism,
  ): void {
    this.addContactMechanism = false;

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.workTask.Customer.AddPartyContactMechanism(partyContactMechanism);
    this.workTask.FullfillContactMechanism =
      partyContactMechanism.ContactMechanism;
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
          const workEffortAssignment: WorkEffortAssignment = this.scope.session.create(
            'WorkEffortAssignment',
          ) as WorkEffortAssignment;
          workEffortAssignment.Assignment = this.workTask;
          workEffortAssignment.Professional = assignee;
        }
      });
    }
    this.scope.save().subscribe(
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
    const fetches: Fetch[] = [
      new Fetch({
        id: party.id,
        include: [
          new TreeNode({
            nodes: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: this.m.PostalBoundary.Country })
                ],
                roleType: this.m.PostalAddress.PostalBoundary,
              }),
            ],
            roleType: this.m.PartyContactMechanism.ContactMechanism,
          }),
        ],
        name: 'partyContactMechanisms',
        path: new Path({ step: this.m.Party.CurrentPartyContactMechanisms }),
      }),
      new Fetch({
        id: party.id,
        name: 'currentContacts',
        path: new Path({ step: this.m.Party.CurrentContacts }),
      }),
    ];

    this.scope.load('Pull', new PullRequest({ fetches })).subscribe(
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
