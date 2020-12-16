import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved } from '@allors/angular/services/core';
import { OrderAdjustment } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { IObject, ISessionObject } from '@allors/domain/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './orderadjustment-edit.component.html',
  providers: [ContextService]
})
export class OrderAdjustmentEditComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;

  public title: string;

  public container: ISessionObject;
  public object: OrderAdjustment;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<OrderAdjustmentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$])
      .pipe(
        switchMap(() => {

          const isCreate = (this.data as IObject).id === undefined;
          const { objectType, associationRoleType } = this.data;

          const pulls = [
          ];

          if (!isCreate) {
            pulls.push(
              pull.OrderAdjustment(
                {
                  object: this.data.id,
                }),
              );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Quote({ object: this.data.associationId }),
              pull.Order({ object: this.data.associationId }),
              pull.Invoice({ object: this.data.associationId }),
            );
          }

          return this.allors.context.load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create: isCreate, objectType, associationRoleType }))
            );
        })
      )
      .subscribe(({ loaded, create, objectType, associationRoleType }) => {
        this.allors.context.reset();

        this.container = loaded.objects.Quote || loaded.objects.Order || loaded.objects.Invoice;
        this.object = loaded.objects.OrderAdjustment as OrderAdjustment;

        if (create) {
          this.title = `Add ${ objectType.name }`;
          this.object = this.allors.context.create(objectType.name) as OrderAdjustment;
          this.container.add(associationRoleType, this.object);
        } else {
          this.title = `Edit ${ objectType.name }`;
        }

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.object.id,
          objectType: this.object.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
