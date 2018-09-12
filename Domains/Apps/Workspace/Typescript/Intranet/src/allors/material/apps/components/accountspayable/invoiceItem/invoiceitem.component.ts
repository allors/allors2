import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrderItem, SerialisedInventoryItem, VatRate, VatRegime } from '../../../../../domain';
import { Fetch, Path, PullRequest, TreeNode, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './invoiceitem.component.html',
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
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public stateService: StateService,
    private dialogService: AllorsMaterialDialogService,
  ) {
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { pull } = this.dataService;

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

          return this.scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();

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
          this.invoiceItem = this.scope.session.create('PurchaseInvoiceItem') as PurchaseInvoiceItem;
          this.invoice.AddPurchaseInvoiceItem(this.invoiceItem);
        } else {
          if (
            this.invoiceItem.InvoiceItemType === this.productItemType
          ) {
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
    const { pull } = this.dataService;

    this.invoiceItem.InvoiceItemType = this.productItemType;

    const pulls = [
      pull.Product({
        object: product.id,
        path: { TODO  }
      })
    ];

    this.scope.load('Pull', new PullRequest({ pulls }))
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
    this.scope.save().subscribe(
      (saved: Saved) => {
        this.router.navigate(['/accountspayable/invoice/' + this.invoice.id]);
      },
      (error: Error) => {
        this.errorService.handle(error);
      },
    );
  }

  public update(): void {
    const isNew = this.invoiceItem.isNew;

    this.scope
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
