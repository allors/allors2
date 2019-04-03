import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { PositionTypeRate, TimeFrequency, RateType, PositionType } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { CreateData } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { StateService } from '../../../services/state';
import { SaveService } from 'src/allors/material/base/services/save';

@Component({
  templateUrl: './positiontyperate-edit.component.html',
  providers: [ContextService]
})
export class PositionTypeRateEditComponent implements OnInit, OnDestroy {

  title: string;
  subTitle: string;

  readonly m: Meta;

  positionTypeRate: PositionTypeRate;
  timeFrequencies: TimeFrequency[];
  rateTypes: RateType[];
  selectedPositionTypes: PositionType[];

  private subscription: Subscription;
  positionTypes: PositionType[];
  originalPositionTypes: PositionType[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<PositionTypeRateEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.PositionTypeRate({
              object: this.data.id,
              include: {
                RateType: x,
                Frequency: x
              }
            }),
            pull.RateType({ sort: new Sort(this.m.RateType.Name) }),
            pull.TimeFrequency({ sort: new Sort(this.m.TimeFrequency.Name) }),
            pull.PositionType({
              sort: new Sort(this.m.PositionType.Title),
              include: {
                PositionTypeRate: x
              }
            }),
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

        this.rateTypes = loaded.collections.RateTypes as RateType[];
        this.timeFrequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        const hour = this.timeFrequencies.find((v) => v.UniqueId.toUpperCase() === 'DB14E5D5-5EAF-4EC8-B149-C558A28D99F5');

        if (isCreate) {
          this.title = 'Add Position Type Rate';
          this.positionTypeRate = this.allors.context.create('PositionTypeRate') as PositionTypeRate;
          this.positionTypeRate.Frequency = hour;
        } else {
          this.positionTypeRate = loaded.objects.PositionTypeRate as PositionTypeRate;

          if (this.positionTypeRate.CanWriteRate) {
            this.title = 'Edit Position Type Rate';
          } else {
            this.title = 'View Position Type Rate';
          }
        }

        this.positionTypes = loaded.collections.PositionTypes as PositionType[];
        this.selectedPositionTypes = this.positionTypes.filter(v => v.PositionTypeRate === this.positionTypeRate);
        this.originalPositionTypes = this.selectedPositionTypes;

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

    if (this.selectedPositionTypes !== undefined) {
      this.selectedPositionTypes.forEach((positionType: PositionType) => {

        positionType.PositionTypeRate = this.positionTypeRate;

        const index = this.originalPositionTypes.indexOf(positionType);
        if (index > -1) {
          this.originalPositionTypes.splice(index, 1);
        }
      });
    }

    this.originalPositionTypes.forEach((positionType: PositionType) => {
      positionType.PositionTypeRate = null;
    });

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.positionTypeRate.id,
          objectType: this.positionTypeRate.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
