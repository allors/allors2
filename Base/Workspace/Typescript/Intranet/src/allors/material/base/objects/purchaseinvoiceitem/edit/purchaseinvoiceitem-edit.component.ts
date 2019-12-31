import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope, SearchFactory } from '../../../../../angular';
import { InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrderItem, SerialisedInventoryItem, VatRate, VatRegime, Part } from '../../../../../domain';
import { PullRequest, Equals, Sort, IObject, And, ContainedIn, Filter, LessThan, Or, Not, Exists, GreaterThan } from '../../../../../framework';
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
  invoiceItemTypes: InvoiceItemType[];
  partItemType: InvoiceItemType;
  productItemType: InvoiceItemType;

  private subscription: Subscription;
  partsFilter: SearchFactory;

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
        switchMap(() => {

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

          return this.allors.context.load(new PullRequest({ pulls }))
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
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.partItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'ff2b943d-57c9-4311-9c56-9ff37959653b');
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '0d07f778-2735-44cb-8354-fb887ada42ad');

        this.partsFilter = new SearchFactory({
          objectType: this.m.Part,
          roleTypes: [this.m.Part.Name, this.m.Part.SearchString],
          post: (predicate: And) => {
            predicate.operands.push(new ContainedIn({
              propertyType: this.m.Part.SupplierOfferingsWherePart,
              extent: new Filter({
                objectType: this.m.SupplierOffering,
                predicate: new Equals({ propertyType: m.SupplierOffering.Supplier, object: this.invoice.BilledFrom }),
              })
            }));
          },
        });

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
      this.invoiceItem.Quantity = '1';
    }
  }
}
