import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { SalesTerm, TermType } from '../../../../../domain';
import { PullRequest, Sort, Equals, ISessionObject, IObject } from '../../../../../framework';
import { ObjectData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './salesterm-edit.component.html',
  providers: [ContextService]
})
export class SalesTermEditComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit Term Type';

  public container: ISessionObject;
  public object: SalesTerm;
  public termTypes: TermType[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<SalesTermEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([, ]) => {

          const create = (this.data as IObject).id === undefined;
          const { objectType, associationRoleType } = this.data;

          const pulls = [
            pull.SalesTerm(
              {
                object: this.data.id,
                include: {
                  TermType: x,
                }
              }),
            pull.TermType({
              predicate: new Equals({ propertyType: m.TermType.IsActive, value: true }),
              sort: [
                new Sort(m.TermType.Name),
              ],
            })
          ];

          if (create && this.data.associationId) {
            pulls.push(
              pull.SalesInvoice({ object: this.data.associationId }),
              pull.SalesOrder({ object: this.data.associationId }),
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

        this.container = loaded.objects.SalesInvoice || loaded.objects.SalesOrder;
        this.object = loaded.objects.SalesTerm as SalesTerm;
        this.termTypes = loaded.collections.TermTypes as TermType[];
        this.termTypes = this.termTypes.filter(v => v.objectType.name === `${objectType.name}Type`);

        if (create) {
          this.title = 'Add Sales Term';
          this.object = this.allors.context.create(objectType.name) as SalesTerm;
          this.container.add(associationRoleType.name, this.object);
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
      },
        this.saveService.errorHandler
      );
  }
}
