import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { ErrorService, Loaded, PdfService, Scope, WorkspaceService } from "../../../../../angular";
import { InternalOrganisation, SalesOrder, SalesOrderState } from "../../../../../domain";
import { And, ContainedIn, Equals, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";

interface SearchData {
  internalOrganisation: string;
  company: string;
  reference: string;
  orderNumber: string;
  state: string;
}

@Component({
  templateUrl: "./salesorders-overview.component.html",
})
export class SalesOrdersOverviewComponent implements OnDestroy {

  public searchForm: FormGroup;

  public title: string = "Sales Orders";
  public data: SalesOrder[];
  public filtered: SalesOrder[];
  public total: number;

  public internalOrganisations: InternalOrganisation[];
  public selectedInternalOrganisation: InternalOrganisation;
  public billToInternalOrganisation: InternalOrganisation;

  public orderStates: SalesOrderState[];
  public selectedOrderState: SalesOrderState;
  public orderState: SalesOrderState;

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
    private changeDetectorRef: ChangeDetectorRef,
    public pdfService: PdfService,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      internalOrganisation: [""],
      company: [""],
      orderNumber: [""],
      reference: [""],
      state: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$ = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$ = Observable.combineLatest(search$, this.page$, this.refresh$, this.stateService.internalOrganisationId$)
      .scan(([previousData, previousTake, previousDate, previousInternalOrganisationId], [data, take, date, internalOrganisationId]) => {
        return [
          data,
          data !== previousData ? 50 : take,
          date,
          internalOrganisationId,
        ];
      }, [] as [SearchData, number, Date, InternalOrganisation]);

    const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

    this.subscription = combined$
      .switchMap(([data, take, , internalOrganisationId]) => {

        const internalOrganisationsQuery: Query[] = [
          new Query(
            {
              name: "orderStates",
              objectType: m.SalesOrderState,
            }),
          new Query(
            {
              name: "internalOrganisations",
              objectType: m.Organisation,
              predicate: new Equals({ roleType: m.Organisation.IsInternalOrganisation, value: true }),
            }),
        ];

        return this.scope
        .load("Pull", new PullRequest({ queries: internalOrganisationsQuery }))
        .switchMap((loaded: Loaded) => {
          this.orderStates = loaded.collections.orderStates as SalesOrderState[];
          this.orderState = this.orderStates.find((v: SalesOrderState) => v.Name === data.state);

          this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];
          this.billToInternalOrganisation = this.internalOrganisations.find(
            (v) => v.PartyName === data.internalOrganisation,
          );

          const predicate: And = new And();
          const predicates: Predicate[] = predicate.predicates;

          predicates.push(new Equals({ roleType: m.SalesOrder.TakenBy, value: internalOrganisationId }));

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

          if (data.internalOrganisation) {
            predicates.push(
              new Equals({
                roleType: m.SalesOrder.BillToCustomer,
                value: this.billToInternalOrganisation,
              }),
            );
          }

          if (data.reference) {
            const like: string = data.reference.replace("*", "%") + "%";
            predicates.push(new Like({ roleType: m.SalesOrder.CustomerReference, value: like }));
          }

          if (data.state) {
            predicates.push(new Equals({ roleType: m.SalesOrder.SalesOrderState, value: this.orderState }));
          }

          const queries: Query[] = [new Query(
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

          return this.scope.load("Pull", new PullRequest({ queries }));
          });
      })
      .subscribe((loaded) => {
        this.data = loaded.collections.orders as SalesOrder[];
        this.total = loaded.values.orders_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  public print(order: SalesOrder) {
    this.pdfService.display(order);
  }

  public goBack(): void {
    window.history.back();
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
