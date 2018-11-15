import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { ProductCategory } from '../../../../../domain';
import { And, Equals, Like, Predicate, PullRequest, Sort } from '../../../../../framework';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, switchMap, scan } from 'rxjs/operators';

interface SearchData {
  name?: string;
}

@Component({
  templateUrl: './categories-overview.component.html',
  providers: [Allors]
})
export class CategoriesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Categories';
  public productCategories: ProductCategory[];
  public filtered: ProductCategory[];

  public search$: BehaviorSubject<SearchData>;
  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle('Categories');

    this.search$ = new BehaviorSubject<SearchData>({});
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const search$ = this.search$
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
      );

    this.subscription = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([data, refresh, internalOrganisationId]) => {
          const predicate: And = new And();
          const predicates: Predicate[] = predicate.operands;

          predicates.push(new Equals({ propertyType: m.ProductCategory.InternalOrganisation, object: internalOrganisationId }));

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.ProductCategory.Name, value: like }));
          }

          const pulls = [
            pull.ProductCategory(
              {
                predicate,
                include: {
                  CategoryImage: x,
                  LocalisedNames: x,
                  LocalisedDescriptions: x,
                },
                sort: new Sort(m.ProductCategory.Name),
              }
            )];

          return scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.productCategories = loaded.collections.ProductCategories as ProductCategory[];
      }, this.errorService.handler);
  }

  public delete(category: ProductCategory): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this category?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(category.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public onView(category: ProductCategory): void {
    this.router.navigate(['/category/' + category.id]);
  }
}
