import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService, FetcherService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { CatScope, InternalOrganisation, Locale, ProductCategory, Organisation } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { ObjectData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

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
  public catScopes: CatScope[];
  public internalOrganisation: InternalOrganisation;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
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
        switchMap(([, ]) => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
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
            pull.CatScope(),
            pull.ProductCategory({
              sort: new Sort(m.ProductCategory.Name),
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }))
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
        this.catScopes = loaded.collections.CatScopes as CatScope[];
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
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.category.id,
          objectType: this.category.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
