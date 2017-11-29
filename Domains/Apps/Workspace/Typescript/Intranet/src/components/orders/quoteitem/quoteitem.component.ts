import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole, QuoteItem, RequestItem, SerialisedInventoryItem, InventoryItem, NonSerialisedInventoryItem, UnitOfMeasure, Product } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="quoteItem" (submit)="save()">

    <div class="pad">
      <div *ngIf="quoteItem.QuoteItemState">
        <a-mat-static [object]="quoteItem" [roleType]="m.QuoteItem.QuoteItemState" display="Name" label="Status"></a-mat-static>
        <button *ngIf="quoteItem.CanExecuteSubmit" mat-button type="button" (click)="submit()">Submit</button>
        <button *ngIf="quoteItem.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button>
      </div>

      <a-mat-autocomplete [object]="quoteItem" [roleType]="m.QuoteItem.Product" [options]="goods" display="Name" [filter]="goodsFilter.create()"
        (onSelect)="goodSelected($event)"></a-mat-autocomplete>
      <a-mat-input [object]="quoteItem" [roleType]="m.QuoteItem.Quantity"></a-mat-input>
      <a-mat-select [object]="quoteItem" [roleType]="m.QuoteItem.UnitOfMeasure" [options]="unitsOfMeasure" display="Name"></a-mat-select>
      <a-mat-input [object]="quoteItem" [roleType]="m.QuoteItem.UnitPrice"></a-mat-input>
      <a-mat-static *ngIf="requestItem" [object]="requestItem" [roleType]="m.RequestItem.MaximumAllowedPrice"></a-mat-static>
      <a-mat-static *ngIf="serialisedInventoryItem?.ExpectedSalesPrice" [object]="serialisedInventoryItem" [roleType]="m.SerialisedInventoryItem.ExpectedSalesPrice"></a-mat-static>
      <a-mat-datepicker [object]="quoteItem" [roleType]="m.QuoteItem.RequiredByDate" disabled="true"></a-mat-datepicker>
      <a-mat-datepicker [object]="quoteItem" [roleType]="m.QuoteItem.EstimatedDeliveryDate"></a-mat-datepicker>
      <a-mat-static *ngIf="requestItem?.Comment" [object]="requestItem" [roleType]="m.RequestItem.Comment"></a-mat-static>
      <a-mat-textarea [object]="quoteItem" [roleType]="m.QuoteItem.Comment"></a-mat-textarea>
      <a-mat-static *ngIf="requestItem?.InternalComment" [object]="requestItem" [roleType]="m.RequestItem.InternalComment"></a-mat-static>
      <a-mat-textarea [object]="quoteItem" [roleType]="m.QuoteItem.InternalComment"></a-mat-textarea>
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
export class QuoteItemEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Quote Item";
  public subTitle: string;
  public quote: ProductQuote;
  public quoteItem: QuoteItem;
  public requestItem: RequestItem;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public goods: Good[];
  public unitsOfMeasure: UnitOfMeasure[];

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
            name: "productQuote",
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({ roleType: m.QuoteItem.QuoteItemState }),
              new TreeNode({ roleType: m.QuoteItem.RequestItem }),
            ],
            name: "quoteItem",
          }),
          new Fetch({
            id: itemId,
            name: "requestItem",
            path: new Path({ step: m.QuoteItem.RequestItem }),
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
              sort: [new Sort({ roleType: m.Good.Name, direction: "Asc" })],
            }),
            new Query(
              {
                name: "unitsOfMeasure",
                objectType: m.UnitOfMeasure,
              }),
          ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.quote = loaded.objects.productQuote as ProductQuote;
        this.quoteItem = loaded.objects.quoteItem as QuoteItem;
        this.requestItem = loaded.objects.requestItem as RequestItem;
        this.goods = loaded.collections.goods as Good[];
        this.unitsOfMeasure = loaded.collections.unitsOfMeasure as UnitOfMeasure[];
        const piece = this.unitsOfMeasure.find((v: UnitOfMeasure) => v.UniqueId.toUpperCase() === "F4BBDB52-3441-4768-92D4-729C6C5D6F1B");

        if (!this.quoteItem) {
          this.title = "Add Quote Item";
          this.quoteItem = this.scope.session.create("QuoteItem") as QuoteItem;
          this.quoteItem.UnitOfMeasure = piece;
          this.quote.AddQuoteItem(this.quoteItem);
        } else {
          this.goodSelected(this.quoteItem.Product);
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

  public goodSelected(product: Product): void {

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

  public submit(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.quoteItem.Submit)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully submitted.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                submitFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            submitFn();
          }
        });
    } else {
      submitFn();
    }
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.quoteItem.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/orders/productQuote/" + this.quote.id]);
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
}
