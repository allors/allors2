import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { IGoodIdentification, GoodIdentificationType } from '../../../../../domain';
import { PullRequest, Sort, Equals, ISessionObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

import { CreateData, EditData, ObjectData } from '../../../../../material/base/services/object';

@Component({
  templateUrl: './igoodidentification-edit.component.html',
  providers: [ContextService]
})
export class IGoodIdentificationEditComponent implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit IGood Identification';

  public container: ISessionObject;
  public object: IGoodIdentification;
  public goodIdentificationTypes: GoodIdentificationType[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<IGoodIdentificationEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const create = (this.data as EditData).id === undefined;
          const { objectType, associationRoleType } = this.data;

          const pulls = [
            pull.IGoodIdentification(
              {
                object: this.data.id,
                include: {
                  GoodIdentificationType: x,
                }
              }),
            pull.GoodIdentificationType({
              predicate: new Equals({ propertyType: m.GoodIdentificationType.IsActive, value: true }),
              sort: [
                new Sort(m.GoodIdentificationType.Name),
              ],
            })
          ];

          if (create && this.data.associationId) {
            pulls.push(
              pull.Good({ object: this.data.associationId }),
              pull.Part({ object: this.data.associationId }),
            );
          }

          return this.allors.context.load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create, objectType, associationRoleType }))
            );
        })
      )
      .subscribe(({ loaded, create, objectType, associationRoleType }) => {
        this.allors.context.reset();

        this.container = loaded.objects.Good || loaded.objects.Part;
        this.object = loaded.objects.IGoodIdentification as IGoodIdentification;
        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];

        if (create) {
          this.title = 'Add Identification';
          this.object = this.allors.context.create(objectType) as IGoodIdentification;
          this.container.add(associationRoleType.name, this.object);
        }

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe((saved: Saved) => {
        const data: ObjectData = {
          id: this.object.id,
          objectType: this.object.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
