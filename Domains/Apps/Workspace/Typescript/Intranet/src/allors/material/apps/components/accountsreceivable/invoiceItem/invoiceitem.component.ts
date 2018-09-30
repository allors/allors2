import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, SearchFactory, Loaded, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { Facility, Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, SalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedInventoryItem, VatRate, VatRegime } from '../../../../../domain';
import { And, ContainedIn, Equals, Fetch, PullRequest, TreeNode, Sort, Filter } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './invoiceitem.component.html',
  providers: [Allors]

})
export class InvoiceItemEditComponent
  implements OnInit, OnDestroy {
  public m: MetaDomain;

  public title = 'Edit Sales Invoice Item';
  public subTitle: string;
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

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService,

  ) {
    this.m = this.allors.m;

    this.refresh$ = new BehaviorSubject<Date>(undefined);
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

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {
          const id: string = this.route.snapshot.paramMap.get('id');
          const itemId: string = this.route.snapshot.paramMap.get('itemId');

          const pulls = [
            pull.SalesInvoice({
              object: id,
              include: {
                VatRegime: x,
              }
            }),
            pull.InvoiceItem({
              object: itemId,
              include:
              {
                SalesInvoiceItem_SalesInvoiceItemState: x,
                SalesInvoiceItem_Facility: {
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

          return scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.invoiceItem = loaded.objects.invoiceItem as SalesInvoiceItem;
        this.orderItem = loaded.objects.orderItem as SalesOrderItem;
        this.goods = loaded.collections.goods as Good[];
        this.vatRates = loaded.collections.vatRates as VatRate[];
        this.vatRegimes = loaded.collections.vatRegimes as VatRegime[];
        this.facilities = loaded.collections.facilities as Facility[];
        this.invoiceItemTypes = loaded.collections.invoiceItemTypes as InvoiceItemType[];
        this.productItemType = this.invoiceItemTypes.find(
          (v: InvoiceItemType) =>
            v.UniqueId.toUpperCase() ===
            '0D07F778-2735-44CB-8354-FB887ADA42AD',
        );

        if (!this.invoiceItem) {
          this.title = 'Add invoice Item';
          this.invoiceItem = scope.session.create('SalesInvoiceItem') as SalesInvoiceItem;
          this.invoice.AddSalesInvoiceItem(this.invoiceItem);
        } else {
          if (this.invoiceItem.InvoiceItemType === this.productItemType) {
            this.goodSelected(this.invoiceItem.Product);
          }
        }
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(product: Product): void {
    const { scope } = this.allors;

    this.invoiceItem.InvoiceItemType = this.productItemType;

    const { pull } = this.allors;

    const pulls = [
      pull.Good({
        object: product.id,
        fetch: {
          // TODO:
          // InventoryItemsWhereGood
        }
      })
    ];

    scope.load('Pull', new PullRequest({ pulls })).subscribe(
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
        this.goBack();
      },
    );
  }

  public facilitySelected(facility: Facility): void {
    const { scope } = this.allors;

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
    const { scope } = this.allors;

    const isNew = this.invoiceItem.isNew;
    scope.save().subscribe(
      (saved: Saved) => {
        this.router.navigate(['/accountsreceivable/invoice/' + this.invoice.id]);
      },
      (error: Error) => {
        this.errorService.handle(error);
      },
    );
  }

  public update(): void {
    const { scope } = this.allors;

    const isNew = this.invoiceItem.isNew;

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        if (isNew) {
          this.router.navigate(['/salesinvoice/' + this.invoice.id + '/item/' + this.invoiceItem.id]);
        } else {
          this.refresh();
        }
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
