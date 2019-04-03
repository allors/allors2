import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import {  ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Catalogue, CatScope, InternalOrganisation, Locale, ProductCategory, Singleton, Organisation } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { switchMap, map } from 'rxjs/operators';
import { CreateData } from '../../../../../material/base/services/object';
import { SaveService } from 'src/allors/material/base/services/save';

@Component({
  templateUrl: './catalogue-edit.component.html',
  providers: [ContextService]
})
export class CatalogueEditComponent implements OnInit, OnDestroy {

  public m: Meta;

  public catalogue: Catalogue;
  public title: string;

  public subTitle: string;

  public singleton: Singleton;
  public locales: Locale[];
  public categories: ProductCategory[];
  public catScopes: CatScope[];
  public internalOrganisation: InternalOrganisation;

  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<CatalogueEditComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private saveService: SaveService,
    private stateService: StateService) {

    this.m = this.metaService.m;

    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            this.fetcher.categories,
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.Catalogue({
              object: this.data.id,
              include: {
                CatalogueImage: x,
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x
                }
              }
            }),
            pull.CatScope()
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }))
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
        this.catScopes = loaded.collections.CatScopes as CatScope[];
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
      },
      this.saveService.errorHandler
    );
  }
}
