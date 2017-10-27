import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { And, ContainedIn, Contains, Equals, Like, Not, Or, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "@allors";
import { SalesOrder } from "@allors";
import { MetaDomain } from "@allors";

interface SearchData {
  company: string;
  reference: string;
  orderNumber: string;
}

@Component({
  templateUrl: "./salesOrdersOverview.component.html",
})
export class SalesOrdersOverviewComponent implements AfterViewInit, OnDestroy {

  public searchForm: FormGroup;

  public title: string = "Sales Orders";
  public data: SalesOrder[];
  public filtered: SalesOrder[];
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
      orderNumber: [""],
      reference: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        if (data.orderNumber) {
          const like: string = "%" + data.orderNumber + "%";
          predicates.push(new Like({ roleType: m.SalesOrder.OrderNumber, value: like }));
        }

        if (data.company) {
          const partyQuery: Query = new Query({
            objectType: m.Party, predicate: new Like({
              roleType: m.Party.PartyName, value: data.company.replace("*", "%") + "%",
            }),
          });

          const containedIn: ContainedIn = new ContainedIn({ roleType: m.SalesOrder.ShipToCustomer, query: partyQuery });
          predicates.push(containedIn);
        }

        if (data.reference) {
          const like: string = data.reference.replace("*", "%") + "%";
          predicates.push(new Like({ roleType: m.SalesOrder.CustomerReference, value: like }));
        }

        const query: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
              new TreeNode({ roleType: m.SalesOrder.SalesOrderState }),
            ],
            name: "orders",
            objectType: m.SalesOrder,
            page: new Page({ skip: 0, take }),
            predicate,
            sort: [new Sort({ roleType: m.SalesOrder.OrderNumber, direction: "Desc" })],
          })];

        return this.scope.load("Pull", new PullRequest({ query }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.orders as SalesOrder[];
        this.total = loaded.values.orders_total;
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
    this.titleService.setTitle("Sales Orders");
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public onView(order: SalesOrder): void {
    this.router.navigate(["/orders/salesOrders/" + order.id]);
  }

  private more(): void {
    this.page$.next(this.data.length + 50);
  }
}
