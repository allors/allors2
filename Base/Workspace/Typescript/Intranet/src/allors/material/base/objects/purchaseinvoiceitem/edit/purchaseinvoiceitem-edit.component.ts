import * as moment from 'moment/moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope, SearchFactory } from '../../../../../angular';
import { InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrderItem, SerialisedInventoryItem, VatRate, VatRegime, Part, Product, SerialisedItem, SupplierOffering, IrpfRegime } from '../../../../../domain';
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
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  invoiceItemTypes: InvoiceItemType[];
  partItemType: InvoiceItemType;
  productItemType: InvoiceItemType;
  part: Part;
  serialisedItems: SerialisedItem[];
  serialisedItem: SerialisedItem;
  serialised: boolean;
  nonUnifiedPart: boolean;
  unifiedGood: boolean;

  private subscription: Subscription;
  partsFilter: SearchFactory;
  transportItemType: InvoiceItemType;
  refurbishItemType: InvoiceItemType;
  supplierOffering: SupplierOffering;

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
                SerialisedItem: x,
                VatRegime: {
                  VatRate: x,
                },
                IrpfRegime: {
                  IrpfRate: x,
                },
              }
            }),
            pull.PurchaseInvoiceItem({
              object: id,
              fetch: {
                PurchaseInvoiceWherePurchaseInvoiceItem: {
                  include: {
                    VatRegime: {
                      VatRate: x,
                    },
                    IrpfRegime: {
                      IrpfRate: x,
                    },
                  }
                }
              }
            }),
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
            }),
            pull.VatRegime({ 
              sort: new Sort(m.VatRegime.Name) }),
            pull.IrpfRegime({ 
              sort: new Sort(m.IrpfRegime.Name) }),
          ];

          if (this.data.associationId) {
            pulls.push(
              pull.PurchaseInvoice({
                object: this.data.associationId,
                include: {
                  VatRegime: {
                    VatRate: x,
                  },
                  IrpfRegime: {
                    IrpfRate: x,
                  }
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

        this.invoiceItem = loaded.objects.PurchaseInvoiceItem as PurchaseInvoiceItem;
        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.partItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'ff2b943d-57c9-4311-9c56-9ff37959653b');
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '0d07f778-2735-44cb-8354-fb887ada42ad');
        this.transportItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '96c1c0ff-b0f1-480f-91a7-4658bebe6674');
        this.refurbishItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'f2d9770b-f933-48b0-a495-df80cb702fce');

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
          this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
          this.invoiceItem = this.allors.context.create('PurchaseInvoiceItem') as PurchaseInvoiceItem;
          this.invoice.AddPurchaseInvoiceItem(this.invoiceItem);
        } else {
          this.invoice = this.invoiceItem.PurchaseInvoiceWherePurchaseInvoiceItem;

          if (this.invoiceItem.Part) {
            this.unifiedGood = this.invoiceItem.Part.objectType.name === m.UnifiedGood.name;
            this.nonUnifiedPart = this.invoiceItem.Part.objectType.name === m.NonUnifiedPart.name;
            this.updateFromPart(this.invoiceItem.Part);
          }

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

  public goodSelected(product: Product): void {
    if (product) {
      this.refreshSerialisedItems(product);
    }
  }

  public serialisedItemSelected(serialisedItem: SerialisedItem): void {

    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
    this.invoiceItem.Quantity = '1';
  }

  public partSelected(part: Part): void {
    if (part) {
      this.unifiedGood = this.invoiceItem.Part.objectType.name === this.m.UnifiedGood.name;
      this.nonUnifiedPart = this.invoiceItem.Part.objectType.name === this.m.NonUnifiedPart.name;

      this.updateFromPart(part);
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
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  private refreshSerialisedItems(product: Product): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.NonUnifiedGood({
        object: product.id,
        fetch: {
          Part: {
            include: {
              SerialisedItems: x,
              InventoryItemKind: x,
            }
          }
        }
      }),
      pull.UnifiedGood({
        object: product.id,
        include: {
          InventoryItemKind: x,
          SerialisedItems: x,
        }
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        this.part = (loaded.objects.UnifiedGood || loaded.objects.Part) as Part;
        this.serialisedItems = this.part.SerialisedItems;
        this.serialised = this.part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';
      });
  }

  private updateFromPart(part: Part) {

    const { pull, x } = this.metaService;

    const pulls = [
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
          fetch: {
            SupplierOfferingsWherePart: {
              include: {
                Supplier: x
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
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.serialised = part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';

        const supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
        this.supplierOffering = supplierOfferings.find(v => moment(v.FromDate).isBefore(moment())
          && (!v.ThroughDate || moment(v.ThroughDate).isAfter(moment()))
          && v.Supplier === this.invoice.BilledFrom);

        this.serialisedItems = loaded.collections.SerialisedItems as SerialisedItem[];

        if (this.invoiceItem.SerialisedItem) {
          this.serialisedItems.push(this.invoiceItem.SerialisedItem);
        }
      });
  }

  private onSave() {

    if (this.invoiceItem.InvoiceItemType !== this.partItemType &&
      this.invoiceItem.InvoiceItemType !== this.partItemType) {
      this.invoiceItem.Quantity = '1';
    }
  }
}
