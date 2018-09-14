import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject,  Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../../angular';
import { InvoiceTermType, SalesInvoice, SalesTerm } from '../../../../../../domain';
import { PullRequest, Sort,Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './invoiceterm.component.html',
})
export class InvoiceTermEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title = 'Edit Sales Order Invoice Term';
  public subTitle: string;
  public invoice: SalesInvoice;
  public salesTerm: SalesTerm;
  public invoiceTermTypes: InvoiceTermType[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
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

    const { m, pull } = this.dataService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const termId: string = this.route.snapshot.paramMap.get('termId');

          const pulls = [
            pull.SalesInvoice({ object: id }),
            pull.SalesTerm({ object: termId, include: { TermType: x } }),
            pull.InvoiceTermType({
              predicate: new Equals({ propertyType: m.InvoiceTermType.IsActive, value: true }),
              sort: new Sort(m.IncoTermType.Name),
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.salesTerm = loaded.objects.salesTerm as SalesTerm;
        this.invoiceTermTypes = loaded.collections.invoiceTermTypes as InvoiceTermType[];

        if (!this.salesTerm) {
          this.title = 'Add Sales Invoice Term';
          this.salesTerm = this.scope.session.create('InvoiceTerm') as SalesTerm;
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
