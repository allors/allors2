import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Field, SearchFactory, Loaded, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { DayOfWeek, IncoTermType, RepeatingSalesInvoice, SalesInvoice, SalesTerm, TimeFrequency } from '../../../../../domain';
import { PullRequest, Fetch, Sort, TreeNode, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './repeatinginvoice.component.html',
  providers: [Allors]

})
export class RepeatingInvoiceEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title = 'Edit Repeating Invoice';
  public subTitle: string;
  public invoice: SalesInvoice;
  public repeatinginvoice: RepeatingSalesInvoice;
  public frequencies: TimeFrequency[];
  public daysOfWeek: DayOfWeek[];

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

    const { pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const repeatingInvoiceId: string = this.route.snapshot.paramMap.get('repeatingInvoiceId');
          const m: MetaDomain = this.m;

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

          return scope
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
          this.repeatinginvoice = scope.session.create('RepeatingSalesInvoice') as RepeatingSalesInvoice;
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
    const { scope } = this.allors;

    scope
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
