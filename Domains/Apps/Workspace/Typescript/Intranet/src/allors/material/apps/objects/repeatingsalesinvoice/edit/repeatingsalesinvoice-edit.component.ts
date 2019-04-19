import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '../../../../../angular';
import { RepeatingSalesInvoice, TimeFrequency, DayOfWeek, SalesInvoice } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { ObjectData } from '../../../../../material/base/services/object';
import { Meta } from '../../../../../meta';
import { SaveService } from 'src/allors/material';

@Component({
  templateUrl: './repeatingsalesinvoice-edit.component.html',
  providers: [ContextService]

})
export class RepeatingSalesInvoiceEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  repeatinginvoice: RepeatingSalesInvoice;
  frequencies: TimeFrequency[];
  daysOfWeek: DayOfWeek[];
  invoice: SalesInvoice;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<RepeatingSalesInvoiceEditComponent>,
    public metaService: MetaService,
    private saveService: SaveService,
    public refreshService: RefreshService,
  ) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = this.data.id === undefined;
          const id = this.data.id;

          const pulls = [
            pull.SalesInvoice({ object: this.data.associationId }),
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
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
        this.repeatinginvoice = loaded.objects.RepeatingSalesInvoice as RepeatingSalesInvoice;
        this.frequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        this.daysOfWeek = loaded.collections.DaysOfWeek as DayOfWeek[];

        if (isCreate) {
          this.title = 'Create Repeating Invoice';
          this.repeatinginvoice = this.allors.context.create('RepeatingSalesInvoice') as RepeatingSalesInvoice;
          this.repeatinginvoice.Source = this.invoice;
        } else {

          if (this.repeatinginvoice.CanWriteFrequency) {
            this.title = 'Edit Repeating Invoice';
          } else {
            this.title = 'View Repeating Invoice';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.repeatinginvoice.id,
          objectType: this.repeatinginvoice.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
