import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import {  ContextService, MetaService, RefreshService } from '../../../../../angular';
import { WorkEffortPurchaseOrderItemAssignment, WorkEffort, PurchaseOrder } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { CreateData } from '../../../../../material/base/services/object';
import { increaseElementDepthCount } from '@angular/core/src/render3/state';

@Component({
  templateUrl: './workeffortpurchaseorderitemassignment-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortPurchaseOrderItemAssignmentEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  workEffortPurchaseOrderItemAssignment: WorkEffortPurchaseOrderItemAssignment;
  workEffort: WorkEffort;
  selectedPurchaseOrder: PurchaseOrder;

  private subscription: Subscription;
  purchaseOrders: PurchaseOrder[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<WorkEffortPurchaseOrderItemAssignmentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    
    private stateService: StateService,
    private snackBar: MatSnackBar) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.WorkEffortPurchaseOrderItemAssignment({
              object: this.data.id,
              include: {
                Assignment: x,
                PurchaseOrderItem: x
              }
            }
            ),
            pull.WorkEffort({
              object: this.data.associationId
            }),
            pull.PurchaseOrder({
              sort: new Sort(this.m.PurchaseOrder.OrderNumber),
              include: {
                TakenViaSupplier: x,
                PurchaseOrderItems: x
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

        this.workEffort = loaded.objects.WorkEffort as WorkEffort;
        this.purchaseOrders = loaded.collections.PurchaseOrders as PurchaseOrder[];

        if (isCreate) {
          this.title = 'Add purchase order item assignment';

          this.workEffortPurchaseOrderItemAssignment = this.allors.context.create('WorkEffortPurchaseOrderItemAssignment') as WorkEffortPurchaseOrderItemAssignment;
          this.workEffortPurchaseOrderItemAssignment.Assignment = this.workEffort;

        } else {
          this.workEffortPurchaseOrderItemAssignment = loaded.objects.WorkEffortPurchaseOrderItemAssignment as WorkEffortPurchaseOrderItemAssignment;
          this.selectedPurchaseOrder = this.workEffortPurchaseOrderItemAssignment.PurchaseOrder;

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
      });
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.workEffortPurchaseOrderItemAssignment.id,
          objectType: this.workEffortPurchaseOrderItemAssignment.objectType,
        };

        this.dialogRef.close(data);
      });
  }
}
