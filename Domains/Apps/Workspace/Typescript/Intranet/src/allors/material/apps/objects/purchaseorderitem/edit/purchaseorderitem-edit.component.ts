import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { PurchaseOrder, PurchaseOrderItem, VatRate, VatRegime, Part, SupplierOffering } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { CreateData, EditData, ObjectData } from '../../../../../material/base/services/object';

@Component({
  templateUrl: './purchaseorderitem-edit.component.html',
  providers: [ContextService]
})
export class PurchaseOrderItemEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  order: PurchaseOrder;
  orderItem: PurchaseOrderItem;
  parts: Part[];
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  discount: number;
  surcharge: number;

  private subscription: Subscription;
  supplierOfferings: SupplierOffering[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<PurchaseOrderItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    public stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          const pulls = [
            pull.PurchaseOrderItem({
              object: this.data.id,
              include: {
                PurchaseOrderItemState: x,
                Part: x,
                SerialisedItem: x,
                DerivedVatRate: x,
                VatRegime: {
                  VatRate: x,
                }
              }
            }),
            pull.PurchaseOrderItem({
              object: this.data.id,
              fetch: {
                PurchaseOrderWherePurchaseOrderItem: {
                  include: {
                    VatRegime: x
                  }
                }
              }
            }),
            pull.PurchaseOrderItem({
              object: this.data.id,
              fetch: {
                PurchaseOrderWherePurchaseOrderItem: {
                  TakenViaSupplier: {
                    SupplierOfferingsWhereSupplier: {
                      Part: x
                    }
                  }
                }
              }
            }),
            pull.VatRate(),
            pull.VatRegime(),
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.PurchaseOrder({
                object: this.data.associationId,
                include: {
                  VatRegime: x
                }
              })
            );
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

        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.parts = loaded.collections.Parts as Part[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];

        if (isCreate) {
          this.title = 'Add Order Item';
          this.orderItem = this.allors.context.create('PurchaseOrderItem') as PurchaseOrderItem;
          this.order.AddPurchaseOrderItem(this.orderItem);

        } else {
          this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;

          if (this.orderItem.CanWriteActualUnitPrice) {
            this.title = 'Edit Purchase Order Item';
          } else {
            this.title = 'View Purchase Order Item';
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
          id: this.orderItem.id,
          objectType: this.orderItem.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
