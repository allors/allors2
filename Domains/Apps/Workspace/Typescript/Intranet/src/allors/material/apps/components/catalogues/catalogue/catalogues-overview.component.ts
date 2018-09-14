import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Loaded, MediaService, Scope, WorkspaceService } from '../../../../../angular';
import { Catalogue, InternalOrganisation } from '../../../../../domain';
import { And, Equals, Fetch, Like, Predicate, PullRequest, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

interface SearchData {
  name: string;
}

@Component({
  templateUrl: './catalogues-overview.component.html',
})
export class CataloguesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Catalogues';
  public total: number;
  public searchForm: FormGroup; public advancedSearch: boolean;
  public data: Catalogue[];
  public filtered: Catalogue[];

  private refresh$: BehaviorSubject<Date>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
    });
  }

  ngOnInit(): void {
    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
          return [data, date, internalOrganisationId];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data, , internalOrganisationId]) => {
          const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

          const predicate: And = new And();
          const operands: Predicate[] = predicate.operands;

          operands.push(new Equals({ propertyType: m.Catalogue.InternalOrganisation, value: internalOrganisationId }));

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            operands.push(new Like({ roleType: m.Catalogue.Name, value: like }));
          }

          const pulls = [
            new Query(
            {
              name: 'catalogues',
              objectType: m.Catalogue,
              predicate,
              include: [
                new TreeNode({ roleType: m.Catalogue.CatalogueImage }),
                new TreeNode({ roleType: m.Catalogue.ProductCategories }),
              ],
              sort: new Sort(m.Catalogue.Name),
            })];

          return this.scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.data = loaded.collections.catalogues as Catalogue[];
        this.total = loaded.values.catalogues_total;
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

  public onView(catalogue: Catalogue): void {
    this.router.navigate(['/catalogue/' + catalogue.id]);
  }
}
