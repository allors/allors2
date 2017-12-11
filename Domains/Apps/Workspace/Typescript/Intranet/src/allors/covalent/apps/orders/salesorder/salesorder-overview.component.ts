import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType, SalesInvoice, SalesInvoiceItem, SalesOrder, Singleton, VarianceReason, VatRate, VatRegime, RequestForQuote, ProductQuote, QuoteItem, SalesOrderItem, ProcessFlow } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
      <mat-icon>arrow_back</mat-icon>
    </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button>
      <mat-icon>settings</mat-icon>
    </button>
  </div>
</mat-toolbar>

<div body *ngIf="order" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Sales Order</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.SalesOrderState" display="Name" label="Status"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.OrderNumber"></a-mat-static>
        </div>
        <div *ngIf="order.Quote" layout="row" class="link" [routerLink]="['/orders/productQuote/' + order.Quote.id ]">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.Quote" display="QuoteNumber"></a-mat-static>
        </div>
        <div *ngIf="salesInvoice" layout="row" class="link" [routerLink]="['/ar/invoice/' + salesInvoice.id ]">
          <a-mat-static [object]="salesInvoice" [roleType]="m.SalesInvoice.InvoiceNumber"></a-mat-static>
        </div>
        <div *ngIf="order.ContactPerson" layout="row" class="link" [routerLink]="['/relations/person/' + order.ContactPerson.id ]">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.ContactPerson" display="displayName"></a-mat-static>
        </div>
        <div layout="row" class="link" [routerLink]="['/relations/organisation/' + order.ShipToCustomer.id ]">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.ShipToCustomer" display="displayName" label="Customer"></a-mat-static>
        </div>
        <div *ngIf="order.ShipToAddress" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.ShipToAddress" display="displayName" label="Ship to"></a-mat-static>
        </div>
        <div *ngIf="order.Description" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.Description"></a-mat-static>
        </div>
        <div *ngIf="quote?.Comment" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.Comment" label="Quote Comment"></a-mat-static>
        </div>
        <div *ngIf="order.Comment" layout="row">
          <a-mat-textarea [object]="order" [roleType]="m.SalesOrder.Comment" label="Order Comment"></a-mat-textarea>
        </div>
        <div *ngIf="quote?.InternalComment" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.InternalComment" label="Quote Internal Comment"></a-mat-static>
        </div>
        <div *ngIf="order.InternalComment" layout="row">
          <a-mat-textarea [object]="order" [roleType]="m.SalesOrder.InternalComment" label="Order Internal Comment"></a-mat-textarea>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> created: {{ order.CreationDate | date}} by {{ order.CreatedBy.displayName}}</p>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> last modified: {{ order.LastModifiedDate | date}} by {{ order.LastModifiedBy.displayName}}</p>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="order.CanWriteSalesOrderItems" mat-button [routerLink]="['/salesOrder/' + order.id]">Edit</button>
        <button mat-button [routerLink]="['/printsalesorder/' + order.id ]">Print</button>
        <button *ngIf="order.CanExecuteShip" mat-button (click)="ship()">Ship to customer</button>
      <button *ngIf="payFirst && order.CanExecuteComplete && order.ValidOrderItems.length > 0"
          mat-button (click)="createInvoice()">Create Invoice</button>
      </mat-card-actions>
    </mat-card>

    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Order Items</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>
        <ng-template tdLoading="order.SalesOrderItems">
          <mat-list class="will-load">
            <div class="mat-padding" *ngIf="order.SalesOrderItems.length === 0" layout="row" layout-align="center center">
              <h3>No Items to display.</h3>
            </div>

            <ng-template let-orderItem let-last="last" ngFor [ngForOf]="order.SalesOrderItems">
              <mat-list-item>
                <h3 *ngIf="orderItem.Product && order.CanWriteSalesOrderItems" mat-line [routerLink]="['/salesOrder/' + order.id + '/item/' + orderItem.id ]">{{orderItem.QuantityOrdered}} * {{orderItem.Product.Name}}</h3>
                <h3 *ngIf="orderItem.Product && !order.CanWriteSalesOrderItems" mat-line>{{orderItem.QuantityOrdered}} * {{orderItem.Product.Name}}</h3>

                <h3 *ngIf="!orderItem.Product && order.CanWriteSalesOrderItems" mat-line  [routerLink]="['/salesOrder/' + order.id + '/item/' + orderItem.id ]">{{orderItem.ItemType.Name}}</h3>
                <h3 *ngIf="!orderItem.Product && !order.CanWriteSalesOrderItems" mat-line>{{orderItem.ItemType.Name}}</h3>

                <h4 mat-line>Total ex VAT: {{orderItem.TotalExVat}}</h4>

                <span>
                  <button mat-icon-button [mat-menu-trigger-for]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu x-position="before" #menu="matMenu">
                    <a *ngIf="order.CanWriteSalesOrderItems" [routerLink]="['/salesOrder/' + order.id + '/item/' + orderItem.id ]" mat-menu-item>Edit</a>
                    <button  mat-menu-item (click)="deleteOrderItem(orderItem)" [disabled]="!orderItem.CanExecuteDelete">Delete</button>
                  </mat-menu>
                </span>
              </mat-list-item>
              <mat-divider *ngIf="!last" mat-inset></mat-divider>
            </ng-template>
          </mat-list>
        </ng-template>
      </mat-card-content>
      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="order.CanWriteSalesOrderItems" mat-button [routerLink]="['/salesOrder/' + order.id +'/item']">+ Add</button>
      </mat-card-actions>
    </mat-card>
  </div>

  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Status History</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <ng-template tdLoading="order.OrderStatuses">
          <mat-list class="will-load">

            <ng-template let-status let-last="last" ngFor [ngForOf]="order.OrderStatuses">
              <mat-list-item>
                <p mat-line class="mat-caption">{{ status.SalesOrderObjectState.Name }} </p>
                <p mat-line hide-gt-md class="mat-caption"> Start: {{ status.StartDateTime | date}} </p>

                <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
                  <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Start: {{ status.StartDateTime | date }}</div>
                </span>

              </mat-list-item>
            </ng-template>
          </mat-list>
        </ng-template>
      </mat-card-content>

    </mat-card>
  </div>
</div>
`,
})
export class SalesOrderOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Sales Order Overview";
  public quote: ProductQuote;
  public order: SalesOrder;
  public orderItems: SalesOrderItem[] = [];
  public goods: Good[] = [];
  public salesInvoice: SalesInvoice;
  public processFlows: ProcessFlow[];
  public payFirst: ProcessFlow;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    public dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open("items saved", "close", { duration: 1000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {
        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.SalesOrderItem.Product }),
                  new TreeNode({ roleType: m.SalesOrderItem.ItemType }),
                  new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
                ],
                roleType: m.SalesOrder.SalesOrderItems,
              }),
              new TreeNode({ roleType: m.SalesOrder.ContactPerson }),
              new TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
              new TreeNode({ roleType: m.SalesOrder.SalesOrderState }),
              new TreeNode({ roleType: m.SalesOrder.CreatedBy }),
              new TreeNode({ roleType: m.SalesOrder.LastModifiedBy }),
              new TreeNode({ roleType: m.SalesOrder.Quote }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesOrder.ShipToAddress,
              }),
            ],
            name: "order",
          }),
        ];

        const salesInvoiceFetch: Fetch = new Fetch({
          id,
          name: "salesInvoice",
          path: new Path({ step: m.SalesOrder.SalesInvoicesWhereSalesOrder }),
        });

        if (id != null) {
          fetch.push(salesInvoiceFetch);
        }

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
            new Query(
            {
              name: "processFlows",
              objectType: m.ProcessFlow,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.order = loaded.objects.order as SalesOrder;
        this.salesInvoice = loaded.objects.salesInvoice as SalesInvoice;
        this.processFlows = loaded.collections.processFlows as ProcessFlow[];
        this.payFirst = this.processFlows.find((v: ProcessFlow) => v.UniqueId.toUpperCase() === "AB01CCC2-6480-4FC0-B20E-265AFD41FAE2");

        if (this.order) {
          this.orderItems = this.order.SalesOrderItems;
        }
      },
      (error: any) => {
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

  public goBack(): void {
    window.history.back();
  }

  public deleteOrderItem(orderItem: SalesOrderItem): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this item?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(orderItem.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public ship(): void {
    this.scope.invoke(this.order.Ship)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Customer Shipment successfully created.", "close", { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public createInvoice(): void {
    this.scope.invoke(this.order.Complete)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Invoice successfully created.", "close", { duration: 5000 });
        this.gotoInvoice();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public gotoInvoice(): void {

      const fetch: Fetch[] = [new Fetch({
        id: this.order.id,
        name: "invoices",
        path: new Path({ step: this.m.SalesOrder.SalesInvoicesWhereSalesOrder }),
      })];

      this.scope.load("Pull", new PullRequest({ fetch }))
        .subscribe((loaded: Loaded) => {
          const invoices = loaded.collections.invoices as SalesInvoice[];
          if (invoices.length === 1) {
            this.router.navigate(["/ar/invoice/" + invoices[0].id]);
          }
        },
        (error: any) => {
          this.errorService.message(error);
          this.goBack();
        });
    }
  }
