import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { OrderTermType, SalesInvoice, SalesTerm } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './orderterm-edit.component.html',
  providers: [ContextService]
})
export class OrderTermEditComponent implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit Sales Invoice Incoterm';
  public subTitle: string;
  public invoice: SalesInvoice;
  public salesTerm: SalesTerm;
  public orderTermTypes: OrderTermType[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    public refreshService: RefreshService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refreshService.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const termId: string = this.route.snapshot.paramMap.get('termId');

          const pulls = [
            pull.SalesInvoice({ object: id }),
            pull.SalesTerm(
              {
                object: termId,
                include: { TermType: x }
              }
            ),
            pull.OrderTermType(
              {
                predicate: new Equals({ propertyType: m.OrderTermType.IsActive, value: true }),
                sort: [
                  new Sort(m.OrderTermType.Name),
                ],
              }
            )
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
        this.salesTerm = loaded.objects.SalesTerm as SalesTerm;
        this.orderTermTypes = loaded.collections.OrderTermTypes as OrderTermType[];

        if (!this.salesTerm) {
          this.title = 'Add Sales Invoice Order Term';
          this.salesTerm = this.allors.context.create('OrderTerm') as SalesTerm;
          this.invoice.AddSalesTerm(this.salesTerm);
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
        this.router.navigate(['/accountsreceivable/invoice/' + this.invoice.id]);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
