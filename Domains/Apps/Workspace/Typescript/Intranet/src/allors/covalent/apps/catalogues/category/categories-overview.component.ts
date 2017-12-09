import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { ContactMechanism, Currency, Good, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, SalesInvoice, SalesInvoiceItem, SalesOrder, VatRate, VatRegime, Catalogue, Singleton, ProductCategory, CatScope, Locale } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

interface SearchData {
  name: string;
}

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


<mat-card>
  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <mat-input-container>
        <input matInput placeholder="Name" formControlName="name">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
    </form>
  </div>

  <mat-divider></mat-divider>
  <ng-template tdLoading="data">
    <mat-list class="will-load">
      <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
        <h3>No categories to display.</h3>
      </div>
      <ng-template let-category let-last="last" ngFor [ngForOf]="data">
        <mat-list-item>
          <mat-icon *ngIf="!category.CategoryImage" mat-list-avatar>photo_camera</mat-icon>
          <img *ngIf="category.CategoryImage" mat-list-avatar src="http://localhost:5000/Media/Download/{{category.CategoryImage.UniqueId}}?revision={{category.CategoryImage.Revision}}"
          />

          <h3 mat-line [routerLink]="'/category/' + [category.id]"> {{category.Name}}</h3>
          <p mat-line> {{category.Description}} </p>

          <span>
            <button mat-icon-button [mat-menu-trigger-for]="menu">
            <mat-icon>more_vert</mat-icon>
            </button>
            <mat-menu x-position="before" #menu="matMenu">
            <a [routerLink]="'/category/' + [category.id]" mat-menu-item>Edit</a>
            <button mat-menu-item (click)="delete(category)">Delete</button>
            </mat-menu>
          </span>
        </mat-list-item>
        <mat-divider *ngIf="!last" mat-inset></mat-divider>
      </ng-template>
    </mat-list>
  </ng-template>
</mat-card>

<mat-card body tdMediaToggle="gt-xs" [mediaClasses]="['push']" *ngIf="this.data && this.data.length !== total">
  <mat-card-content>
    <button mat-button (click)="more()">More</button> {{this.data?.length}}/{{total}}
  </mat-card-content>
</mat-card>

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/category']">
  <mat-icon>add</mat-icon>
</a>
`,
})
export class CategoriesOverviewComponent implements AfterViewInit, OnDestroy {

  public title: string = "Categories";
  public total: number;
  public searchForm: FormGroup;
  public data: ProductCategory[];
  public filtered: ProductCategory[];
  private refresh$: BehaviorSubject<Date>;
  private page$: BehaviorSubject<number>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.titleService.setTitle("Categories");

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable
    .combineLatest(search$, this.page$, this.refresh$)
    .scan(([previousData, previousTake, previousDate]: [SearchData, number, Date], [data, take, date]: [SearchData, number, Date]): [SearchData, number, Date] => {
      return [
        data,
        data !== previousData ? 50 : take,
        date,
      ];
    }, [] as [SearchData, number, Date]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        if (data.name) {
          const like: string = data.name.replace("*", "%") + "%";
          predicates.push(new Like({ roleType: m.ProductCategory.Name, value: like }));
        }

        const query: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.ProductCategory.CategoryImage }),
              new TreeNode({ roleType: m.ProductCategory.LocalisedNames }),
              new TreeNode({ roleType: m.ProductCategory.LocalisedDescriptions }),
            ],
            name: "categories",
            objectType: m.ProductCategory,
            page: new Page({ skip: 0, take }),
            predicate,
          })];

        return this.scope.load("Pull", new PullRequest({ query }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.categories as ProductCategory[];
        this.total = loaded.values.categories_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public goBack(): void {
    window.history.back();
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

  public delete(category: ProductCategory): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this category?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  public onView(category: ProductCategory): void {
    this.router.navigate(["/category/" + category.id]);
  }
}
