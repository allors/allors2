import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';

import { ErrorService, Loaded, Saved, SessionService } from '../../../../../angular';
import { Catalogue, CatScope, InternalOrganisation, Locale, ProductCategory, Singleton } from '../../../../../domain';
import { Equals, Fetch, PullRequest, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './catalogue-edit.component.html',
  providers: [SessionService]
})
export class CatalogueComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public catalogue: Catalogue;
  public title: string;

  public subTitle: string;

  public singleton: Singleton;
  public locales: Locale[];
  public categories: ProductCategory[];
  public catScopes: CatScope[];
  public internalOrganisation: InternalOrganisation;

  private subscription: Subscription;

  private refresh$: BehaviorSubject<Date>;

  private fetcher: Fetcher;

  constructor(
    @Self() private allors: SessionService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.m = this.allors.m;

    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

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

          return this.allors.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.catalogue = loaded.objects.catalogue as Catalogue;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.categories = loaded.collections.categories as ProductCategory[];
        this.catScopes = loaded.collections.CatScopes as CatScope[];
        this.internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (!this.catalogue) {
          this.catalogue = this.allors.session.create('Catalogue') as Catalogue;
          this.catalogue.InternalOrganisation = this.internalOrganisation;
        }

        this.title = this.catalogue.Name;
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

    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {

    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
