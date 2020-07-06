import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { OrderAdjustment } from '../../../../../domain';
import { PullRequest, Sort, Equals, ISessionObject, IObject } from '../../../../../framework';
import { ObjectData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './orderadjustment-edit.component.html',
  providers: [ContextService]
})
export class OrderAdjustmentEditComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit Order/Invoice Adjustment';

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

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$])
      .pipe(
        switchMap(() => {

          const create = (this.data as IObject).id === undefined;
          const { objectType, associationRoleType } = this.data;

          const pulls = [
            pull.OrderAdjustment(
              {
                object: this.data.id,
              }),
          ];

          if (create && this.data.associationId) {
            pulls.push(
              pull.Quote({ object: this.data.associationId }),
              pull.Order({ object: this.data.associationId }),
              pull.Invoice({ object: this.data.associationId }),
            );
          }

          return this.allors.context.load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create, objectType, associationRoleType }))
            );
        })
      )
      .subscribe(({ loaded, create, objectType, associationRoleType }) => {
        this.allors.context.reset();

        this.container = loaded.objects.Quote || loaded.objects.Order || loaded.objects.Invoice;
        this.object = loaded.objects.OrderAdjustment as OrderAdjustment;

        if (create) {
          this.title = 'Add Order/Invoice Adjustment';
          this.object = this.allors.context.create(objectType.name) as OrderAdjustment;
          this.container.add(associationRoleType, this.object);
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
      .subscribe((saved: Saved) => {
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
