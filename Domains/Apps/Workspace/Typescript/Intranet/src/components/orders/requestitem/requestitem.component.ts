import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole, RequestItem, InventoryItem, SerialisedInventoryItem, NonSerialisedInventoryItem, UnitOfMeasure, Product } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="requestItem" (submit)="save()">

    <div class="pad">
      <div *ngIf="requestItem.RequestItemState">
        <a-mat-static [object]="requestItem" [roleType]="m.RequestItem.RequestItemState" display="Name" label="Status"></a-mat-static>
         <button *ngIf="requestItem.CanExecuteSubmit" mat-button type="button" (click)="submit()">Submit</button>
        <button *ngIf="requestItem.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button>
        <button *ngIf="requestItem.CanExecuteHold" mat-button type="button" (click)="hold()">Pending Customer</button>
      </div>

      <a-mat-autocomplete [object]="requestItem" [roleType]="m.RequestItem.Product" [options]="goods" display="Name" [filter]="goodsFilter.create()"
        (onSelect)="goodSelected($event)"></a-mat-autocomplete>
      <a-mat-input [object]="requestItem" [roleType]="m.RequestItem.Quantity"></a-mat-input>
      <a-mat-select [object]="requestItem" [roleType]="m.RequestItem.UnitOfMeasure" [options]="unitsOfMeasure" display="Name"></a-mat-select>
      <a-mat-static *ngIf="serialisedInventoryItem?.ExpectedSalesPrice" [object]="serialisedInventoryItem" [roleType]="m.SerialisedInventoryItem.ExpectedSalesPrice"></a-mat-static>
      <a-mat-input [object]="requestItem" [roleType]="m.RequestItem.MaximumAllowedPrice"></a-mat-input>
      <a-mat-datepicker [object]="requestItem" [roleType]="m.RequestItem.RequiredByDate"></a-mat-datepicker>
      <a-mat-textarea [object]="requestItem" [roleType]="m.RequestItem.Comment"></a-mat-textarea>
      <a-mat-textarea [object]="requestItem" [roleType]="m.RequestItem.InternalComment"></a-mat-textarea>
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
export class RequestItemEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Request Item";
  public subTitle: string;
  public request: RequestForQuote;
  public requestItem: RequestItem;
  public goods: Good[];
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
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
            name: "requestForQuote",
          }),
          new Fetch({
            id: itemId,
            include: [new TreeNode({ roleType: m.RequestItem.RequestItemState })],
            name: "requestItem",
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
        this.request = loaded.objects.requestForQuote as RequestForQuote;
        this.requestItem = loaded.objects.requestItem as RequestItem;
        this.goods = loaded.collections.goods as Good[];
        this.unitsOfMeasure = loaded.collections.unitsOfMeasure as UnitOfMeasure[];
        const piece = this.unitsOfMeasure.find((v: UnitOfMeasure) => v.UniqueId.toUpperCase() === "F4BBDB52-3441-4768-92D4-729C6C5D6F1B");

        if (!this.requestItem) {
          this.title = "Add Request Item";
          this.requestItem = this.scope.session.create("RequestItem") as RequestItem;
          this.requestItem.UnitOfMeasure = piece;
          this.request.AddRequestItem(this.requestItem);
        } else {
          this.goodSelected(this.requestItem.Product);
        }
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
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

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public submit(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.requestItem.Submit)
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
      this.scope.invoke(this.requestItem.Cancel)
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

  public hold(): void {
    const holdFn: () => void = () => {
      this.scope.invoke(this.requestItem.Hold)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully held.", "close", { duration: 5000 });
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
                holdFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            holdFn();
          }
        });
    } else {
      holdFn();
    }
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/orders/request/" + this.request.id]);
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
