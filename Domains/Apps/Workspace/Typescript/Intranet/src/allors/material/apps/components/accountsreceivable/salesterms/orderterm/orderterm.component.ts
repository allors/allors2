import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';


import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Field, FilterFactory, Loaded, Saved, Scope, WorkspaceService } from '../../../../../../angular';
import { OrderTermType, SalesInvoice, SalesTerm } from '../../../../../../domain';
import { Fetch, Path, PullRequest, Query, Sort, TreeNode, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';

@Component({
  templateUrl: './orderterm.component.html',
})
export class OrderTermEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title = 'Edit Sales Invoice Incoterm';
  public subTitle: string;
  public invoice: SalesInvoice;
  public salesTerm: SalesTerm;
  public orderTermTypes: OrderTermType[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const termId: string = this.route.snapshot.paramMap.get('termId');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            name: 'salesInvoice',
          }),
          new Fetch({
            id: termId,
            include: [
              new TreeNode({ roleType: m.SalesTerm.TermType }),
            ],
            name: 'salesTerm',
          }),
        ];

        const queries: Query[] = [
          new Query({
            name: 'orderTermTypes',
            objectType: m.OrderTermType,
            predicate: new Equals({ roleType: m.OrderTermType.IsActive, value: true }),
            sort: [
              new Sort({ roleType: m.OrderTermType.Name, direction: 'Asc' }),
            ],
          }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.salesTerm = loaded.objects.salesTerm as SalesTerm;
        this.orderTermTypes = loaded.collections.orderTermTypes as OrderTermType[];

        if (!this.salesTerm) {
          this.title = 'Add Sales Invoice Order Term';
          this.salesTerm = this.scope.session.create('OrderTerm') as SalesTerm;
          this.invoice.AddSalesTerm(this.salesTerm);
        }
      },
      (error: Error) => {
        this.errorService.handle(error);
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
        this.router.navigate(['/accountsreceivable/invoice/' + this.invoice.id]);
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
