import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { TdDialogService, TdMediaService } from "@covalent/core";

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

interface SearchData {
  company: string;
  description: string;
  quoteNumber: string;
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
        <input fxFlex matInput placeholder="Organisation" formControlName="company">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
      <mat-input-container>
        <input fxFlex matInput placeholder="Number" formControlName="quoteNumber">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
      <mat-input-container>
        <input fxFlex matInput placeholder="Description" formControlName="description">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
    </form>
  </div>

  <mat-divider></mat-divider>

  <ng-template tdLoading="organisations">
    <mat-list class="will-load">
      <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
        <h3>No quotes to display.</h3>
      </div>
      <ng-template let-quote let-last="last" ngFor [ngForOf]="data">
        <mat-list-item>

          <h3 mat-line [routerLink]="['/orders/productQuote/' + quote.id ]"> {{quote.Receiver.displayName}}</h3>
          <h4 mat-line [routerLink]="['/orders/productQuote/' + quote.id ]"> {{quote.QuoteNumber }}: {{quote.Description}} ({{quote.QuoteState.Name}})</h4>
          <p mat-line hide-gt-md class="mat-caption"> last modified: {{ quote.LastModifiedDate | timeAgo }} </p>

          <span flex></span>

          <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
                <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ quote.CreationDate | date }} </div>
                <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ quote.LastModifiedDate | timeAgo }} </div>
              </span>

          <span>
          <button mat-icon-button [mat-menu-trigger-for]="menu">
          <mat-icon>more_vert</mat-icon>
          </button>
          <mat-menu x-position="before" #menu="matMenu">
          <a [routerLink]="['/orders/productQuote/' + quote.id ]" mat-menu-item>Details</a>
          <a [routerLink]="['/printproductquote/' + quote.id ]" mat-menu-item>Print</a>
          <!-- <button mat-menu-item (click)="delete(organisation)">Delete</button> -->
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

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/productQuote']">
  <mat-icon>add</mat-icon>
</a>
`,
})
export class ProductQuotesOverviewComponent implements AfterViewInit, OnDestroy {

  public searchForm: FormGroup;

  public title: string = "Quotes";
  public data: ProductQuote[];
  public filtered: ProductQuote[];
  public total: number;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    public dialogService: TdDialogService,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      company: [""],
      description: [""],
      quoteNumber: [""],
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

        if (data.quoteNumber) {
          const like: string = "%" + data.quoteNumber + "%";
          predicates.push(new Like({ roleType: m.ProductQuote.QuoteNumber, value: like }));
        }

        if (data.company) {
          const partyQuery: Query = new Query({
            objectType: m.Party, predicate: new Like({
              roleType: m.Party.PartyName, value: data.company.replace("*", "%") + "%",
            }),
          });

          const containedIn: ContainedIn = new ContainedIn({ roleType: m.ProductQuote.Receiver, query: partyQuery });
          predicates.push(containedIn);
        }

        if (data.description) {
          const like: string = data.description.replace("*", "%") + "%";
          predicates.push(new Like({ roleType: m.ProductQuote.Description, value: like }));
        }

        const query: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.ProductQuote.Receiver }),
              new TreeNode({ roleType: m.ProductQuote.QuoteState }),
            ],
            name: "quotes",
            objectType: m.ProductQuote,
            page: new Page({ skip: 0, take }),
            predicate,
            sort: [new Sort({ roleType: m.ProductQuote.QuoteNumber, direction: "Desc" })],
          })];

        return this.scope.load("Pull", new PullRequest({ query }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.quotes as ProductQuote[];
        this.total = loaded.values.quotes_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  public goBack(): void {
    window.history.back();
  }

  public ngAfterViewInit(): void {
    this.titleService.setTitle("Requests");
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public onView(quote: ProductQuote): void {
    this.router.navigate(["/orders/productQuotes/" + quote.id]);
  }

  private more(): void {
    this.page$.next(this.data.length + 50);
  }
}
