import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { WorkEffortInventoryAssignment, WorkEffort, Part, InventoryItem, Facility, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, SerialisedInventoryItemState, SerialisedInventoryItem } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { CreateData } from '../../../../../material/base/services/object';

@Component({
  templateUrl: './workeffortinventoryassignment-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortInventoryAssignmentEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  workEffortInventoryAssignment: WorkEffortInventoryAssignment;
  parts: Part[];
  workEffort: WorkEffort;
  inventoryItems: InventoryItem[];
  facility: Facility;
  state: NonSerialisedInventoryItemState | SerialisedInventoryItemState;
  serialised: boolean;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
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

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.WorkEffortInventoryAssignment({
              object: this.data.id,
              include: {
                Assignment: x,
                InventoryItem: {
                  Part: {
                    InventoryItemKind: x
                  },
                }
              }
            }),
            pull.WorkEffort({
              object: this.data.associationId
            }),
            pull.InventoryItem({
              sort: new Sort(m.InventoryItem.Name),
              include: {
                Part: {
                  InventoryItemKind: x
                },
                Facility: x,
                SerialisedInventoryItem_SerialisedInventoryItemState: x,
                NonSerialisedInventoryItem_NonSerialisedInventoryItemState: x
              }
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

        this.inventoryItems = loaded.collections.InventoryItems as InventoryItem[];
        this.workEffort = loaded.objects.WorkEffort as WorkEffort;

        if (isCreate) {
          this.title = 'Add work effort inventory assignment';

          this.workEffortInventoryAssignment = this.allors.context.create('WorkEffortInventoryAssignment') as WorkEffortInventoryAssignment;
          this.workEffortInventoryAssignment.Assignment = this.workEffort;

        } else {
          this.workEffortInventoryAssignment = loaded.objects.WorkEffortInventoryAssignment as WorkEffortInventoryAssignment;
          this.inventoryItemSelected(this.workEffortInventoryAssignment.InventoryItem);

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
        const data: IObject = {
          id: this.workEffortInventoryAssignment.id,
          objectType: this.workEffortInventoryAssignment.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public inventoryItemSelected(inventoryItem: InventoryItem): void {
    this.serialised = inventoryItem.Part.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase();

    if (inventoryItem.objectType === this.metaService.m.NonSerialisedInventoryItem) {
      const item = inventoryItem as NonSerialisedInventoryItem;
      this.state = item.NonSerialisedInventoryItemState;
    } else {
      const item = inventoryItem as SerialisedInventoryItem;
      this.state = item.SerialisedInventoryItemState;
    }
  }
}
