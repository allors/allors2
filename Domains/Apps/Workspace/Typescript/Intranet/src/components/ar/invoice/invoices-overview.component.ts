import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { TdDialogService, TdMediaService } from "@covalent/core";

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort } from "@allors/framework";

interface SearchData {
  company: string;
  reference: string;
  invoiceNumber: string;
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
    <button mat-icon-button>
      <mat-icon>settings</mat-icon>
    </button>
  </div>
</mat-toolbar>

<mat-card>
  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <mat-input-container>
        <input fxFlex matInput placeholder="Organisation" formControlName="company">
      </mat-input-container>
      <mat-input-container>
        <input fxFlex matInput placeholder="Number" formControlName="invoiceNumber">
      </mat-input-container>
      <mat-input-container>
        <input fxFlex matInput placeholder="Customer Reference" formControlName="reference">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
    </form>
  </div>

  <mat-divider></mat-divider>

  <ng-template tdLoading="organisations">
    <mat-list class="will-load">
      <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
        <h3>No sales invoices to display.</h3>
      </div>
      <ng-template let-invoice let-last="last" ngFor [ngForOf]="data">
        <mat-list-item>

          <h3 [routerLink]="['/ar/invoice/' + invoice.id ]" mat-line> {{invoice.BillToCustomer.displayName}}</h3>
          <h4 mat-line> {{invoice.InvoiceNumber }}: {{invoice.CustomerReference}} ({{invoice.SalesInvoiceState.Name}})</h4>
          <p mat-line hide-gt-md class="mat-caption"> last modified: {{ invoice.LastModifiedDate | timeAgo }} </p>

          <span flex></span>

          <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
            <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ invoice.CreationDate | date }} </div>
            <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ invoice.LastModifiedDate | timeAgo }} </div>
          </span>

          <span>
            <button mat-icon-button [mat-menu-trigger-for]="menu">
              <mat-icon>more_vert</mat-icon>
            </button>
            <mat-menu x-position="before" #menu="matMenu">
              <a [routerLink]="['/ar/invoice/' + invoice.id ]" mat-menu-item>Details</a>
              <a [routerLink]="['/printinvoice/' + invoice.id ]" mat-menu-item>Print</a>
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

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/invoice']">
  <mat-icon>add</mat-icon>
</a>
`,
})
export class InvoicesOverviewComponent implements AfterViewInit, OnDestroy {

  public searchForm: FormGroup;

  public title: string = "Sales Invoices";
  public data: SalesInvoice[];
  public filtered: SalesInvoice[];
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
      invoiceNumber: [""],
      reference: [""],
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

        if (data.invoiceNumber) {
          const like: string = "%" + data.invoiceNumber + "%";
          predicates.push(new Like({ roleType: m.SalesInvoice.InvoiceNumber, value: like }));
        }

        if (data.company) {
          const partyQuery: Query = new Query({
            objectType: m.Party, predicate: new Like({
              roleType: m.Party.PartyName, value: data.company.replace("*", "%") + "%",
            }),
          });

          const containedIn: ContainedIn = new ContainedIn({ roleType: m.SalesInvoice.BillToCustomer, query: partyQuery });
          predicates.push(containedIn);
        }

        if (data.reference) {
          const like: string = data.reference.replace("*", "%") + "%";
          predicates.push(new Like({ roleType: m.SalesInvoice.CustomerReference, value: like }));
        }

        const query: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.SalesInvoice.BillToCustomer }),
              new TreeNode({ roleType: m.SalesInvoice.SalesInvoiceState }),
            ],
            name: "invoices",
            objectType: m.SalesInvoice,
            page: new Page({ skip: 0, take }),
            predicate,
            sort: [new Sort({ roleType: m.SalesInvoice.InvoiceNumber, direction: "Desc" })],
          })];

        return this.scope.load("Pull", new PullRequest({ query }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.invoices as SalesInvoice[];
        this.total = loaded.values.invoices_total;
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
    this.titleService.setTitle("Sales Invoices");
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public onView(invoice: SalesInvoice): void {
    this.router.navigate(["/ar/invoices/" + invoice.id]);
  }

  private more(): void {
    this.page$.next(this.data.length + 50);
  }
}
