import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
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
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance,
  Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship,
  OrganisationRole, Party, PartyContactMechanism, Person, ProcessFlow, ProductCategory, ProductCharacteristic,
  ProductCharacteristicValue, ProductFeature, ProductQuote, ProductType, QuoteItem, RequestForQuote, SalesInvoice, SalesInvoiceItem, SalesOrder, SalesOrderItem,
  Singleton, TelecommunicationsNumber, VarianceReason, VatRate, VatRegime } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

interface SearchData {
  firstName: string;
  lastName: string;
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
    <button mat-icon-button matTooltip="Export to CSV" [routerLink]="['/export/people']"><mat-icon>cloud_download</mat-icon></button>
  </div>
</mat-toolbar>

<mat-card>
  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <mat-input-container>
        <input matInput placeholder="First Name" formControlName="firstName">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
      <mat-input-container>
        <input matInput placeholder="Last Name" formControlName="lastName">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
    </form>
  </div>

  <mat-divider></mat-divider>

  <mat-card-content>
    <ng-template tdLoading="people">
      <mat-list class="will-load">
        <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
          <h3>No people to display.</h3>
        </div>
        <ng-template let-person let-last="last" ngFor [ngForOf]="data">
          <mat-list-item>

            <mat-icon *ngIf="!person.Picture" mat-list-avatar>person</mat-icon>
            <img *ngIf="person.Picture" mat-list-avatar src="http://localhost:5000/Media/Download/{{person.Picture.UniqueId}}?revision={{person.Picture.Revision}}"
            />


            <h3 mat-line [routerLink]="['/relations/person/' + person.id]"> {{person.PartyName}} </h3>
            <p *ngIf="person.GeneralPhoneNumber" mat-line> {{person.GeneralPhoneNumber.CountryCode}} {{person.GeneralPhoneNumber.AreaCode}} {{person.GeneralPhoneNumber.ContactNumber}}

              <p mat-line hide-gt-md class="mat-caption"> last login: {{ person.LastModifiedDate | timeAgo }} </p>

              <span flex></span>

              <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ person.CreationDate | date }} </div>
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ person.LastModifiedDate | timeAgo }} </div>
            </span>

              <span>
              <button mat-icon-button [mat-menu-trigger-for]="menu">
              <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu x-position="before" #menu="matMenu">
              <a [routerLink]="['/relations/person/' + person.id]" mat-menu-item>Details</a>
              <button  mat-menu-item (click)="delete(person)" [disabled]="!person.CanExecuteDelete">Delete</button>
              </mat-menu>
            </span>

          </mat-list-item>
          <mat-divider *ngIf="!last" mat-inset></mat-divider>
        </ng-template>
      </mat-list>
    </ng-template>

  </mat-card-content>
</mat-card>

<mat-card body tdMediaToggle="gt-xs" [mediaClasses]="['push']" *ngIf="this.data && this.data.length !== total">
  <mat-card-content>
    <button mat-button (click)="more()">More</button> {{this.data?.length}}/{{total}}
  </mat-card-content>
</mat-card>

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/person']">
  <mat-icon>add</mat-icon>
</a>
`,
})
export class PeopleOverviewComponent implements AfterViewInit, OnDestroy {

  public total: number;
  public title: string = "People";
  public searchForm: FormGroup;
  public data: Person[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    private snackBarService: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope()
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      firstName: [""],
      lastName: [""],
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

        if (data.firstName) {
          const like: string = "%" + data.firstName + "%";
          predicates.push(new Like({ roleType: m.Person.FirstName, value: like }));
        }

        if (data.lastName) {
          const like: string = data.lastName.replace("*", "%") + "%";
          predicates.push(new Like({ roleType: m.Person.LastName, value: like }));
        }

        const query: Query[] = [new Query(
          {
            name: "people",
            objectType: m.Person,
            predicate,
            page: new Page({ skip: 0, take }),
            include: [
              new TreeNode({ roleType: m.Person.Picture }),
              new TreeNode({ roleType: m.Person.GeneralPhoneNumber }),
            ],
            sort: [new Sort({ roleType: m.Person.FirstName })],
          })];

        return this.scope.load("Pull", new PullRequest({ query }));

      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.people as Person[];
        this.total = loaded.values.people_total;
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public delete(person: Person): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this person?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(person.Delete)
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

  public onView(person: Person): void {
    this.router.navigate(["/relations/peson/" + person.id]);
  }
}
