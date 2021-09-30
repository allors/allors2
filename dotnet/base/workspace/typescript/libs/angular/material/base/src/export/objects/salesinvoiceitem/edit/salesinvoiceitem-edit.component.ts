import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { InventoryItem, Part, Facility, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItem, VatRegime, IrpfRegime, InvoiceItemType, SupplierOffering, UnifiedGood, Product, SalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedItemAvailability, NonUnifiedPart, Organisation } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { FetcherService, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

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

  goodsFilter: SearchFactory;
  internalOrganisation: Organisation;
  showIrpf: boolean;
  vatRegimeInitialRole: VatRegime;
  irpfRegimeInitialRole: IrpfRegime;
  
  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
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

    this.subscription = combineLatest([this.refreshService.refresh$])
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;
          const { id } = this.data;

          const pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.warehouses,
            pull.SerialisedItemAvailability(),
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
              sort: new Sort(m.InvoiceItemType.Name),
            }),
            pull.IrpfRegime({ 
              sort: new Sort(m.IrpfRegime.Name) }),
            pull.SerialisedItemAvailability(),
          ];

          if (!isCreate) {
            pulls.push(
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
                  DerivedVatRegime: {
                    VatRates: x,
                  },
                  DerivedIrpfRegime: {
                    IrpfRates: x,
                  }
                }
              }),
              pull.SalesInvoiceItem({
                object: id,
                fetch: {
                  SalesInvoiceWhereSalesInvoiceItem: {
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
              pull.SalesInvoice({
                object: this.data.associationId,
                include: {
                  DerivedVatRegime: {
                    VatRates: x,
                  },
                  DerivedIrpfRegime: {
                    IrpfRates: x,
                  }
                }
              })
            );
          }

          this.goodsFilter = Filters.goodsFilter(m);

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
        this.invoiceItem = loaded.objects.SalesInvoiceItem as SalesInvoiceItem;
        this.orderItem = loaded.objects.SalesOrderItem as SalesOrderItem;
        this.parts = loaded.collections.NonUnifiedParts as NonUnifiedPart[];
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
          this.vatRegimeInitialRole = this.invoice.DerivedVatRegime;
          this.irpfRegimeInitialRole = this.invoice.DerivedIrpfRegime;
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
