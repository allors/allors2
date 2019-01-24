import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, SearchFactory, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Facility, Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, SalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedInventoryItem, VatRate, VatRegime, SerialisedItem, Part } from '../../../../../domain';
import { And, ContainedIn, Equals, PullRequest, Sort, Filter } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { CreateData, EditData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './salesinvoiceitem-edit.component.html',
  providers: [ContextService]

})
export class SalesInvoiceItemEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  invoice: SalesInvoice;
  invoiceItem: SalesInvoiceItem;
  orderItem: SalesOrderItem;
  inventoryItems: InventoryItem[];
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  goods: Good[];
  invoiceItemTypes: InvoiceItemType[];
  productItemType: InvoiceItemType;
  facilities: Facility[];
  goodsFacilityFilter: SearchFactory;
  part: Part;
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];

  private previousProduct;
  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<SalesInvoiceItemEditComponent>,
    public refreshService: RefreshService,
    public metaService: MetaService,
    private errorService: ErrorService,
    public stateService: StateService,
  ) {
    this.m = this.metaService.m;

    this.goodsFacilityFilter = new SearchFactory({
      objectType: this.m.Good,
      roleTypes: [this.m.Good.Name],
      post: (predicate: And) => {
        const extent = new Filter({
          objectType: this.m.VendorProduct,
          predicate: new Equals({ propertyType: this.m.VendorProduct.InternalOrganisation, value: this.stateService.internalOrganisationId }),
        });

        predicate.operands.push(new ContainedIn({ propertyType: this.m.Product.VendorProductsWhereProduct, extent }));
      },
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;
          const { id } = this.data;

          const pulls = [
            pull.SalesInvoiceItem({
              object: id,
              include:
              {
                SalesInvoiceItemState: x,
                Facility: {
                  Owner: x,
                },
                VatRegime: {
                  VatRate: x,
                }
              }
            }),
            pull.Good({
              sort: [
                new Sort(m.Good.Name),
              ],
            }),
            pull.SalesInvoiceItem({
              object: id,
              fetch: {
                SalesInvoiceWhereSalesInvoiceItem: {
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
            pull.VatRegime(),
            pull.Facility({
              include: {
                Owner: x
              },
              sort: [
                new Sort(m.Facility.Name),
              ],
            })
          ];

          if (this.data.associationId) {
            pulls.push(
              pull.SalesInvoice({
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

        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
        this.invoiceItem = loaded.objects.SalesInvoiceItem as SalesInvoiceItem;
        this.orderItem = loaded.objects.SalesOrderItem as SalesOrderItem;
        this.goods = loaded.collections.Goods as Good[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.productItemType = this.invoiceItemTypes.find(
          (v: InvoiceItemType) =>
            v.UniqueId.toUpperCase() ===
            '0D07F778-2735-44CB-8354-FB887ADA42AD',
        );

        if (isCreate) {
          this.title = 'Add sales invoice Item';
          this.invoiceItem = this.allors.context.create('SalesInvoiceItem') as SalesInvoiceItem;
          this.invoice.AddSalesInvoiceItem(this.invoiceItem);
        } else {
          this.title = 'Edit invoice Item';

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
      }, this.errorService.handler);
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
        const data: ObjectData = {
          id: this.invoiceItem.id,
          objectType: this.invoiceItem.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goodSelected(object: any) {
    if (object) {
      this.refreshSerialisedItems(object as Product);
    }
  }

  public serialisedItemSelected(serialisedItem: SerialisedItem): void {
    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
  }

  public facilitySelected(facility: Facility): void {

    if (facility !== undefined) {
      this.goodsFacilityFilter = new SearchFactory({
        objectType: this.m.Good,
        roleTypes: [this.m.Good.Name],
        post: (predicate: And) => {
          const extent = new Filter({
            objectType: this.m.VendorProduct,
            predicate: new Equals({ propertyType: this.m.VendorProduct.InternalOrganisation, object: facility.Owner }),
          });

          predicate.operands.push(new ContainedIn({ propertyType: this.m.Product.VendorProductsWhereProduct, extent }));
        },
      });
    }
  }

  private refreshSerialisedItems(good: Product): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Good({
        object: good.id,
        fetch: {
          Part: {
            include: {
              SerialisedItems: x
            }
          }
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.part = loaded.objects.Part as Part;
        this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true);

        if (this.invoiceItem.Product !== this.previousProduct) {
          this.invoiceItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.invoiceItem.Product;
        }

      }, this.errorService.handler);
  }

  private onSave() {

    if (this.invoiceItem.InvoiceItemType !== this.productItemType) {
      this.invoiceItem.Quantity = 1;
    }
  }
}
