import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, TestScope, MetaService, RefreshService, Context, Saved, NavigationService, Action, Invoked, SearchFactory } from '@allors/angular/services/core';
import { ElectronicAddress, Enumeration, Employment, Person, Party, Organisation, CommunicationEventPurpose, FaceToFaceCommunication, CommunicationEventState, OrganisationContactRelationship, InventoryItem, InternalOrganisation, InventoryItemTransaction, InventoryTransactionReason, Part, Facility, Lot, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItemState, SerialisedInventoryItemState, NonSerialisedInventoryItem, ContactMechanism, LetterCorrespondence, PartyContactMechanism, PostalAddress, OrderAdjustment, OrganisationContactKind, PartyRate, TimeFrequency, RateType, PhoneCommunication, TelecommunicationsNumber, PositionType, PositionTypeRate, ProductIdentification, ProductIdentificationType, ProductType, SerialisedItemCharacteristicType, PurchaseInvoiceApproval, PurchaseOrderApprovalLevel1, PurchaseOrderApprovalLevel2, PurchaseOrder, PurchaseOrderItem, VatRegime, IrpfRegime, InvoiceItemType, SupplierOffering, UnifiedGood, Product, ProductQuote, QuoteItem, RequestItem, UnitOfMeasure, RequestItemState, RequestState, QuoteItemState, QuoteState, SalesOrderItemState, SalesOrderState, ShipmentItemState, ShipmentState, Receipt, SalesInvoice, PaymentApplication, RepeatingPurchaseInvoice, DayOfWeek, RepeatingSalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedItemAvailability, NonUnifiedPart } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, ids } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId, FetcherService, FiltersService } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort, And, ContainedIn, Extent, LessThan, Or, Not, Exists, GreaterThan } from '@allors/data/system';


@Component({
  templateUrl: './salesinvoiceitem-edit.component.html',
  providers: [ContextService]

})
export class SalesInvoiceItemEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  invoice: SalesInvoice;
  invoiceItem: SalesInvoiceItem;
  orderItem: SalesOrderItem;
  inventoryItems: InventoryItem[];
  vatRegimes: VatRegime[];
  irpfRegimes: IrpfRegime[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  invoiceItemTypes: InvoiceItemType[];
  productItemType: InvoiceItemType;
  facilities: Facility[];
  goodsFacilityFilter: SearchFactory;
  part: Part;
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];
  serialisedItemAvailabilities: SerialisedItemAvailability[];

  private previousProduct;
  private subscription: Subscription;
  parts: NonUnifiedPart[];
  partItemType: InvoiceItemType;
  supplierOffering: SupplierOffering;
  inRent: SerialisedItemAvailability;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<SalesInvoiceItemEditComponent>,
    public refreshService: RefreshService,
    public metaService: MetaService,
    private fetcher: FetcherService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;

    this.goodsFacilityFilter = new SearchFactory({
      objectType: this.m.Good,
      roleTypes: [this.m.Good.Name],
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;
          const { id } = this.data;

          const pulls = [
            this.fetcher.warehouses,
            pull.SalesInvoiceItem({
              object: id,
              include:
              {
                SalesInvoiceItemState: x,
                SerialisedItem: x,
                NextSerialisedItemAvailability: x,
                Facility: {
                  Owner: x,
                },
                VatRegime: {
                  VatRate: x,
                },
                IrpfRegime: {
                  IrpfRate: x,
                },
              }
            }),
            pull.SalesInvoiceItem({
              object: id,
              fetch: {
                SalesInvoiceWhereSalesInvoiceItem: {
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
            pull.SerialisedItemAvailability(),
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
              sort: new Sort(m.InvoiceItemType.Name),
            }),
            pull.VatRegime({ 
              sort: new Sort(m.VatRegime.Name) }),
            pull.IrpfRegime({ 
              sort: new Sort(m.IrpfRegime.Name) }),
            pull.SerialisedItemAvailability(),
          ];

          if (this.data.associationId) {
            pulls.push(
              pull.SalesInvoice({
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

        this.invoiceItem = loaded.objects.SalesInvoiceItem as SalesInvoiceItem;
        this.orderItem = loaded.objects.SalesOrderItem as SalesOrderItem;
        this.parts = loaded.collections.NonUnifiedParts as NonUnifiedPart[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.irpfRegimes = loaded.collections.IrpfRegimes as IrpfRegime[];
        this.serialisedItemAvailabilities = loaded.collections.SerialisedItemAvailabilities as SerialisedItemAvailability[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === '0d07f778-2735-44cb-8354-fb887ada42ad');
        this.partItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId === 'ff2b943d-57c9-4311-9c56-9ff37959653b');

        const serialisedItemAvailabilities = loaded.collections.SerialisedItemAvailabilities as SerialisedItemAvailability[];
        this.inRent = serialisedItemAvailabilities.find((v: SerialisedItemAvailability) => v.UniqueId === 'ec87f723-2284-4f5c-ba57-fcf328a0b738');

        if (isCreate) {
          this.title = 'Add sales invoice Item';
          this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
          this.invoiceItem = this.allors.context.create('SalesInvoiceItem') as SalesInvoiceItem;
          this.invoice.AddSalesInvoiceItem(this.invoiceItem);
        } else {
          this.title = 'Edit invoice Item';
          this.invoice = this.invoiceItem.SalesInvoiceWhereSalesInvoiceItem;

          this.previousProduct = this.invoiceItem.Product;
          this.serialisedItem = this.invoiceItem.SerialisedItem;

          if (this.invoiceItem.InvoiceItemType === this.productItemType) {
            this.goodSelected(this.invoiceItem.Product);
          }

          if (this.invoiceItem.CanWriteQuantity) {
            this.title = 'Edit invoice Item';
          } else {
            this.title = 'View invoice Item';
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

  public goodSelected(object: any) {
    if (object) {
      this.refreshSerialisedItems(object as Product);
    }
  }

  public serialisedItemSelected(serialisedItem: ISessionObject): void {
    const unifiedGood = this.invoiceItem.Product as UnifiedGood;
    this.serialisedItem = unifiedGood.SerialisedItems.find((v) => v === serialisedItem);
    this.invoiceItem.AssignedUnitPrice = this.serialisedItem.ExpectedSalesPrice;
    this.invoiceItem.Quantity = '1';
}

  private refreshSerialisedItems(good: Product): void {

    const { pull, x } = this.metaService;

    const unifiedGoodPullName = `${this.m.UnifiedGood.name}_items`;
    const nonUnifiedGoodPullName = `${this.m.NonUnifiedGood.name}_items`;

    const pulls = [
      pull.NonUnifiedGood({
        name: nonUnifiedGoodPullName,
        object: good.id,
        fetch: {
          Part: {
            SerialisedItems: {
              include: {
                SerialisedItemAvailability: x,
                PartWhereSerialisedItem: x,
              }
            }
          }
        }
      }),
      pull.UnifiedGood({
        name: unifiedGoodPullName,
        object: good.id,
        fetch: {
          SerialisedItems: {
            include: {
              SerialisedItemAvailability: x,
              PartWhereSerialisedItem: x,
            }
          }
        }
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {
        const serialisedItems1 = loaded.collections[unifiedGoodPullName] as SerialisedItem[];
        const serialisedItems2 = loaded.collections[nonUnifiedGoodPullName] as SerialisedItem[];
        const items = serialisedItems1 || serialisedItems2;

        this.serialisedItems = items.filter(v => v.AvailableForSale === true || v.SerialisedItemAvailability === this.inRent);

        if (this.invoiceItem.Product !== this.previousProduct) {
          this.invoiceItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.invoiceItem.Product;
        }

      });
  }
}
