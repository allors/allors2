import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import {
  InventoryItem,
  Part,
  SerialisedInventoryItem,
  SerialisedItem,
  NonSerialisedInventoryItem,
  PurchaseInvoice,
  PurchaseInvoiceItem,
  PurchaseOrderItem,
  VatRegime,
  IrpfRegime,
  InvoiceItemType,
  SupplierOffering,
  UnifiedGood,
  Organisation,
} from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, TreeFactory } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { FetcherService, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort, And, ContainedIn, Extent } from '@allors/data/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  templateUrl: './purchaseinvoiceitem-edit.component.html',
  providers: [ContextService],
})
export class PurchaseInvoiceItemEditComponent extends TestScope implements OnInit, OnDestroy {
  m: Meta;

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
  serviceItemType: InvoiceItemType;
  timeItemType: InvoiceItemType;
  part: Part;
  serialisedItems: SerialisedItem[];
  serialisedItem: SerialisedItem;
  serialised: boolean;
  nonUnifiedPart: boolean;
  unifiedGood: boolean;
  showIrpf: boolean;
  vatRegimeInitialRole: VatRegime;
  irpfRegimeInitialRole: IrpfRegime;

  private subscription: Subscription;
  partsFilter: SearchFactory;
  supplierOffering: SupplierOffering;
  
  unifiedGoodsFilter: SearchFactory;
  internalOrganisation: Organisation;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseInvoiceItemEditComponent>,
    public metaService: MetaService,
    private fetcher: FetcherService,
    public refreshService: RefreshService,
    private saveService: SaveService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$])
      .pipe(
        switchMap(() => {
          const isCreate = this.data.id === undefined;
          const { id } = this.data;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
              sort: new Sort(m.InvoiceItemType.Name),
            }),
            pull.IrpfRegime({
              sort: new Sort(m.IrpfRegime.Name),
            }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.PurchaseInvoiceItem({
                object: id,
                include: {
                  PurchaseInvoiceItemState: x,
                  SerialisedItem: x,
                  DerivedVatRegime: {
                    VatRates: x,
                  },
                  DerivedIrpfRegime: {
                    IrpfRates: x,
                  },
            },
              }),
              pull.PurchaseInvoiceItem({
                object: id,
                fetch: {
                  PurchaseInvoiceWherePurchaseInvoiceItem: {
                    include: {
                      DerivedVatRegime: {
                        VatRates: x,
                      },
                      DerivedIrpfRegime: {
                        IrpfRates: x,
                      },
                    },
                  },
                },
              }),
            );
          }

          if (this.data.associationId) {
            pulls.push(
              pull.PurchaseInvoice({
                object: this.data.associationId,
                include: {
                  DerivedVatRegime: {
                    VatRates: x,
                  },
                  DerivedIrpfRegime: {
                    IrpfRates: x,
                  },
                },
              })
            );
          }

          this.unifiedGoodsFilter = Filters.unifiedGoodsFilter(m, this.metaService.tree);

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => ({ loaded, isCreate })));
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.invoiceItem = loaded.objects.PurchaseInvoiceItem as PurchaseInvoiceItem;
        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.partItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'ff2b943d-57c9-4311-9c56-9ff37959653b');
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '0d07f778-2735-44cb-8354-fb887ada42ad');
        this.serviceItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'a4d2e6d0-c6c1-46ec-a1cf-3a64822e7a9e');
        this.timeItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'da178f93-234a-41ed-815c-819af8ca4e6f');

        this.partsFilter = new SearchFactory({
          objectType: this.m.Part,
          roleTypes: [this.m.Part.Name, this.m.Part.SearchString],
          post: (predicate: And) => {
            predicate.operands.push(
              new ContainedIn({
                propertyType: this.m.Part.SupplierOfferingsWherePart,
                extent: new Extent({
                  objectType: this.m.SupplierOffering,
                  predicate: new Equals({ propertyType: m.SupplierOffering.Supplier, object: this.invoice.BilledFrom }),
                }),
              })
            );
          },
        });

        if (isCreate) {
          this.title = 'Add purchase invoice Item';
          this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
          this.invoiceItem = this.allors.context.create('PurchaseInvoiceItem') as PurchaseInvoiceItem;
          this.invoice.AddPurchaseInvoiceItem(this.invoiceItem);
          this.vatRegimeInitialRole = this.invoice.DerivedVatRegime;
          this.irpfRegimeInitialRole = this.invoice.DerivedIrpfRegime;
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

  public goodSelected(unifiedGood: ISessionObject): void {
    if (unifiedGood) {
      this.part = unifiedGood as UnifiedGood;
      this.refreshSerialisedItems(unifiedGood as UnifiedGood);
    }
  }

  public serialisedItemSelected(serialisedItem: ISessionObject): void {
    this.serialisedItem = this.part.SerialisedItems.find((v) => v === serialisedItem);
    this.invoiceItem.Quantity = '1';
  }

  public partSelected(part: ISessionObject): void {
    if (part) {
      this.unifiedGood = this.invoiceItem.Part.objectType.name === this.m.UnifiedGood.name;
      this.nonUnifiedPart = this.invoiceItem.Part.objectType.name === this.m.NonUnifiedPart.name;

      this.updateFromPart(part as Part);
    }
  }

  public save(): void {
    this.onSave();

    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.invoiceItem.id,
        objectType: this.invoiceItem.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }

  private refreshSerialisedItems(unifiedGood: UnifiedGood): void {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.NonUnifiedGood({
        object: unifiedGood.id,
        fetch: {
          Part: {
            include: {
              SerialisedItems: x,
              InventoryItemKind: x,
            },
          },
        },
      }),
      pull.UnifiedGood({
        object: unifiedGood.id,
        include: {
          InventoryItemKind: x,
          SerialisedItems: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe(() => {
      this.serialisedItems = this.part.SerialisedItems;
      this.serialised = this.part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';
    });
  }

  private updateFromPart(part: Part) {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Part({
        object: part,
        fetch: {
          SerialisedItems: {
            include: {
              OwnedBy: x,
            },
          },
        },
      }),
      pull.Part({
        object: part,
        fetch: {
          SupplierOfferingsWherePart: {
            include: {
              Supplier: x,
            },
          },
        },
      }),
      pull.Part({
        object: part,
        include: {
          InventoryItemKind: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      this.part = (loaded.objects.UnifiedGood || loaded.objects.Part) as Part;
      this.serialised = part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';

      const supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
      this.supplierOffering = supplierOfferings.find(
        (v) =>
          isBefore(new Date(v.FromDate), new Date()) &&
          (!v.ThroughDate || isAfter(new Date(v.ThroughDate), new Date())) &&
          v.Supplier === this.invoice.BilledFrom
      );

      this.serialisedItems = loaded.collections.SerialisedItems as SerialisedItem[];

      if (this.invoiceItem.SerialisedItem) {
        this.serialisedItems.push(this.invoiceItem.SerialisedItem);
      }
    });
  }

  private onSave() {
    if (this.invoiceItem.InvoiceItemType !== this.partItemType && this.invoiceItem.InvoiceItemType !== this.partItemType) {
      this.invoiceItem.Quantity = '1';
    }
  }
}
