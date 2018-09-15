import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { ProductCategory } from '../../../../../domain';
import { And, Equals, Like, Predicate, PullRequest, Sort } from '../../../../../framework';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, switchMap, scan } from 'rxjs/operators';

interface SearchData {
  name: string;
}

@Component({
  templateUrl: './categories-overview.component.html',
})
export class CategoriesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Categories';
  public total: number;
  public searchForm: FormGroup;
  public advancedSearch: boolean;
  public data: ProductCategory[];
  public filtered: ProductCategory[];

  private refresh$: BehaviorSubject<Date>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle('Categories');

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
    });

  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = Observable.combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
          return [data, date, internalOrganisationId];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data, , internalOrganisationId]) => {
          const predicate: And = new And();
          const operands: Predicate[] = predicate.operands;

          operands.push(new Equals({ propertyType: m.ProductCategory.InternalOrganisation, value: internalOrganisationId }));

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            operands.push(new Like({ roleType: m.ProductCategory.Name, value: like }));
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

          return this.scope.load('Pull', new PullRequest({ pulls }));

        })
      )
      .subscribe((loaded) => {
        this.data = loaded.collections.categories as ProductCategory[];
        this.total = loaded.values.categories_total;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
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

  public delete(category: ProductCategory): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this category?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(category.Delete)
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

  public onView(category: ProductCategory): void {
    this.router.navigate(['/category/' + category.id]);
  }
}
