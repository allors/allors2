import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService, Field } from "../../../../angular";
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType, SalesInvoice, SalesInvoiceItem, SalesOrder, Singleton, VarianceReason, VatRate, VatRegime, RequestForQuote, ProductQuote, QuoteItem, SalesOrderItem, ProcessFlow, Store, InventoryItem, SerialisedInventoryItem, SalesInvoiceItemType, Product } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="orderItem" (submit)="save()">

    <div class="pad">
      <div *ngIf="orderItem.SalesOrderItemState">
        <a-mat-static [object]="orderItem" [roleType]="m.SalesOrderItem.SalesOrderItemState" display="Name" label="Status"></a-mat-static>
        <!-- <button *ngIf="orderItem.CanExecuteSubmit" mat-button type="button" (click)="submit()">Submit</button>
        <button *ngIf="orderItem.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button> -->
      </div>
      <a-mat-autocomplete *ngIf="!orderItem.ItemType || orderItem.ItemType === productItemType"
      [object]="orderItem" [roleType]="m.SalesOrderItem.Product" [options]="goods" display="Name"
      (onChange)="goodSelected($event)" [filter]="goodsFilter.create()"></a-mat-autocomplete>
      <a-mat-select  [object]="orderItem" [roleType]="m.SalesOrderItem.ItemType" [options]="salesInvoiceItemTypes" display="Name"></a-mat-select>
      <a-mat-input *ngIf="orderItem.ItemType && orderItem.ItemType === productItemType" [object]="orderItem" [roleType]="m.SalesOrderItem.QuantityOrdered"></a-mat-input>
      <a-mat-static *ngIf="serialisedInventoryItem?.ExpectedSalesPrice" [object]="serialisedInventoryItem" [roleType]="m.SerialisedInventoryItem.ExpectedSalesPrice"></a-mat-static>
      <br/>
      <a-mat-input *ngIf="orderItem.ItemType && orderItem.ItemType === productItemType" [object]="orderItem" [roleType]="m.SalesOrderItem.ActualUnitPrice" label="Unit Price"></a-mat-input>
      <a-mat-input *ngIf="orderItem.ItemType && orderItem.ItemType !== productItemType" [object]="orderItem" [roleType]="m.SalesOrderItem.ActualUnitPrice" label="Amount"></a-mat-input>
      <a-mat-static [object]="orderItem" [roleType]="m.SalesOrderItem.UnitVat"></a-mat-static>
      <a-mat-static [object]="orderItem" [roleType]="m.SalesOrderItem.TotalIncVat"></a-mat-static>
      <!-- <mat-input-container>
        <input matInput placeholder="Discount amount" name="discount" [(ngModel)]="discount">
      </mat-input-container>
      <mat-input-container>
        <input matInput placeholder="Surcharge amount" name="surcharge" [(ngModel)]="surcharge">
      </mat-input-container>
      <br/> -->

      <a-mat-static *ngIf="!orderItem.AssignedVatRegime && orderItem.VatRegime" [object]="orderItem.VatRegime" [roleType]="m.VatRegime.VatRate" display="Rate" label="VAT % from order"></a-mat-static>
      <a-mat-select [object]="orderItem" [roleType]="m.SalesOrderItem.AssignedVatRegime" [options]="vatRegimes" display="Name" label="Item VAT regime"></a-mat-select>
      <a-mat-static *ngIf="orderItem.AssignedVatRegime" [object]="orderItem.AssignedVatRegime" [roleType]="m.VatRegime.VatRate" display="Rate"></a-mat-static>
      <a-mat-static *ngIf="quoteItem?.Comment" [object]="quoteItem" [roleType]="m.QuoteItem.Comment" label="Quote Comment"></a-mat-static>
      <a-mat-textarea [object]="orderItem" [roleType]="m.SalesOrderItem.Comment" label="Order Comment"></a-mat-textarea>
      <a-mat-static *ngIf="quoteItem?.InternalComment" [object]="quoteItem" [roleType]="m.QuoteItem.InternalComment" label="Quote Internal Comment"></a-mat-static>
      <a-mat-textarea [object]="orderItem" [roleType]="m.SalesOrderItem.InternalComment" label="Order Internal Comment"></a-mat-textarea>
    </div>

    <mat-divider></mat-divider>

    <mat-card-actions>
      <button mat-button color="primary" type="submit" [disabled]="!form.form.valid">SAVE</button>
      <button mat-button (click)="goBack()" type="button">CANCEL</button>
    </mat-card-actions>
  </form>
</td-layout-card-over>
`,
})
export class SalesOrderItemEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Sales Order Item";
  public subTitle: string;
  public order: SalesOrder;
  public orderItem: SalesOrderItem;
  public quoteItem: QuoteItem;
  public goods: Good[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public discount: number;
  public surcharge: number;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public salesInvoiceItemTypes: SalesInvoiceItemType[];
  public productItemType: SalesInvoiceItemType;

  public goodsFilter: Filter;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.goodsFilter = new Filter({scope: this.scope, objectType: this.m.Good, roleTypes: [this.m.Good.Name]});
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const itemId: string = this.route.snapshot.paramMap.get("itemId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "salesOrder",
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
              new TreeNode({ roleType: m.SalesOrderItem.QuoteItem }),
              new TreeNode({ roleType: m.SalesOrderItem.DiscountAdjustment }),
              new TreeNode({ roleType: m.SalesOrderItem.SurchargeAdjustment }),
              new TreeNode({ roleType: m.SalesOrderItem.DerivedVatRate }),
              new TreeNode({
                nodes: [new TreeNode({ roleType: m.VatRegime.VatRate })],
                roleType: m.SalesOrderItem.VatRegime,
              }),
            ],
            name: "orderItem",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
          new Query(
            {
              name: "vatRates",
              objectType: m.VatRate,
            }),
          new Query(
            {
              name: "vatRegimes",
              objectType: m.VatRegime,
            }),
          new Query(
            {
              name: "salesInvoiceItemTypes",
              objectType: m.SalesInvoiceItemType,
            }),
          ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.order = loaded.objects.salesOrder as SalesOrder;
        this.orderItem = loaded.objects.orderItem as SalesOrderItem;
        this.quoteItem = loaded.objects.quoteItem as QuoteItem;
        this.goods = loaded.collections.goods as Good[];
        this.vatRates = loaded.collections.vatRates as VatRate[];
        this.vatRegimes = loaded.collections.vatRegimes as VatRegime[];
        this.salesInvoiceItemTypes = loaded.collections.salesInvoiceItemTypes as SalesInvoiceItemType[];
        this.productItemType = this.salesInvoiceItemTypes.find((v: SalesInvoiceItemType) => v.UniqueId.toUpperCase() === "0D07F778-2735-44CB-8354-FB887ADA42AD");

        if (!this.orderItem) {
          this.title = "Add Order Item";
          this.orderItem = this.scope.session.create("SalesOrderItem") as SalesOrderItem;
          this.order.AddSalesOrderItem(this.orderItem);
        } else {
          if (this.orderItem.ItemType === this.productItemType) {
            this.update(this.orderItem.Product);
          }
          if (this.orderItem.DiscountAdjustment) {
            this.discount = this.orderItem.DiscountAdjustment.Amount;
          }
          if (this.orderItem.SurchargeAdjustment) {
            this.surcharge = this.orderItem.SurchargeAdjustment.Amount;
          }
        }
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(field: Field) {
    if (field.object) {
      this.update(field.object as Product);
    }
  }

  public save(): void {

    // if (this.discount !== 0) {
    //   const discountAdjustment = this.scope.session.create("DiscountAdjustment") as DiscountAdjustment;
    //   discountAdjustment.Amount = this.discount;
    //   this.orderItem.DiscountAdjustment = discountAdjustment;
    // }

    // if (this.surcharge !== 0) {
    //   const surchargeAdjustment = this.scope.session.create("SurchargeAdjustment") as SurchargeAdjustment;
    //   surchargeAdjustment.Amount = this.surcharge;
    //   this.orderItem.SurchargeAdjustment = surchargeAdjustment;
    // }

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/orders/salesOrder/" + this.order.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  private update(product: Product): void {

    this.orderItem.ItemType = this.productItemType;

    const fetch: Fetch[] = [
      new Fetch({
        id: product.id,
        name: "inventoryItem",
        path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
      }),
    ];

    this.scope
        .load("Pull", new PullRequest({ fetch }))
        .subscribe((loaded: Loaded) => {
          this.inventoryItems = loaded.collections.inventoryItem as InventoryItem[];
          if (this.inventoryItems[0] instanceof SerialisedInventoryItem) {
            this.serialisedInventoryItem = this.inventoryItems[0] as SerialisedInventoryItem;
          }
          if (this.inventoryItems[0] instanceof NonSerialisedInventoryItem) {
            this.nonSerialisedInventoryItem = this.inventoryItems[0] as NonSerialisedInventoryItem;
          }
        },
        (error: Error) => {
          this.errorService.message(error);
          this.goBack();
        },
      );
  }

}
