import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved } from '@allors/angular/services/core';
import { Organisation, InternalOrganisation, ProductCategory, Scope, Locale } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId, FetcherService } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './productcategory-edit.component.html',
  providers: [ContextService]
})
export class ProductCategoryEditComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;
  public title: string;

  public category: ProductCategory;
  public locales: Locale[];
  public categories: ProductCategory[];
  public scopes: Scope[];
  public internalOrganisation: InternalOrganisation;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<ProductCategoryEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.Scope(),
            pull.ProductCategory({
              sort: new Sort(m.ProductCategory.Name),
            }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.ProductCategory(
                {
                  object: this.data.id,
                  include: {
                    Children: x,
                    LocalisedNames: {
                      Locale: x,
                    },
                    LocalisedDescriptions: {
                      Locale: x,
                    }
                  }
                }
              ),
            );
          }

          return this.allors.context.load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.category = loaded.objects.ProductCategory as ProductCategory;
        this.categories = loaded.collections.ProductCategories as ProductCategory[];
        this.scopes = loaded.collections.Scopes as Scope[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        if (isCreate) {
          this.title = 'Add Category';
          this.category = this.allors.context.create('ProductCategory') as ProductCategory;
          this.category.InternalOrganisation = this.internalOrganisation;
        } else {
          if (this.category.CanWriteCatScope) {
            this.title = 'Edit Category';
          } else {
            this.title = 'View Category';
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
          id: this.category.id,
          objectType: this.category.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
