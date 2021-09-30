import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { TimeFrequency, SalesInvoice, DayOfWeek, RepeatingSalesInvoice } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { IObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './repeatingsalesinvoice-edit.component.html',
  providers: [ContextService],
})
export class RepeatingSalesInvoiceEditComponent extends TestScope implements OnInit, OnDestroy {
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
    public refreshService: RefreshService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {
          const isCreate = this.data.id === undefined;
          const id = this.data.id;

          const pulls = [
            pull.TimeFrequency({
              predicate: new Equals({ propertyType: m.TimeFrequency.IsActive, value: true }),
              sort: new Sort(m.TimeFrequency.Name),
            }),
            pull.DayOfWeek(),
          ];

          if (!isCreate) {
            pulls.push(
              pull.RepeatingSalesInvoice({
                object: id,
                include: {
                  Frequency: x,
                  DayOfWeek: x,
                },
              }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.SalesInvoice({ object: this.data.associationId }),
            );
          }

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => ({ loaded, isCreate })));
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
    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.repeatinginvoice.id,
        objectType: this.repeatinginvoice.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }
}
