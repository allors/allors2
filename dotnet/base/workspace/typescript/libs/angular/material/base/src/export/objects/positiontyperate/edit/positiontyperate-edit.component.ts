import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved } from '@allors/angular/services/core';
import { TimeFrequency, RateType, PositionType, PositionTypeRate } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { IObject } from '@allors/domain/system';
import { Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './positiontyperate-edit.component.html',
  providers: [ContextService]
})
export class PositionTypeRateEditComponent extends TestScope implements OnInit, OnDestroy {

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
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PositionTypeRateEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            pull.RateType({ sort: new Sort(this.m.RateType.Name) }),
            pull.TimeFrequency({ sort: new Sort(this.m.TimeFrequency.Name) }),
            pull.PositionType({
              sort: new Sort(this.m.PositionType.Title),
              include: {
                PositionTypeRate: x
              }
            }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.PositionTypeRate({
                object: this.data.id,
                include: {
                  RateType: x,
                  Frequency: x
                }
              }),
            );
          }

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.rateTypes = loaded.collections.RateTypes as RateType[];
        this.timeFrequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        const hour = this.timeFrequencies.find((v) => v.UniqueId === 'db14e5d5-5eaf-4ec8-b149-c558a28d99f5');

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
      .subscribe(() => {
        const data: IObject = {
          id: this.positionTypeRate.id,
          objectType: this.positionTypeRate.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
