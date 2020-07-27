import * as moment from 'moment/moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope, SearchFactory } from '../../../../../angular';
import { InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, PurchaseOrder, PurchaseOrderItem, SerialisedInventoryItem, VatRate, VatRegime, Part, Product, SerialisedItem, SupplierOffering, IrpfRegime, Facility, UnifiedGood } from '../../../../../domain';
import { PullRequest, Equals, Sort, IObject, And, ContainedIn, Filter, LessThan, Or, Not, Exists, GreaterThan, ISessionObject } from '../../../../../framework';
import { ObjectData, SaveService, FiltersService } from '../../../../../material';
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
  supplierOfferings: SupplierOffering[];
  selectedFacility: Facility;

  private subscription: Subscription;
  partsFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<PurchaseOrderItemEditComponent>,
    public metaService: MetaService,
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
                VatRegime: {
                  VatRate: x,
                },
                IrpfRegime: {
                  IrpfRate: x,
                },
              }
            }),
            pull.PurchaseOrderItem({
              object: id,
              fetch: {
                PurchaseOrderWherePurchaseOrderItem: {
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
              sort: new Sort(m.InvoiceItemType.Name)
            }),
            pull.VatRegime({
              sort: new Sort(m.VatRegime.Name)
            }),
            pull.IrpfRegime({
              sort: new Sort(m.IrpfRegime.Name)
            }),
            pull.Facility({
              sort: new Sort(m.Facility.Name)
            }),
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.SupplierOffering({
                name: `AllSupplierOfferings`,
                include: {
                  Part: x,
                  Supplier: x
                }
              }),
              pull.PurchaseOrder({
                object: this.data.associationId,
                include: {
                  VatRegime: x,
                  IrpfRegime: x,
                  TakenViaSupplier: x
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

        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.facilities = loaded.collections.Facilities as Facility[];

        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.partItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'ff2b943d-57c9-4311-9c56-9ff37959653b');
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '0d07f778-2735-44cb-8354-fb887ada42ad');
        this.serviceItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'a4d2e6d0-c6c1-46ec-a1cf-3a64822e7a9e');
        this.timeItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'da178f93-234a-41ed-815c-819af8ca4e6f');

        if (isCreate) {
          this.supplierOfferings = loaded.collections.AllSupplierOfferings as SupplierOffering[];
        } else {
          this.supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
        }

        this.partsFilter = new SearchFactory({
          objectType: this.m.NonUnifiedPart,
          roleTypes: [this.m.NonUnifiedPart.Name, this.m.NonUnifiedPart.SearchString],
          post: (predicate: And) => {
            predicate.operands.push(new ContainedIn({
              propertyType: this.m.NonUnifiedPart.SupplierOfferingsWherePart,
              extent: new Filter({
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
          this.title = 'Add purchase order Item';
          this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
          this.orderItem = this.allors.context.create('PurchaseOrderItem') as PurchaseOrderItem;
          this.selectedFacility = this.order.StoredInFacility;
          this.order.AddPurchaseOrderItem(this.orderItem);
        } else {
          this.order = this.orderItem.PurchaseOrderWherePurchaseOrderItem;
          this.selectedFacility = this.orderItem.StoredInFacility;

          if (this.orderItem.Part) {
            this.unifiedGood = this.orderItem.Part.objectType.name === m.UnifiedGood.name;
            this.nonUnifiedPart = this.orderItem.Part.objectType.name === m.NonUnifiedPart.name;
            this.updateFromPart(this.orderItem.Part);
          }

          if (this.orderItem.CanWriteQuantityOrdered) {
            this.title = 'Edit purchase order Item';
          } else {
            this.title = 'View purchase order Item';
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
      .subscribe((loaded) => {

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
        this.supplierOffering = supplierOfferings.find(v => moment(v.FromDate).isBefore(moment())
          && (!v.ThroughDate || moment(v.ThroughDate).isAfter(moment()))
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
