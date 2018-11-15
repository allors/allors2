import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrderItem, SerialisedInventoryItem, VatRate, VatRegime } from '../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './invoiceitem.component.html',
  providers: [Allors]
})
export class InvoiceItemEditComponent
  implements OnInit, OnDestroy {
  public m: MetaDomain;

  public title = 'Edit Purchase Invoice Item';
  public subTitle: string;
  public invoice: PurchaseInvoice;
  public invoiceItem: PurchaseInvoiceItem;
  public orderItem: PurchaseOrderItem;
  public inventoryItems: InventoryItem[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public goods: Good[];
  public invoiceItemTypes: InvoiceItemType[];
  public productItemType: InvoiceItemType;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public stateService: StateService,
    private dialogService: AllorsMaterialDialogService,
  ) {
    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {
          const id: string = this.route.snapshot.paramMap.get('id');
          const itemId: string = this.route.snapshot.paramMap.get('itemId');
          const m: MetaDomain = this.m;

          const pulls = [
            pull.PurchaseInvoice(
              {
                object: id
              }
            ),
            pull.PurchaseInvoiceItem(
              {
                object: id,
                include: {
                  PurchaseInvoiceItemState: x,
                  PurchaseOrderItem: x,
                  VatRegime: {
                    VatRate: x
                  }
                }
              }),
            pull.Good(),
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
            }),
            pull.VatRate(),
            pull.VatRegime()
          ];

          return scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();

        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
        this.invoiceItem = loaded.objects.invoiceItem as PurchaseInvoiceItem;
        this.orderItem = loaded.objects.orderItem as PurchaseOrderItem;
        this.goods = loaded.collections.goods as Good[];
        this.vatRates = loaded.collections.vatRates as VatRate[];
        this.vatRegimes = loaded.collections.vatRegimes as VatRegime[];
        this.invoiceItemTypes = loaded.collections.invoiceItemTypes as InvoiceItemType[];
        this.productItemType = this.invoiceItemTypes.find(
          (v: InvoiceItemType) => v.UniqueId.toUpperCase() === '0D07F778-2735-44CB-8354-FB887ADA42AD',
        );

        if (!this.invoiceItem) {
          this.title = 'Add invoice Item';
          this.invoiceItem = scope.session.create('PurchaseInvoiceItem') as PurchaseInvoiceItem;
          this.invoice.AddPurchaseInvoiceItem(this.invoiceItem);
        } else {
          if (
            this.invoiceItem.InvoiceItemType === this.productItemType
          ) {
            // TODO:
            // this.goodSelected(this.invoiceItem.Product);
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
    const { pull, scope } = this.allors;

    this.invoiceItem.InvoiceItemType = this.productItemType;

    const pulls = [
      pull.Product({
        object: product.id,
        // TODO:
        // fetch: {   }
      })
    ];

    scope.load('Pull', new PullRequest({ pulls }))
      .subscribe(
        (loaded) => {
          this.inventoryItems = loaded.collections.inventoryItem as InventoryItem[];
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

  public save(): void {

    const { m, pull, scope } = this.allors;

    scope.save().subscribe(
      (saved: Saved) => {
        this.router.navigate(['/accountspayable/invoice/' + this.invoice.id]);
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
          this.router.navigate(['/purchaseinvoice/' + this.invoice.id + '/item/' + this.invoiceItem.id]);
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
