import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved } from '@allors/angular/services/core';
import { ProductIdentification, ProductIdentificationType } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './productidentification-edit.component.html',
  providers: [ContextService]
})
export class ProductIdentificationEditComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit IGood Identification';

  public container: ISessionObject;
  public object: ProductIdentification;
  public productIdentificationTypes: ProductIdentificationType[];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<ProductIdentificationEditComponent>,
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
        switchMap(() => {

          const isCreate = (this.data as IObject).id === undefined;
          const { objectType, associationRoleType } = this.data;

          const pulls = [
            pull.ProductIdentificationType({
              predicate: new Equals({ propertyType: m.ProductIdentificationType.IsActive, value: true }),
              sort: [
                new Sort(m.ProductIdentificationType.Name),
              ],
            })
          ];

          if (!isCreate) {
            pulls.push(
              pull.ProductIdentification(
                {
                  object: this.data.id,
                  include: {
                    ProductIdentificationType: x,
                  }
                }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Good({ object: this.data.associationId }),
              pull.Part({ object: this.data.associationId }),
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

        this.container = loaded.objects.Good || loaded.objects.Part;
        this.object = loaded.objects.ProductIdentification as ProductIdentification;
        this.productIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];

        if (create) {
          this.title = 'Add Identification';
          this.object = this.allors.context.create(objectType) as ProductIdentification;
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
