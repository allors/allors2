import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../../angular';
import { InvoiceTermType, SalesOrder, SalesTerm } from '../../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './invoiceterm.component.html',
  providers: [Allors]
})
export class InvoiceTermEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title = 'Edit Sales Order Invoice Term';
  public subTitle: string;
  public order: SalesOrder;
  public salesTerm: SalesTerm;
  public invoiceTermTypes: InvoiceTermType[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const termId: string = this.route.snapshot.paramMap.get('termId');

          const pulls = [
            pull.SalesOrder({ object: id }),
            pull.SalesTerm({
              object: termId,
              include: {
                TermType: x,
              }
            }),
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
              sort: [
                new Sort(m.InvoiceItemType.Name),
              ],
            })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.order = loaded.objects.salesOrder as SalesOrder;
        this.salesTerm = loaded.objects.salesTerm as SalesTerm;
        this.invoiceTermTypes = loaded.collections.invoiceTermTypes as InvoiceTermType[];

        if (!this.salesTerm) {
          this.title = 'Add Order Invoice Term';
          this.salesTerm = scope.session.create('InvoiceTerm') as SalesTerm;
          this.order.AddSalesTerm(this.salesTerm);
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    const { scope } = this.allors;

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(['/orders/salesOrder/' + this.order.id]);
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
