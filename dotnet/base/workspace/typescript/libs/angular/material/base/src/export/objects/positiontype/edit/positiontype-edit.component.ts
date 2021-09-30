import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved } from '@allors/angular/services/core';
import { PositionType } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { IObject } from '@allors/domain/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './positiontype-edit.component.html',
  providers: [ContextService]
})
export class PositionTypeEditComponent extends TestScope implements OnInit, OnDestroy {

  public title: string;
  public subTitle: string;

  public m: Meta;

  public positionType: PositionType;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PositionTypeEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
          ];

          if (!isCreate) {
            pulls.push(
              pull.PositionType({
                object: this.data.id,
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

        if (isCreate) {
          this.title = 'Add Position Type';
          this.positionType = this.allors.context.create('PositionType') as PositionType;
        } else {
          this.positionType = loaded.objects.PositionType as PositionType;

          if (this.positionType.CanWriteTitle) {
            this.title = 'Edit Position Type';
          } else {
            this.title = 'View Position Type';
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
          id: this.positionType.id,
          objectType: this.positionType.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
