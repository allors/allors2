import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { Router } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole, InventoryItem, SerialisedInventoryItem, NonSerialisedInventoryItem, RequestItem } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

@Component({
  template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button><mat-icon>settings</mat-icon></button>
  </div>
</mat-toolbar>

<div body *ngIf="request" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Request for Quote</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequestState" display="Name" label="Status"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequestNumber"></a-mat-static>
        </div>
        <div *ngIf="quote" layout="row" class="link" [routerLink]="['/orders/productQuote/' + quote.id ]">
          <a-mat-static [object]="quote" [roleType]="m.Quote.QuoteNumber"></a-mat-static>
        </div>
        <div *ngIf="request.Originator?.objectType.name == 'Person'" layout="row" class="link" [routerLink]="['/relations/person/' + request.Originator.id ]">
          <a-mat-static [object]="request" [roleType]="m.Request.Originator" display="displayName" label="From"></a-mat-static>
        </div>
        <div *ngIf="request.Originator?.objectType.name == 'Organisation'" layout="row" class="link" [routerLink]="['/relations/organisation/' + request.Originator.id ]">
          <a-mat-static [object]="request" [roleType]="m.Request.Originator" display="displayName" label="From"></a-mat-static>
        </div>
        <div *ngIf="request.ContactPerson" layout="row" class="link" [routerLink]="['/relations/person/' + request.ContactPerson.id ]">
          <a-mat-static [object]="request" [roleType]="m.Request.ContactPerson" display="displayName"></a-mat-static>
        </div>
        <div *ngIf="request.FullfillContactMechanism" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.FullfillContactMechanism" display="displayName" label="Reply to"></a-mat-static>
        </div>
        <div *ngIf="request.EmailAddress" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.EmailAddress" display="displayName"></a-mat-static>
        </div>
        <div *ngIf="request.TelephoneNumber" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.TelephoneCountryCode"></a-mat-static>
          <a-mat-static [object]="request" [roleType]="m.Request.TelephoneNumber"></a-mat-static>
        </div>
        <div *ngIf="request.Description" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.Description"></a-mat-static>
        </div>
        <div *ngIf="request.Comment" layout="row">
          <a-mat-textarea [object]="request" [roleType]="m.Request.Comment"></a-mat-textarea>
        </div>
        <div *ngIf="request.InternalComment" layout="row">
          <a-mat-textarea [object]="request" [roleType]="m.Request.InternalComment"></a-mat-textarea>
        </div>
        <div *ngIf="request.RequestDate" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequestDate"></a-mat-static>
        </div>
        <div *ngIf="request.RequiredResponseDate" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequiredResponseDate"></a-mat-static>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> created: {{ request.CreationDate | date}} by {{ request.CreatedBy.displayName}}</p>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> last modified: {{ request.LastModifiedDate | date}} by {{ request.LastModifiedBy.displayName}}</p>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="request.CanWriteRequestItems" mat-button [routerLink]="['/request/' + request.id ]">Edit</button>
        <button *ngIf="request.CanExecuteCreateQuote && request.RequestItems.length > 0" mat-button (click)="createQuote()">Create Quote</button>
      </mat-card-actions>
    </mat-card>

    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Request Items</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>
        <ng-template tdLoading="request.RequestItems">
          <mat-list class="will-load">
            <div class="mat-padding" *ngIf="request.RequestItems.length === 0" layout="row" layout-align="center center">
              <h3>No Items to display.</h3>
            </div>

            <ng-template let-requestItem let-last="last" ngFor [ngForOf]="request.RequestItems">
              <mat-list-item>
                <h3 *ngIf="requestItem.Product" mat-line [routerLink]="['/request/' + request.id + '/item/' + requestItem.id ]">{{requestItem.Quantity}} * {{requestItem.Product.Name}}</h3>
                <h4 *ngIf="serialisedInventoryItem?.ExpectedSalesPrice" mat-line>{{ serialisedInventoryItem.ExpectedSalesPrice }}</h4>
                <span>
                  <button mat-icon-button [mat-menu-trigger-for]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu x-position="before" #menu="matMenu">
                    <a [routerLink]="['/request/' + request.id + '/item/' + requestItem.id ]" mat-menu-item>Edit</a>
                    <button  mat-menu-item (click)="deleteRequestItem(requestItem)" [disabled]="!requestItem.CanExecuteDelete">Delete</button>
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
        <button *ngIf="request.CanWriteRequestItems" mat-button [routerLink]="['/request/' + request.id +'/item']">+ Add</button>
      </mat-card-actions>
    </mat-card>
  </div>

  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Status History</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <ng-template tdLoading="request.RequestStatuses">
          <mat-list class="will-load">

            <ng-template let-status let-last="last" ngFor [ngForOf]="request.RequestStatuses">
              <mat-list-item>
                <p mat-line class="mat-caption">{{ status.RequestObjectState.Name }} </p>
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
export class RequestOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Requests Overview";
  public request: RequestForQuote;
  public quote: ProductQuote;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;

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

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
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
                  new TreeNode({ roleType: m.RequestItem.Product }),
                ],
                roleType: m.Request.RequestItems,
              }),
              new TreeNode({ roleType: m.Request.RequestItems }),
              new TreeNode({ roleType: m.Request.Originator }),
              new TreeNode({ roleType: m.Request.ContactPerson }),
              new TreeNode({ roleType: m.Request.RequestState }),
              new TreeNode({ roleType: m.Request.Currency }),
              new TreeNode({ roleType: m.Request.CreatedBy }),
              new TreeNode({ roleType: m.Request.LastModifiedBy }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.Request.FullfillContactMechanism,
              }),
            ],
            name: "request",
          }),
        ];

        const quoteFetch: Fetch = new Fetch({
          id,
          name: "quote",
          path: new Path({ step: m.RequestForQuote.QuoteWhereRequest }),
        });

        if (id != null) {
          fetch.push(quoteFetch);
        }

        return this.scope
          .load("Pull", new PullRequest({ fetch }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.request = loaded.objects.request as RequestForQuote;
        this.quote = loaded.objects.quote as ProductQuote;
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

  public createQuote(): void {

    this.scope.invoke(this.request.CreateQuote)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open("Quote successfully created.", "close", { duration: 5000 });
        this.gotoQuote();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public deleteRequestItem(requestItem: RequestItem): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this item?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(requestItem.Delete)
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

  public gotoQuote(): void {

    const fetch: Fetch[] = [new Fetch({
      id: this.request.id,
      name: "quote",
      path: new Path({ step: this.m.RequestForQuote.QuoteWhereRequest }),
    })];

    this.scope.load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded: Loaded) => {
        const quote = loaded.objects.quote as ProductQuote;
        this.router.navigate(["/orders/productQuote/" + quote.id]);
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }
}
