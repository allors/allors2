import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, TestScope, FetcherService } from '../../../../../angular';
import { RepeatingPurchaseInvoice, TimeFrequency, DayOfWeek, Organisation } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { ObjectData } from '../../../../../material/base/services/object';
import { Meta } from '../../../../../meta';
import { SaveService } from 'src/allors/material';
import { InternalOrganisationId } from '../../../../../angular/apps/state';

@Component({
  templateUrl: './repeatingpurchaseinvoice-edit.component.html',
  providers: [ContextService]

})
export class RepeatingPurchaseInvoiceEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  repeatinginvoice: RepeatingPurchaseInvoice;
  frequencies: TimeFrequency[];
  daysOfWeek: DayOfWeek[];
  supplier: Organisation;
  internalOrganisation: Organisation;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<RepeatingPurchaseInvoiceEditComponent>,
    public metaService: MetaService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
    private fetcher: FetcherService,
    public refreshService: RefreshService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([]) => {

          const isCreate = this.data.id === undefined;
          const id = this.data.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Organisation({ object: this.data.associationId }),
            pull.RepeatingPurchaseInvoice({
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

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.supplier = loaded.objects.Organisation as Organisation;
        this.repeatinginvoice = loaded.objects.RepeatingPurchaseInvoice as RepeatingPurchaseInvoice;
        this.frequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        this.daysOfWeek = loaded.collections.DaysOfWeek as DayOfWeek[];

        if (isCreate) {
          this.title = 'Create Repeating Invoice';
          this.repeatinginvoice = this.allors.context.create('RepeatingPurchaseInvoice') as RepeatingPurchaseInvoice;
          this.repeatinginvoice.Supplier = this.supplier;
          this.repeatinginvoice.InternalOrganisation = this.internalOrganisation;
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
