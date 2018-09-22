import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Catalogue } from '../../../../../domain';
import { And, Like, Predicate, PullRequest, Sort, Equals } from '../../../../../framework';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

interface SearchData {
  name?: string;
}

@Component({
  templateUrl: './catalogues-overview.component.html',
})
export class CataloguesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Catalogues';
  public data: Catalogue[];
  public filtered: Catalogue[];

  public search$: BehaviorSubject<SearchData>;
  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.search$ = new BehaviorSubject<SearchData>({});
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

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

          predicates.push(new Equals({ propertyType: m.Catalogue.InternalOrganisation, object: internalOrganisationId }));

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.Catalogue.Name, value: like }));
          }

          const pulls = [
            pull.Catalogue(
              {
                predicate,
                include: {
                  CatalogueImage: x,
                  ProductCategories: x,
                },
                sort: new Sort(m.Catalogue.Name),
              }
            )];

          return this.scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.data = loaded.collections.Catalogues as Catalogue[];
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }

  public delete(catalogue: Catalogue): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this catalogue?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(catalogue.Delete)
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

  public onView(catalogue: Catalogue): void {
    this.router.navigate(['/catalogue/' + catalogue.id]);
  }
}
