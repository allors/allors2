import * as moment from 'moment/moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope, FetcherService, SearchFactory } from '../../../../../angular';
import { PurchaseOrder, PurchaseOrderItem, VatRate, VatRegime, Part, SupplierOffering, SerialisedItem, Organisation, UnifiedGood, NonUnifiedPart, Facility } from '../../../../../domain';
import { PullRequest, IObject, Equals, And, LessThan, Or, Not, Exists, GreaterThan, Filter, ContainedIn } from '../../../../../framework';
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
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  discount: number;
  surcharge: number;
  supplierOfferings: SupplierOffering[];
  supplierOffering: SupplierOffering;
  selectedFacility: Facility;
  addFacility = false;
  facilities: Facility[];

  private subscription: Subscription;
  serialisedItems: SerialisedItem[];
  isSerialised: boolean;
  internalOrganisation: Organisation;
  sparePartsFilter: SearchFactory;
  nonUnifiedPart: boolean;
  unifiedGood: boolean;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseOrderItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public filtersService: FiltersService,
    private saveService: SaveService,
    private snackBar: MatSnackBar,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x, m } = this.metaService;

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
                StoredInFacility: x,
                VatRate: x,
                VatRegime: {
                  VatRate: x,
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
            pull.Facility(),
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
                  TakenViaSupplier: x
                }
              })
            );
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

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.facilities = loaded.collections.Facilities as Facility[];

        if (isCreate) {
          this.supplierOfferings = loaded.collections.AllSupplierOfferings as SupplierOffering[];
        } else {
          this.supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
        }

        this.sparePartsFilter = new SearchFactory({
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
          this.title = 'Add Order Item';
          this.orderItem = this.allors.context.create('PurchaseOrderItem') as PurchaseOrderItem;
          this.selectedFacility = this.order.StoredInFacility;
          this.order.AddPurchaseOrderItem(this.orderItem);

        } else {
          this.orderItem = loaded.objects.PurchaseOrderItem as PurchaseOrderItem;
          this.selectedFacility = this.orderItem.StoredInFacility;

          if (this.orderItem.Part) {
            this.unifiedGood = this.orderItem.Part.objectType.name === m.UnifiedGood.name;
            this.nonUnifiedPart = this.orderItem.Part.objectType.name === m.NonUnifiedPart.name;

            if (this.unifiedGood) {
              this.updateFromPart(this.orderItem.Part);
            }

            if (this.nonUnifiedPart) {
              this.updateFromSparePart(this.orderItem.Part);
            }
          }

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
      this.unifiedGood = this.orderItem.Part.objectType.name === this.m.UnifiedGood.name;
      this.nonUnifiedPart = this.orderItem.Part.objectType.name === this.m.NonUnifiedPart.name;

      this.updateFromPart(part);
    } else {
        this.orderItem.QuantityOrdered = '1';
    }
  }

  public sparePartSelected(part: Part): void {
    if (part) {
      this.unifiedGood = this.orderItem.Part.objectType.name === this.m.UnifiedGood.name;
      this.nonUnifiedPart = this.orderItem.Part.objectType.name === this.m.NonUnifiedPart.name;

      this.updateFromSparePart(part);
    }
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
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
          id: this.orderItem.id,
          objectType: this.orderItem.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public update(): void {
    const { context } = this.allors;

    this.onSave();

    context
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  private onSave() {
    this.orderItem.StoredInFacility = this.selectedFacility;
  }

  private updateFromSparePart(part: Part) {

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
        this.isSerialised = part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';

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

  private updateFromPart(part: Part) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Part(
        {
          object: part,
          fetch: {
            SerialisedItems: {
              include: {
                OwnedBy: x,
                SerialisedInventoryItemsWhereSerialisedItem: x,
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
        this.isSerialised = part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';

        if (this.isSerialised) {
          this.orderItem.QuantityOrdered = '1';
        }

        const serialisedItems = loaded.collections.SerialisedItems as SerialisedItem[];
        this.serialisedItems = serialisedItems.filter(v => v.SerialisedInventoryItemsWhereSerialisedItem.length === 0 || v.SerialisedInventoryItemsWhereSerialisedItem[0].Quantity === 0);

        if (this.orderItem.SerialisedItem) {
          this.serialisedItems.push(this.orderItem.SerialisedItem);
        }
      });
  }
}
