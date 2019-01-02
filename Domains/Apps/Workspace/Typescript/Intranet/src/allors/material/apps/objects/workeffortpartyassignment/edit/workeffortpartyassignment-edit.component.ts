import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Enumeration, WorkEffortPartyAssignment, Person, WorkEffort, Party, Organisation } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { EditData, CreateData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './workeffortpartyassignment-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortPartyAssignmentEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  workEffortPartyAssignment: WorkEffortPartyAssignment;
  people: Person[];
  workEfforts: WorkEffort[];
  person: Person;
  party: Party;
  contacts: Person[] = [];
  title: string;

  private subscription: Subscription;
  organisation: Organisation;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<WorkEffortPartyAssignmentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          const pulls = [
            pull.WorkEffortPartyAssignment({
              object: this.data.id,
              include: {
                Assignment: x,
                Party: x
              }
            }),
            pull.Party({
              object: this.data.associationId
            }),
            pull.Organisation({
              object: this.data.associationId,
              include: {
                CurrentOrganisationContactRelationships: {
                  Contact: x
                }
              }
            }),
            pull.WorkEffort({
              sort: new Sort(m.WorkEffort.Name)
            }),
            pull.Person({
              sort: new Sort(m.Person.PartyName)
            }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.workEfforts = loaded.collections.WorkEfforts as WorkEffort[];
        this.people = loaded.collections.People as Person[];
        this.party = loaded.objects.Party as Party;
        this.organisation = loaded.objects.Organisation as Organisation;

        if (isCreate) {
          this.title = 'Add Work Effort Party Assignment';

          this.workEffortPartyAssignment = this.allors.context.create('WorkEffortPartyAssignment') as WorkEffortPartyAssignment;

          if (this.organisation) {
            this.organisation.CurrentOrganisationContactRelationships.forEach((relationship) => {
              this.contacts.push(relationship.Contact);
            });
          }

          if (this.party.objectType.name === m.Person.name) {
            this.person = this.party as Person;
            this.workEffortPartyAssignment.Party = this.person;
          }

        } else {
          this.workEffortPartyAssignment = loaded.objects.WorkEffortPartyAssignment as WorkEffortPartyAssignment;
          this.person = this.workEffortPartyAssignment.Party as Person;

          if (this.workEffortPartyAssignment.CanWriteFromDate) {
            this.title = 'Edit Work Effort Party Assignment';
          } else {
            this.title = 'View Work Effort Party Assignment';
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.workEffortPartyAssignment.id,
          objectType: this.workEffortPartyAssignment.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
