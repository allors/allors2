import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService } from '../../../../../angular';
import { OrderTermType, SalesOrder, SalesTerm } from '../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './orderterm.component.html',
  providers: [ContextService]
})
export class OrderTermEditComponent implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit Sales Order Incoterm';
  public subTitle: string;
  public order: SalesOrder;
  public salesTerm: SalesTerm;
  public orderTermTypes: OrderTermType[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const termId: string = this.route.snapshot.paramMap.get('termId');

          const pulls = [
            pull.SalesOrder({ object: id }),
            pull.SalesTerm({
              object: id,
              include: { TermType: x }
            }),
            pull.OrderTermType({
              predicate: new Equals({ propertyType: m.OrderTermType.IsActive, value: true }),
              sort: new Sort(m.OrderTermType.Name),
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.order = loaded.objects.salesOrder as SalesOrder;
        this.salesTerm = loaded.objects.salesTerm as SalesTerm;
        this.orderTermTypes = loaded.collections.orderTermTypes as OrderTermType[];

        if (!this.salesTerm) {
          this.title = 'Add Sales Order Order Term';
          this.salesTerm = this.allors.context.create('OrderTerm') as SalesTerm;
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

    this.allors.context
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
