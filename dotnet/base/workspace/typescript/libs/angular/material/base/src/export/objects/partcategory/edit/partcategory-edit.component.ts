import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, RefreshService, Saved, MetaService } from '@allors/angular/services/core';
import { Meta } from '@allors/meta/generated';
import { PartCategory, Locale } from '@allors/domain/generated';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import { FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';
import { TestScope, Action } from '@allors/angular/core';

@Component({
  templateUrl: './partcategory-edit.component.html',
  providers: [ContextService]
})
export class PartCategoryEditComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;
  public title: string;

  public category: PartCategory;
  public locales: Locale[];
  public categories: PartCategory[];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PartCategoryEditComponent>,
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
            pull.PartCategory({
              sort: new Sort(m.PartCategory.Name),
            }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.PartCategory(
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

        this.category = loaded.objects.PartCategory as PartCategory;
        this.categories = loaded.collections.PartCategories as PartCategory[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        if (isCreate) {
          this.title = 'Add Part Category';
          this.category = this.allors.context.create('PartCategory') as PartCategory;
        } else {
          if (this.category.CanWriteName) {
            this.title = 'Edit Part Category';
          } else {
            this.title = 'View Part Category';
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
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
