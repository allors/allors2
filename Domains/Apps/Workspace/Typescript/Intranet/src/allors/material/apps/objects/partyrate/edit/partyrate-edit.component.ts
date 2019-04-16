import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { PartyRate, TimeFrequency, RateType, Party } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { CreateData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  templateUrl: './partyrate-edit.component.html',
  providers: [ContextService]
})
export class PartyRateEditComponent implements OnInit, OnDestroy {

  title: string;
  subTitle: string;

  readonly m: Meta;

  partyRate: PartyRate;
  timeFrequencies: TimeFrequency[];
  rateTypes: RateType[];

  private subscription: Subscription;
  party: Party;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<PartyRateEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.PartyRate({
              object: this.data.id,
              include: {
                RateType: x,
                Frequency: x
              }
            }),
            pull.Party({
              object: this.data.associationId,
              include: {
                PartyRates: x,
              }
            }),
            pull.RateType({ sort: new Sort(this.m.RateType.Name) }),
            pull.TimeFrequency({ sort: new Sort(this.m.TimeFrequency.Name) }),
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

        this.party = loaded.objects.Party as Party;
        this.rateTypes = loaded.collections.RateTypes as RateType[];
        this.timeFrequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        const hour = this.timeFrequencies.find((v) => v.UniqueId.toUpperCase() === 'DB14E5D5-5EAF-4EC8-B149-C558A28D99F5');

        if (isCreate) {
          this.title = 'Add Party Rate';
          this.partyRate = this.allors.context.create('PartyRate') as PartyRate;
          this.partyRate.Frequency = hour;
          this.party.AddPartyRate(this.partyRate);
        } else {
          this.partyRate = loaded.objects.PartyRate as PartyRate;

          if (this.partyRate.CanWriteRate) {
            this.title = 'Edit Party Rate';
          } else {
            this.title = 'View Party Rate';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public setDirty(): void {
    this.allors.context.session.hasChanges = true;
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.partyRate.id,
          objectType: this.partyRate.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
