import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest } from "@allors/framework";

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

<div body *ngIf="invoice" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Sales Invoice</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.SalesInvoiceState" display="Name" label="Status"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.InvoiceNumber"></a-mat-static>
        </div>
        <div *ngIf="invoice.SalesOrder" layout="row" class="link" [routerLink]="['/orders/salesOrder/' + invoice.SalesOrder.id ]">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.SalesOrder" display="OrderNumber"></a-mat-static>
        </div>
        <div layout="row" class="link" [routerLink]="['/relations/organisation/' + invoice.BillToCustomer.id ]">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.BillToCustomer" display="displayName" label="Customer"></a-mat-static>
        </div>
        <div *ngIf="invoice.ContactPerson" layout="row" class="link" [routerLink]="['/relations/person/' + invoice.ContactPerson.id ]">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.ContactPerson" display="displayName"></a-mat-static>
        </div>
        <div *ngIf="invoice.BillToContactMechanism" layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.BillToContactMechanism" display="displayName" label="Bill to"></a-mat-static>
        </div>
        <div *ngIf="invoice.Description" layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.Description"></a-mat-static>
        </div>
        <div *ngIf="order?.Comment" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.Comment" label="Order Comment"></a-mat-static>
        </div>
        <div *ngIf="invoice.Comment" layout="row">
          <a-mat-textarea [object]="invoice" [roleType]="m.SalesInvoice.Comment" label="Invoice Comment"></a-mat-textarea>
        </div>
        <div *ngIf="order?.InternalComment" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.InternalComment" label="Order Internal Comment"></a-mat-static>
        </div>
        <div *ngIf="invoice.InternalComment" layout="row">
          <a-mat-textarea [object]="invoice" [roleType]="m.SalesInvoice.InternalComment" label="Invoice Internal Comment"></a-mat-textarea>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> created: {{ invoice.CreationDate | date}} by {{ invoice.CreatedBy.displayName}}</p>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> last modified: {{ invoice.LastModifiedDate | date}} by {{ invoice.LastModifiedBy.displayName}}</p>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="invoice.CanWriteSalesInvoiceItems" mat-button [routerLink]="['/invoice/' + invoice.id]">Edit</button>
        <button mat-button [routerLink]="['/printinvoice/' + invoice.id ]">Print</button>
      </mat-card-actions>
    </mat-card>

    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Invoice Items</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>
        <ng-template tdLoading="invoice.SalesInvoiceItems">
          <mat-list class="will-load">
            <div class="mat-padding" *ngIf="invoice.SalesInvoiceItems.length === 0" layout="row" layout-align="center center">
              <h3>No Items to display.</h3>
            </div>

            <ng-template let-invoiceItem let-last="last" ngFor [ngForOf]="invoice.SalesInvoiceItems">
              <mat-list-item>
                <h3 *ngIf="invoiceItem.Product && invoice.CanWriteSalesInvoiceItems" mat-line [routerLink]="['/invoice/' + invoice.id + '/item/' + invoiceItem.id ]">{{invoiceItem.Quantity}} * {{invoiceItem.Product.Name}}</h3>
                <h3 *ngIf="invoiceItem.Product && !invoice.CanWriteSalesInvoiceItems" mat-line>{{invoiceItem.Quantity}} * {{invoiceItem.Product.Name}}</h3>

                <h3 *ngIf="!invoiceItem.Product && invoice.CanWriteSalesInvoiceItems" mat-line [routerLink]="['/invoice/' + invoice.id + '/item/' + invoiceItem.id ]">{{invoiceItem.SalesInvoiceItemType.Name}}</h3>
                <h3 *ngIf="!invoiceItem.Product && !invoice.CanWriteSalesInvoiceItems" mat-line>{{invoiceItem.SalesInvoiceItemType.Name}}</h3>

                <h4 mat-line>Total ex VAT: {{invoiceItem.TotalExVat}}</h4>

                <span>
                  <button mat-icon-button [mat-menu-trigger-for]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu x-position="before" #menu="matMenu">
                    <a *ngIf="invoice.CanWriteSalesInvoiceItems" [routerLink]="['/invoice/' + invoice.id + '/item/' + invoiceItem.id ]" mat-menu-item>Edit</a>
                    <button  mat-menu-item (click)="deleteInvoiceItem(invoiceItem)" [disabled]="!invoiceItem.CanExecuteDelete">Delete</button>
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
        <button *ngIf="invoice.CanWriteSalesInvoiceItems" mat-button [routerLink]="['/invoice/' + invoice.id +'/item']">+ Add</button>
      </mat-card-actions>
    </mat-card>
  </div>

  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Status History</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <ng-template tdLoading="invoice.InvoiceStatuses">
          <mat-list class="will-load">

            <ng-template let-status let-last="last" ngFor [ngForOf]="invoice.InvoiceStatuses">
              <mat-list-item>
                <p mat-line class="mat-caption">{{ status.SalesInvoiceObjectState.Name }} </p>
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
export class InvoiceOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Sales Invoice Overview";
  public order: SalesOrder;
  public invoice: SalesInvoice;
  public goods: Good[] = [];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
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
                  new TreeNode({ roleType: m.SalesInvoiceItem.Product }),
                  new TreeNode({ roleType: m.SalesInvoiceItem.SalesInvoiceItemType }),
                ],
                roleType: m.SalesInvoice.SalesInvoiceItems,
              }),
              new TreeNode({ roleType: m.SalesInvoice.ContactPerson }),
              new TreeNode({ roleType: m.SalesInvoice.BillToCustomer }),
              new TreeNode({ roleType: m.SalesInvoice.SalesInvoiceState }),
              new TreeNode({ roleType: m.SalesInvoice.CreatedBy }),
              new TreeNode({ roleType: m.SalesInvoice.LastModifiedBy }),
              new TreeNode({ roleType: m.SalesInvoice.SalesOrder }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesInvoice.BillToContactMechanism,
              }),
            ],
            name: "invoice",
          }),
          new Fetch({
            id,
            name: "order",
            path: new Path({ step: m.SalesInvoice.SalesOrder }),
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.order = loaded.objects.order as SalesOrder;
        this.invoice = loaded.objects.invoice as SalesInvoice;
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

  public deleteInvoiceItem(invoiceItem: SalesInvoiceItem): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this item?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(invoiceItem.Delete)
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
}
