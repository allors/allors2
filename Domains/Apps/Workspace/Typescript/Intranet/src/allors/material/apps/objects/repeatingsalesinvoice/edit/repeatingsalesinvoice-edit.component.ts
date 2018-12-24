import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService } from '../../../../../angular';
import { DayOfWeek, RepeatingSalesInvoice, SalesInvoice, TimeFrequency } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './repeatingsalesinvoice-edit.component.html',
  providers: [ContextService]

})
export class RepeatingSalesInvoiceEditComponent implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit Repeating Invoice';
  public subTitle: string;
  public invoice: SalesInvoice;
  public repeatinginvoice: RepeatingSalesInvoice;
  public frequencies: TimeFrequency[];
  public daysOfWeek: DayOfWeek[];

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

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const repeatingInvoiceId: string = this.route.snapshot.paramMap.get('repeatingInvoiceId');
          const m: Meta = this.m;

          const pulls = [

            pull.SalesInvoice({ object: id }),
            pull.RepeatingSalesInvoice({
              object: id,
              include: {
                Frequency: x,
                DayOfWeek: x,
              }
            }),
            pull.TimeFrequency({
              predicate: new Equals({ propertyType: m.TimeFrequency.IsActive, value: true }),
              sort: new Sort(m.TimeFrequency.Name),
            }),
            pull.DayOfWeek()
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.repeatinginvoice = loaded.objects.repeatingInvoice as RepeatingSalesInvoice;
        this.frequencies = loaded.collections.frequencies as TimeFrequency[];
        this.daysOfWeek = loaded.collections.daysOfWeek as DayOfWeek[];

        if (!this.repeatinginvoice) {
          this.title = 'Add Repeating Invoice';
          this.repeatinginvoice = this.allors.context.create('RepeatingSalesInvoice') as RepeatingSalesInvoice;
          this.repeatinginvoice.Source = this.invoice;
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
