import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrderItem, SerialisedInventoryItem, VatRate, VatRegime, Part } from '../../../../../domain';
import { PullRequest, Equals, Sort, IObject } from '../../../../../framework';
import { ObjectData, SaveService, FiltersService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './purchaseinvoiceitem-edit.component.html',
  providers: [ContextService]
})
export class PurchaseInvoiceItemEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  invoice: PurchaseInvoice;
  invoiceItem: PurchaseInvoiceItem;
  orderItem: PurchaseOrderItem;
  inventoryItems: InventoryItem[];
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  parts: Part[];
  invoiceItemTypes: InvoiceItemType[];
  partItemType: InvoiceItemType;
  productItemType: InvoiceItemType;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<PurchaseInvoiceItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = this.data.id === undefined;
          const { id } = this.data;

          const pulls = [
            pull.PurchaseInvoiceItem({
              object: id,
              include:
              {
                PurchaseInvoiceItemState: x,
                VatRegime: {
                  VatRate: x,
                }
              }
            }),
            pull.Part({
              sort: [
                new Sort(m.Part.Name),
              ],
            }),
            pull.PurchaseInvoiceItem({
              object: id,
              fetch: {
                PurchaseInvoiceWherePurchaseInvoiceItem: {
                  include: {
                    VatRegime: x
                  }
                }
              }
            }),
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
            }),
            pull.VatRate(),
            pull.VatRegime()
          ];

          if (this.data.associationId) {
            pulls.push(
              pull.PurchaseInvoice({
                object: this.data.associationId,
                include: {
                  VatRegime: x
                }
              })
            );
          }

          return this.allors.context.load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
        this.invoiceItem = loaded.objects.PurchaseInvoiceItem as PurchaseInvoiceItem;
        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.parts = loaded.collections.Parts as Part[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.partItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'ff2b943d57c943119c569ff37959653b');
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '0d07f778273544cb8354fb887ada42ad');

        if (isCreate) {
          this.title = 'Add purchase invoice Item';
          this.invoiceItem = this.allors.context.create('PurchaseInvoiceItem') as PurchaseInvoiceItem;
          this.invoice.AddPurchaseInvoiceItem(this.invoiceItem);
        } else {

          if (this.invoiceItem.CanWriteQuantity) {
            this.title = 'Edit purchase invoice Item';
          } else {
            this.title = 'View purchase invoice Item';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.onSave();

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.invoiceItem.id,
          objectType: this.invoiceItem.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }

  private onSave() {

    if (this.invoiceItem.InvoiceItemType !== this.partItemType &&
      this.invoiceItem.InvoiceItemType !== this.partItemType) {
      this.invoiceItem.Quantity = 1;
    }
  }
}
