import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { ErrorService, Loaded, Scope, WorkspaceService } from "../../../../../angular";
import { InternalOrganisation, ProductQuote } from "../../../../../domain";
import { And, ContainedIn, Equals, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";

interface SearchData {
  company: string;
  description: string;
  quoteNumber: string;
}

@Component({
  templateUrl: "./productquotes-overview.component.html",
})
export class ProductQuotesOverviewComponent implements OnDestroy {

  public searchForm: FormGroup;

  public title: string = "Quotes";
  public data: ProductQuote[];
  public filtered: ProductQuote[];
  public total: number;

  private subscription: Subscription;
  private scope: Scope;

  private refresh$: BehaviorSubject<Date>;
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
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      company: [""],
      description: [""],
      quoteNumber: [""],
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

    this.subscription = combined$
      .switchMap(([data, take, , internalOrganisationId]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        predicates.push(new Equals({ roleType: m.ProductQuote.Issuer, value: internalOrganisationId }));

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

        const queries: Query[] = [new Query(
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

        return this.scope.load("Pull", new PullRequest({ queries }));

      })
      .subscribe((loaded) => {
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
