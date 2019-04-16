import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, InternalOrganisationId } from '../../../../../angular';
import { WorkEffortPartyAssignment, Person, WorkEffort, Party, Employment } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { CreateData } from '../../../../../material/base/services/object';
import { SaveService } from 'src/allors/material';

@Component({
  templateUrl: './workeffortpartyassignment-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortPartyAssignmentEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  workEffortPartyAssignment: WorkEffortPartyAssignment;
  people: Person[];
  person: Person;
  party: Party;
  workEffort: WorkEffort;
  assignment: WorkEffort;
  contacts: Person[] = [];
  title: string;

  private subscription: Subscription;
  employees: Person[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<WorkEffortPartyAssignmentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([, internalOrganisationId]) => {

          const isCreate = (this.data as IObject).id === undefined;

          let pulls = [
            pull.WorkEffortPartyAssignment({
              object: this.data.id,
              include: {
                Assignment: x,
                Party: x
              }
            }),
            pull.Organisation({
              object: internalOrganisationId,
              fetch: {
                EmploymentsWhereEmployer: {
                  include: {
                    Employee: x
                  }
                }
              },
              sort: new Sort(m.Person.PartyName)
            }),
          ];

          if (isCreate) {
            pulls = [
              ...pulls,
              pull.Party({
                object: this.data.associationId
              }),
              pull.WorkEffort({
                object: this.data.associationId
              }),
            ];
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        if (isCreate) {
          this.title = 'Add Party Assignment';

          this.workEffortPartyAssignment = this.allors.context.create('WorkEffortPartyAssignment') as WorkEffortPartyAssignment;
          this.party = loaded.objects.Party as Party;
          this.workEffort = loaded.objects.WorkEffort as WorkEffort;

          if (this.party !== undefined && this.party.objectType.name === m.Person.name) {
            this.person = this.party as Person;
            this.workEffortPartyAssignment.Party = this.person;
          }

          if (this.workEffort !== undefined && this.workEffort.objectType.name === m.WorkTask.name) {
            this.assignment = this.workEffort as WorkEffort;
            this.workEffortPartyAssignment.Assignment = this.assignment;
          }

        } else {
          this.workEffortPartyAssignment = loaded.objects.WorkEffortPartyAssignment as WorkEffortPartyAssignment;
          this.party = this.workEffortPartyAssignment.Party;
          this.workEffort = this.workEffortPartyAssignment.Assignment;
          this.person = this.workEffortPartyAssignment.Party as Person;
          this.assignment = this.workEffortPartyAssignment.Assignment;

          if (this.workEffortPartyAssignment.CanWriteFromDate) {
            this.title = 'Edit Party Assignment';
          } else {
            this.title = 'View Party Assignment';
          }
        }

        const employments = loaded.collections.Employments as Employment[];
        this.employees = employments.filter(v => v.FromDate <= this.workEffort.ScheduledStart && (v.ThroughDate === null || v.ThroughDate >= this.workEffort.ScheduledStart)).map(v => v.Employee);
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.workEffortPartyAssignment.id,
          objectType: this.workEffortPartyAssignment.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
