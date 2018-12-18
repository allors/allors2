import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, SearchFactory, Loaded, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Facility, Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, SalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedInventoryItem, VatRate, VatRegime } from '../../../../../domain';
import { And, ContainedIn, Equals, Fetch, PullRequest, TreeNode, Sort, Filter } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { CreateData, EditData, ObjectData } from 'src/allors/material/base/services/object';
import { SalesOrderItemEditComponent } from '../../salesorderitem/edit/salesorderitem-edit.module';

@Component({
  templateUrl: './salesinvoiceitem-edit.component.html',
  providers: [ContextService]

})
export class SalesInvoiceItemEditComponent
  implements OnInit, OnDestroy {
  public m: Meta;
  public title: string;

  public invoice: SalesInvoice;
  public invoiceItem: SalesInvoiceItem;
  public orderItem: SalesOrderItem;
  public inventoryItems: InventoryItem[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public goods: Good[];
  public invoiceItemTypes: InvoiceItemType[];
  public productItemType: InvoiceItemType;
  public facilities: Facility[];
  public goodsFacilityFilter: SearchFactory;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<SalesOrderItemEditComponent>,
    public refreshService: RefreshService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
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
            pull.SalesInvoice({
              // TODO:
              object: id,
              include: {
                VatRegime: x,
              }
            }),
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
          this.title = 'Add invoice Item';
          this.invoiceItem = this.allors.context.create('SalesInvoiceItem') as SalesInvoiceItem;
          this.invoice.AddSalesInvoiceItem(this.invoiceItem);
        } else {
          this.title = 'Edit invoice Item';
          if (this.invoiceItem.InvoiceItemType === this.productItemType) {
            this.goodSelected(this.invoiceItem.Product);
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(product: Product): void {

    this.invoiceItem.InvoiceItemType = this.productItemType;

    const { pull } = this.metaService;

    const pulls = [
      pull.Good({
        object: product.id,
        fetch: {
          // TODO:
          // InventoryItemsWhereGood
        }
      })
    ];

    this.allors.context.load('Pull', new PullRequest({ pulls })).subscribe(
      (loaded) => {
        this.inventoryItems = loaded.collections
          .inventoryItem as InventoryItem[];
        if (this.inventoryItems[0].objectType.name === 'SerialisedInventoryItem') {
          this.serialisedInventoryItem = this
            .inventoryItems[0] as SerialisedInventoryItem;
        }
        if (this.inventoryItems[0].objectType.name === 'NonSerialisedInventoryItem') {
          this.nonSerialisedInventoryItem = this
            .inventoryItems[0] as NonSerialisedInventoryItem;
        }
      },
      (error: Error) => {
        this.errorService.handle(error);
      },
    );
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

  public save(): void {

    const isNew = this.invoiceItem.isNew;
    this.allors.context.save().subscribe(
      (saved: Saved) => {
        const data: ObjectData = {
          id: this.invoiceItem.id,
          objectType: this.invoiceItem.objectType,
        };

        this.dialogRef.close(data);
      },
      (error: Error) => {
        this.errorService.handle(error);
      },
    );
  }

  public update(): void {

    const isNew = this.invoiceItem.isNew;

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
