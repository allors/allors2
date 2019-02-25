import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { WorkEffortInventoryAssignment, WorkEffort, Part } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { EditData, CreateData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './workeffortinventoryassignment-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortInventoryAssignmentEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  workEffortInventoryAssignment: WorkEffortInventoryAssignment;
  title: string;

  private subscription: Subscription;
  parts: Part[];
  workEffort: WorkEffort;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<WorkEffortInventoryAssignmentEditComponent>,
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
            pull.WorkEffortInventoryAssignment({
              object: this.data.id,
              include: {
                Assignment: x,
                InventoryItem: {
                  Part: x
                }
              }
            }),
            pull.WorkEffort({
              object: this.data.associationId
            }),
            pull.Part({
              sort: new Sort(m.Part.Name)
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

        this.parts = loaded.collections.People as Part[];
        this.workEffort = loaded.objects.WorkEffort as WorkEffort;

        if (isCreate) {
          this.title = 'Add work effort inventory assignment';

          this.workEffortInventoryAssignment = this.allors.context.create('WorkEffortInventoryAssignment') as WorkEffortInventoryAssignment;
          this.workEffortInventoryAssignment.Assignment = this.workEffort;

        } else {
          this.workEffortInventoryAssignment = loaded.objects.WorkEffortInventoryAssignment as WorkEffortInventoryAssignment;

          if (this.workEffortInventoryAssignment.CanWriteInventoryItem) {
            this.title = 'Edit work effort inventory assignment';
          } else {
            this.title = 'View work effort inventory assignment';
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
          id: this.workEffortInventoryAssignment.id,
          objectType: this.workEffortInventoryAssignment.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
