import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { WorkEffortPurchaseOrderItemAssignment, WorkEffort, PurchaseOrder, PurchaseOrderItem } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { ObjectData } from '../../../../../material/base/services/object';
import { SaveService } from '../../../../../../allors/material';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './workeffortpoiassignment-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortPurchaseOrderItemAssignmentEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  workEffortPurchaseOrderItemAssignment: WorkEffortPurchaseOrderItemAssignment;
  workEffort: WorkEffort;
  selectedPurchaseOrder: PurchaseOrder;

  private subscription: Subscription;
  purchaseOrders: PurchaseOrder[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<WorkEffortPurchaseOrderItemAssignmentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
    private snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([]) => {

          const isCreate = this.data.id === undefined;

          let pulls = [
            pull.WorkEffortPurchaseOrderItemAssignment({
              object: this.data.id,
              include: {
                Assignment: x,
                PurchaseOrderItem: x
              }
            }
            ),
            pull.PurchaseOrder({
              sort: new Sort(this.m.PurchaseOrder.OrderNumber),
              include: {
                TakenViaSupplier: x,
                PurchaseOrderItems: x
              }
            }),
          ];

          if (isCreate) {
            pulls = [
              ...pulls,
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

        this.purchaseOrders = loaded.collections.PurchaseOrders as PurchaseOrder[];

        if (isCreate) {
          this.workEffort = loaded.objects.WorkEffort as WorkEffort;
          this.title = 'Add purchase order item assignment';

          this.workEffortPurchaseOrderItemAssignment = this.allors.context.create('WorkEffortPurchaseOrderItemAssignment') as WorkEffortPurchaseOrderItemAssignment;
          this.workEffortPurchaseOrderItemAssignment.Assignment = this.workEffort;

        } else {
          this.workEffortPurchaseOrderItemAssignment = loaded.objects.WorkEffortPurchaseOrderItemAssignment as WorkEffortPurchaseOrderItemAssignment;
          this.selectedPurchaseOrder = this.workEffortPurchaseOrderItemAssignment.PurchaseOrder;
          this.workEffort = this.workEffortPurchaseOrderItemAssignment.Assignment;

          if (this.workEffortPurchaseOrderItemAssignment.CanWritePurchaseOrderItem) {
            this.title = 'Edit purchase order item assignment';
          } else {
            this.title = 'View purchase order item assignment';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public update(): void {
    const { context } = this.allors;

    context
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.workEffortPurchaseOrderItemAssignment.id,
          objectType: this.workEffortPurchaseOrderItemAssignment.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
