import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Catalogue, CatScope, InternalOrganisation, Locale, ProductCategory, Singleton } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';
import { CreateData, EditData } from 'src/allors/material/base/services/object';

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
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<CatalogueEditComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.m = this.metaService.m;

    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const create = (this.data as EditData).id === undefined;
          const { id, objectType, associationRoleType } = this.data;

          const pulls = [
            this.fetcher.categories,
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.Catalogue({
              object: id,
              include: {
                CatalogueImage: x,
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x
                }
              }
            })
            ,
            pull.CatScope()];

          return this.allors.context.load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create }))
            );
        })
      )
      .subscribe(({ loaded, create }) => {

        this.allors.context.reset();

        this.catalogue = loaded.objects.Catalogue as Catalogue;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.categories = loaded.collections.Categories as ProductCategory[];
        this.catScopes = loaded.collections.CatScopes as CatScope[];
        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;

        if (create) {
          this.title = 'Add Catalogue';
          this.catalogue = this.allors.context.create('Catalogue') as Catalogue;
          this.catalogue.InternalOrganisation = this.internalOrganisation;
        } else {
          this.title = 'Edit Catalogue';
        }

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public imageSelected(id: string): void {
    this.update();
    this.snackBar.open('Catalogue succesfully saved.', 'close', { duration: 5000 });
  }

  public save(): void {

    this.allors.context.save()
      .subscribe((saved: Saved) => {
        this.dialogRef.close();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {

    this.allors.context.save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
