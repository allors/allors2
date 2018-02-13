import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Field, Filter, Loaded, Saved, Scope, WorkspaceService } from "../../../../../../angular";
import { IncoTermType, SalesInvoice, SalesTerm } from "../../../../../../domain";
import { Fetch, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../../framework";
import { MetaDomain } from "../../../../../../meta";

@Component({
  templateUrl: "./incoterm.component.html",
})
export class IncoTermEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Sales Invoice Incoterm";
  public subTitle: string;
  public invoice: SalesInvoice;
  public salesTerm: SalesTerm;
  public incoTermTypes: IncoTermType[];

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
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

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
            include: [
              new TreeNode({ roleType: m.SalesTerm.TermType }),
            ],
            name: "salesTerm",
          }),
        ];

        const query: Query[] = [
          new Query({
            name: "incoTermTypes",
            objectType: m.IncoTermType,
          }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded) => {

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.salesTerm = loaded.objects.salesTerm as SalesTerm;
        this.incoTermTypes = loaded.collections.incoTermTypes as IncoTermType[];

        if (!this.salesTerm) {
          this.title = "Add Invoice Incoterm";
          this.salesTerm = this.scope.session.create("IncoTerm") as SalesTerm;
          this.invoice.AddSalesTerm(this.salesTerm);
        }
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/accountsreceivable/invoice/" + this.invoice.id]);
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
