import { Observable, BehaviorSubject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Predicate, And, Or, Not, Equals, Like, Contains, TreeNode, Sort, Page } from '../../../../domain';
import { Organisation, OrganisationRole } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../angular';

interface SearchData {
  name: string;
}

@Component({
  templateUrl: './organisations.component.html',
})
export class OrganisationsComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  title: string = 'Organisations';

  total: number;

  searchForm: FormGroup;

  data: Organisation[];

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

        const organisationRolesQuery: Query[] = [new Query(
          {
            name: 'organisationRoles',
            objectType: m.OrganisationRole,
          })];

        return this.scope
          .load('Pull', new PullRequest({ query: organisationRolesQuery }))
          .switchMap((loaded: Loaded) => {
            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const customerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === 'Customer');

            const predicate: And = new And({
              predicates: [
                new Contains({ roleType: m.Organisation.OrganisationRoles, object: customerRole }),
              ],
            });

            const predicates: Predicate[] = predicate.predicates;
            if (data.name) {
              const like: string = data.name.replace('*', '%') + '%';
              predicates.push(new Like({ roleType: m.Organisation.Name, value: like }));
            }

            const query: Query[] = [new Query(
              {
                name: 'organisations',
                objectType: m.Organisation,
                predicate: predicate,
                page: new Page({ skip: 0, take: take }),
                include: [
                  new TreeNode({
                    roleType: m.Organisation.GeneralCorrespondence,
                    nodes: [
                      new TreeNode({
                        roleType: m.PostalAddress.PostalBoundary,
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                      }),
                    ],
                  }),
                ],
                sort: [new Sort({ roleType: m.Organisation.Name })],
              })];

            return this.scope.load('Pull', new PullRequest({ query: query }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.organisations as Organisation[];
        this.total = loaded.values.organisations_total;
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
    this.titleService.setTitle('Organisations');
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  delete(organisation: Organisation): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this person?' })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(organisation.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  onView(organisation: Organisation): void {
    this.router.navigate(['/relations/organisations/' + organisation.id + '/overview']);
  }
}
