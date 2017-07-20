import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Predicate, And, Or, Not, Equals, Like, Contains, ContainedIn, TreeNode, Sort, Page } from '../../../../domain';
import { Catalogue } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Invoked } from '../../../../angular';

interface SearchData {
  name: string;
}

@Component({
  templateUrl: './catalogues.component.html',
})
export class CataloguesComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private page$: BehaviorSubject<number>;

  private subscription: Subscription;
  private scope: Scope;

  title: string = 'Catalogues';
  total: number;
  searchForm: FormGroup;
  data: Catalogue[];
  filtered: Catalogue[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MdSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        if (data.name) {
          const like: string = data.name.replace('*', '%') + '%';
          predicates.push(new Like({ roleType: m.Catalogue.Name, value: like }));
        }

        const query: Query[] = [new Query(
          {
            name: 'catalogues',
            objectType: m.Catalogue,
            predicate: predicate,
            page: new Page({ skip: 0, take: take }),
            include: [
              new TreeNode({ roleType: m.Catalogue.CatalogueImage }),
              new TreeNode({ roleType: m.Catalogue.ProductCategories }),
            ],
          })];

        return this.scope.load('Pull', new PullRequest({ query: query }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.catalogues as Catalogue[];
        this.total = loaded.values.catalogues_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  more(): void {
    this.page$.next(this.data.length + 50);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle('Catalogues');
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  delete(catalogue: Catalogue): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this catalogue?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(catalogue: Catalogue): void {
    this.router.navigate(['/catalogues/catalogues/' + catalogue.id + '/edit']);
  }
}
