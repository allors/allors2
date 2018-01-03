import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Field, Filter, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { SalesOrder, SalesTerm, SalesTermType } from "../../../../domain";
import { Fetch, Path, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  templateUrl: "./salesterm.component.html",
})
export class SalesTermEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Sales Order Item";
  public subTitle: string;
  public order: SalesOrder;
  public salesTerm: SalesTerm;
  public salesTermTypes: SalesTermType[];

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
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const termId: string = this.route.snapshot.paramMap.get("termId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "salesOrder",
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
          new Query(
            {
              name: "salesTermTypes",
              objectType: m.SalesTermType,
              sort: [new Sort({ roleType: m.SalesTermType.Name, direction: "Asc" })],
            }),
          ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.order = loaded.objects.salesOrder as SalesOrder;
        this.salesTerm = loaded.objects.salesTerm as SalesTerm;
        this.salesTermTypes = loaded.collections.salesTermTypes as SalesTermType[];

        if (!this.salesTerm) {
          this.title = "Add Order Term";
          this.salesTerm = this.scope.session.create("SalesTerm") as SalesTerm;
          this.order.AddSalesTerm(this.salesTerm);
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
        this.router.navigate(["/orders/salesOrder/" + this.order.id]);
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
