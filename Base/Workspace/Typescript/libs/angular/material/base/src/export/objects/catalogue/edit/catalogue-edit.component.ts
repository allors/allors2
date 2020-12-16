import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { Organisation, InternalOrganisation, Catalogue, Singleton, ProductCategory, Scope, Locale } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import { FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './catalogue-edit.component.html',
  providers: [ContextService]
})
export class CatalogueEditComponent extends TestScope implements OnInit, OnDestroy {

  public m: Meta;

  public catalogue: Catalogue;
  public title: string;

  public subTitle: string;

  public singleton: Singleton;
  public locales: Locale[];
  public categories: ProductCategory[];
  public scopes: Scope[];
  public internalOrganisation: InternalOrganisation;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<CatalogueEditComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
    private fetcher: FetcherService) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.categories,
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.Scope()
          ];

          if (!isCreate) {
            pulls.push(
              pull.Catalogue({
                object: this.data.id,
                include: {
                  CatalogueImage: x,
                  LocalisedNames: {
                    Locale: x,
                  },
                  LocalisedDescriptions: {
                    Locale: x,
                  },
                },
              }),
            );
          }

          return this.allors.context.load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create: isCreate }))
            );
        })
      )
      .subscribe(({ loaded, create }) => {

        this.allors.context.reset();

        this.catalogue = loaded.objects.Catalogue as Catalogue;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.categories = loaded.collections.ProductCategories as ProductCategory[];
        this.scopes = loaded.collections.Scopes as Scope[];
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        if (create) {
          this.title = 'Add Catalogue';
          this.catalogue = this.allors.context.create('Catalogue') as Catalogue;
          this.catalogue.InternalOrganisation = this.internalOrganisation;
        } else {
          if (this.catalogue.CanWriteCatScope) {
            this.title = 'Edit Catalogue';
          } else {
            this.title = 'View Catalogue';
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

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.catalogue.id,
          objectType: this.catalogue.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
