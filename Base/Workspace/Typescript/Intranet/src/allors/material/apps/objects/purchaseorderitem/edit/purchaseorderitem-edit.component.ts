import * as moment from 'moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope, FetcherService } from '../../../../../angular';
import { PurchaseOrder, PurchaseOrderItem, VatRate, VatRegime, Part, SupplierOffering, SerialisedItem, Organisation } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { ObjectData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './purchaseorderitem-edit.component.html',
  providers: [ContextService]
})
export class PurchaseOrderItemEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  order: PurchaseOrder;
  orderItem: PurchaseOrderItem;
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  discount: number;
  surcharge: number;
  supplierOfferings: SupplierOffering[];
  supplierOffering: SupplierOffering;

  private subscription: Subscription;
  parts: Set<Part>;
  serialisedItems: SerialisedItem[];
  serialised: boolean;
  internalOrganisation: Organisation;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseOrderItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private snackBar: MatSnackBar,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.PurchaseOrderItem({
              object: this.data.id,
              include: {
                PurchaseOrderItemState: x,
                PurchaseOrderItemShipmentState: x,
                PurchaseOrderItemPaymentState: x,
                Part: x,
                SerialisedItem: x,
                VatRate: x,
                VatRegime: {
                  VatRate: x,
                }
              }
            }),
            pull.PurchaseOrderItem({
              object: this.data.id,
              fetch: {
                PurchaseOrderWherePurchaseOrderItem: {
                  TakenViaSupplier: {
                    SupplierOfferingsWhereSupplier: {
                      include: {
                        Part: {
                          InventoryItemKind: x
                        }
                      }
                    }
                  }
                }
              }
            }),
            pull.PurchaseOrderItem({
              object: this.data.id,
              fetch: {
                PurchaseOrderWherePurchaseOrderItem:
                {
                  include: {
                    VatRegime: x
                  }
                }
              }
            }),
            pull.VatRate(),
            pull.VatRegime(),
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.SupplierOffering({
                include: {
                  Part: x,
                  Supplier: x
                }
              }),
              pull.PurchaseOrder({
                object: this.data.associationId,
                include: {
                  VatRegime: x,
                  TakenViaSupplier: x
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

        const now = moment.utc();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];

        this.supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
        const parts = this.supplierOfferings
          .filter(v => v.Supplier === this.order.TakenViaSupplier && v.Supplier === this.order.TakenViaSupplier && moment(v.FromDate).isBefore(now) && (v.ThroughDate === null || moment(v.ThroughDate).isAfter(now)))
          .map(v => v.Part)
          .sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

        this.parts = new Set(parts);

        if (isCreate) {
          this.title = 'Add Order Item';
          this.orderItem = this.allors.context.create('PurchaseOrderItem') as PurchaseOrderItem;
          this.order.AddPurchaseOrderItem(this.orderItem);

        } else {
          this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
          this.updateFromPart(this.orderItem.Part);

          if (this.orderItem.CanWriteAssignedUnitPrice) {
            this.title = 'Edit Purchase Order Item';
          } else {
            this.title = 'View Purchase Order Item';
          }
        }
      });
  }

  public partSelected(part: Part): void {
    if (part) {
      this.updateFromPart(part);
    }
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
          id: this.orderItem.id,
          objectType: this.orderItem.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
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

  private updateFromPart(part: Part) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Part(
        {
          object: part,
          fetch: {
            SupplierOfferingsWherePart: x
          }
        }
      ),
      pull.Part(
        {
          object: part,
          fetch: {
            SerialisedItems: {
              include: {
                OwnedBy: x
              }
            }
          }
        }
      ),
      pull.Part(
        {
          object: part,
          include: {
            InventoryItemKind: x,
          }
        }
      ),
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.serialised = part.InventoryItemKind.UniqueId === '2596e2dd3f5d4588a4a2167d6fbe3fae';

        const supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
        this.supplierOffering = supplierOfferings.find(v => moment(v.FromDate).isBefore(moment())
          && (!v.ThroughDate || moment(v.ThroughDate).isAfter(moment()))
          && v.Supplier === this.order.TakenViaSupplier);

        const serialisedItems = loaded.collections.SerialisedItems as SerialisedItem[];
        this.serialisedItems = serialisedItems.filter(v => v.OwnedBy !== this.internalOrganisation);

        if (this.orderItem.SerialisedItem) {
          this.serialisedItems.push(this.orderItem.SerialisedItem);
        }
      });
  }
}
