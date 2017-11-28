import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { AllorsService, ErrorService, Loaded, Scope } from "@allors";
import { And, ContainedIn, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "@allors";
import { SalesInvoice } from "@allors";
import { MetaDomain } from "@allors";

interface SearchData {
  company: string;
  reference: string;
  invoiceNumber: string;
}

@Component({
  templateUrl: "./invoices-overview.component.html",
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
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    public dialogService: TdDialogService,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
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
        const m: MetaDomain = this.allors.meta;

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
