import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { PositionType } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { CreateData } from '../../../../../material/base/services/object';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './positiontype-edit.component.html',
  providers: [ContextService]
})
export class PositionTypeEditComponent implements OnInit, OnDestroy {

  public title: string;
  public subTitle: string;

  public m: Meta;

  public positionType: PositionType;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<PositionTypeEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
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
            pull.PositionType({
              object: this.data.id,
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
        const data: IObject = {
          id: this.positionType.id,
          objectType: this.positionType.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
