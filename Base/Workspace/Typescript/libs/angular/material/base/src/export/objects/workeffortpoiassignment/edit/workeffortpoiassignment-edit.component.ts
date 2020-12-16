import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { PurchaseOrder, PurchaseOrderItem, WorkEffort, WorkEffortPurchaseOrderItemAssignment } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';


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
  purchaseOrderItems: PurchaseOrderItem[];

  constructor(
    @Self() public allors: ContextService,
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

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          let pulls = [
            pull.PurchaseOrder({
              sort: new Sort(this.m.PurchaseOrder.OrderNumber),
              include: {
                TakenViaSupplier: x,
                PurchaseOrderItems: {
                  Part: x,
                  PurchaseOrderWherePurchaseOrderItem: x,
                },
                WorkEffortPurchaseOrderItemAssignmentsWherePurchaseOrder: x,
              }
            }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.WorkEffortPurchaseOrderItemAssignment({
                object: this.data.id,
                include: {
                  Assignment: x,
                  PurchaseOrderItem: x
                }
              }),
            );
          }

          if (isCreate) {
            pulls = [
              ...pulls,
              pull.WorkEffort({
                object: this.data.associationId
              }),
            ];
          }

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        if (isCreate) {
          this.workEffort = loaded.objects.WorkEffort as WorkEffort;
          this.title = 'Add purchase order item assignment';

          this.workEffortPurchaseOrderItemAssignment = this.allors.context.create('WorkEffortPurchaseOrderItemAssignment') as WorkEffortPurchaseOrderItemAssignment;
          this.workEffortPurchaseOrderItemAssignment.Assignment = this.workEffort;
          this.workEffortPurchaseOrderItemAssignment.Quantity = 1;

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

        const purchaseOrders = loaded.collections.PurchaseOrders as PurchaseOrder[];
        this.purchaseOrders = purchaseOrders
          .filter(v => v.PurchaseOrderItems
                  .find(i => i.WorkEffortPurchaseOrderItemAssignmentsWherePurchaseOrderItem.length === 0
                            && !i.Part
                            && i.PurchaseOrderWherePurchaseOrderItem.OrderedBy === this.workEffort.TakenBy));
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
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public purchaseOrderSelected(purchaseOrder: PurchaseOrder): void {
    this.purchaseOrderItems = purchaseOrder.PurchaseOrderItems.filter(v => v.WorkEffortPurchaseOrderItemAssignmentsWherePurchaseOrderItem.length === 0);
  }
}
