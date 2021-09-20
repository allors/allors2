import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { InventoryItem, Part, Facility, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItem, PurchaseOrder, PurchaseOrderItem, VatRegime, IrpfRegime, InvoiceItemType, SupplierOffering, UnifiedGood, Product, Organisation } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, TreeFactory } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { FetcherService, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort, And, ContainedIn, Extent, LessThan, Or, Not, Exists, GreaterThan } from '@allors/data/system';
import { TestScope, SearchFactory } from '@allors/angular/core';


@Component({
  templateUrl: './purchaseorderitem-edit.component.html',
  providers: [ContextService]
})
export class PurchaseOrderItemEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  order: PurchaseOrder;
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
  addFacility = false;
  supplierOffering: SupplierOffering;
  facilities: Facility[];
  selectedFacility: Facility;
  showIrpf: boolean;
  vatRegimeInitialRole: VatRegime;
  irpfRegimeInitialRole: IrpfRegime;

  private subscription: Subscription;
  partsFilter: SearchFactory;
  
  unifiedGoodsFilter: SearchFactory;
  internalOrganisation: Organisation;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseOrderItemEditComponent>,
    public metaService: MetaService,
    private fetcher: FetcherService,
    public refreshService: RefreshService,
    private saveService: SaveService,
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
              sort: new Sort(m.InvoiceItemType.Name)
            }),
            pull.IrpfRegime({
              sort: new Sort(m.IrpfRegime.Name)
            }),
            pull.Facility({
              sort: new Sort(m.Facility.Name)
            }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.PurchaseOrderItem({
                object: id,
                include:
                {
                  InvoiceItemType: x,
                  PurchaseOrderItemState: x,
                  PurchaseOrderItemShipmentState: x,
                  PurchaseOrderItemPaymentState: x,
                  Part: x,
                  SerialisedItem: x,
                  StoredInFacility: x,
                  DerivedVatRegime: {
                    VatRates: x,
                  },
                  DerivedIrpfRegime: {
                    IrpfRates: x,
                  }
                }
              }),
              pull.PurchaseOrderItem({
                object: id,
                fetch: {
                  PurchaseOrderWherePurchaseOrderItem: {
                    include: {
                      DerivedVatRegime: {
                        VatRates: x,
                      },
                      DerivedIrpfRegime: {
                        IrpfRates: x,
                      }
                    }
                  }
                }
              }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.PurchaseOrder({
                object: this.data.associationId,
                include: {
                  DerivedVatRegime: x,
                  DerivedIrpfRegime: x,
                  TakenViaSupplier: x
                }
              })
            );
          }

          this.unifiedGoodsFilter = Filters.unifiedGoodsFilter(m, this.metaService.tree);

          return this.allors.context.load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.showIrpf = this.internalOrganisation.Country.IsoCode === "ES";
        this.vatRegimes = this.internalOrganisation.Country.DerivedVatRegimes;
        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.facilities = loaded.collections.Facilities as Facility[];

        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.partItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'ff2b943d-57c9-4311-9c56-9ff37959653b');
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '0d07f778-2735-44cb-8354-fb887ada42ad');
        this.serviceItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'a4d2e6d0-c6c1-46ec-a1cf-3a64822e7a9e');
        this.timeItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'da178f93-234a-41ed-815c-819af8ca4e6f');

        this.partsFilter = new SearchFactory({
          objectType: this.m.NonUnifiedPart,
          roleTypes: [this.m.NonUnifiedPart.Name, this.m.NonUnifiedPart.SearchString],
          post: (predicate: And) => {
            predicate.operands.push(new ContainedIn({
              propertyType: this.m.NonUnifiedPart.SupplierOfferingsWherePart,
              extent: new Extent({
                objectType: this.m.SupplierOffering,
                predicate: new And([
                  new Equals({ propertyType: m.SupplierOffering.Supplier, object: this.order.TakenViaSupplier }),
                  new LessThan({ roleType: m.SupplierOffering.FromDate, value: this.order.OrderDate }),
                  new Or([
                    new Not({ operand: new Exists({ propertyType: m.SupplierOffering.ThroughDate }) }),
                    new GreaterThan({ roleType: m.SupplierOffering.ThroughDate, value: this.order.OrderDate }),
                  ])
                ])
              })
            }));
          },
        });

        if (isCreate) {
          this.title = 'Add Purchase Order Item';
          this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
          this.orderItem = this.allors.context.create('PurchaseOrderItem') as PurchaseOrderItem;
          this.selectedFacility = this.order.StoredInFacility;
          this.order.AddPurchaseOrderItem(this.orderItem);
          this.vatRegimeInitialRole = this.order.DerivedVatRegime;
          this.irpfRegimeInitialRole = this.order.DerivedIrpfRegime;
        } else {
          this.order = this.orderItem.PurchaseOrderWherePurchaseOrderItem;
          this.selectedFacility = this.orderItem.StoredInFacility;

          if (this.orderItem.Part) {
            this.unifiedGood = this.orderItem.Part.objectType.name === m.UnifiedGood.name;
            this.nonUnifiedPart = this.orderItem.Part.objectType.name === m.NonUnifiedPart.name;
            this.updateFromPart(this.orderItem.Part);
          }

          if (this.orderItem.CanWriteQuantityOrdered) {
            this.title = 'Edit Purchase Order Item';
          } else {
            this.title = 'View Purchase Order Item';
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

    if(serialisedItem) {
    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
    this.orderItem.QuantityOrdered = '1';
    }
  }

  public partSelected(part: ISessionObject): void {
    if (part) {
      this.unifiedGood = this.orderItem.Part.objectType.name === this.m.UnifiedGood.name;
      this.nonUnifiedPart = this.orderItem.Part.objectType.name === this.m.NonUnifiedPart.name;

      this.updateFromPart(part as Part);
    }
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
  }

  public save(): void {

    this.onSave();

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.orderItem.id,
          objectType: this.orderItem.objectType,
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
      .subscribe(() => {

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
        this.part = (loaded.objects.UnifiedGood || loaded.objects.Part) as Part;
        this.serialised = part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';

        const supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
        this.supplierOffering = supplierOfferings.find(v => isBefore(new Date(v.FromDate), new Date())
          && (!v.ThroughDate || isAfter(new Date(v.ThroughDate), new Date()))
          && v.Supplier === this.order.TakenViaSupplier);

        this.serialisedItems = loaded.collections.SerialisedItems as SerialisedItem[];

        if (this.orderItem.SerialisedItem) {
          this.serialisedItems.push(this.orderItem.SerialisedItem);
        }
      });
  }

  private onSave() {

    if (this.orderItem.InvoiceItemType !== this.partItemType &&
      this.orderItem.InvoiceItemType !== this.partItemType) {
      this.orderItem.QuantityOrdered = '1';
    }
  }
}
