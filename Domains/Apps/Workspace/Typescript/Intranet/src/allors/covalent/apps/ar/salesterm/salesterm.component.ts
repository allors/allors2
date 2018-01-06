import {  AfterViewInit,  ChangeDetectorRef,  Component,  OnDestroy,  OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import {
  ErrorService,
  Field,
  Filter,
  Loaded,
  Saved,
  Scope,
  WorkspaceService,
} from "../../../../angular";
import {
  IncoTermType,
  InvoiceTermType,
  OrderTermType,
  SalesInvoice,
  SalesTerm,
} from "../../../../domain";
import {
  Fetch,
  Path,
  PullRequest,
  Query,
  Sort,
  TreeNode
} from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  templateUrl: "./salesterm.component.html"
})
export class SalesTermEditComponent implements OnInit, OnDestroy {
  public m: MetaDomain;

  public title: string = "Edit Sales Invoice Item";
  public subTitle: string;
  public invoice: SalesInvoice;
  public salesTerm: SalesTerm;
  public incoTermTypes: IncoTermType[];
  public invoiceTermTypes: InvoiceTermType[];
  public orderTermTypes: OrderTermType[];
  public salesTermTypes: any[];

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
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef,
  ) {
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<
      [UrlSegment[], Date]
    > = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {
        const id: string = this.route.snapshot.paramMap.get("id");
        const termId: string = this.route.snapshot.paramMap.get("termId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "salesInvoice",
          }),
          new Fetch({
            id: termId,
            include: [new TreeNode({ roleType: m.SalesTerm.TermType })],
            name: "salesTerm",
          }),
        ];

        const query: Query[] = [
          new Query({
            name: "incoTermTypes",
            objectType: m.IncoTermType,
          }),
          new Query({
            name: "invoiceTermTypes",
            objectType: m.InvoiceTermType,
          }),
          new Query({
            name: "orderTermTypes",
            objectType: m.OrderTermType,
          }),
        ];

        return this.scope.load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe(
        (loaded: Loaded) => {
          this.invoice = loaded.objects.salesInvoice as SalesInvoice;
          this.salesTerm = loaded.objects.salesTerm as SalesTerm;
          this.incoTermTypes = loaded.collections.incoTermTypes as IncoTermType[];
          this.invoiceTermTypes = loaded.collections.invoiceTermTypes as InvoiceTermType[];
          this.orderTermTypes = loaded.collections.orderTermTypes as OrderTermType[];
          this.salesTermTypes = this.incoTermTypes.concat(this.invoiceTermTypes).concat(this.orderTermTypes);

          if (!this.salesTerm) {
            this.title = "Add Invoice Term";
            this.salesTerm = this.scope.session.create(
              "SalesTerm",
            ) as SalesTerm;
            this.invoice.AddSalesTerm(this.salesTerm);
          }
        },
        (error: Error) => {
          this.errorService.message(error);
          this.goBack();
        }
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.scope.save().subscribe(
      (saved: Saved) => {
        this.router.navigate(["/ar/invoice/" + this.invoice.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      }
    );
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
