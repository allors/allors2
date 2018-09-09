import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, combineLatest, Observable, Subscription } from 'rxjs';

import { ErrorService, FilterFactory, Loaded, Saved, Scope, WorkspaceService } from '../../../../angular';
import { Person, Data } from '../../../../domain';
import { PullRequest } from '../../../../framework';
import { MetaDomain } from '../../../../meta';
import { RadioGroupOption } from '../../../base/components/radiogroup/radiogroup.component';
import { PullFactory } from '../../../../meta/generated/pull.g';

@Component({
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;

  public data: Data;

  public people: Person[];

  public peopleFilter: FilterFactory;

  public radioGroupOptions: RadioGroupOption[] = [
    { label: "One", value: "one" },
    { label: "Two", value: "two" },
    { label: "Three", value: "three" },
  ];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
  ) {

    this.title = 'Form';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.peopleFilter = new FilterFactory({ objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName] });

    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = combineLatest(route$, this.refresh$);

    const x = {};
    const metaPopulation = this.workspaceService.metaPopulation;
    const pull = new PullFactory(metaPopulation);

    this.subscription = combined$
      .switchMap(([]: [UrlSegment[], Date]) => {

        const pulls = [
          pull.Data(
            {
              include: {
                AutocompleteFilter: x,
                AutocompleteOptions: x,
                Chips: x,
                File: x,
                MultipleFiles: x
              }
            }),
          pull.Person(),
        ];

        return this.scope
          .load('Pull', new PullRequest({ pulls }));
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.people = loaded.collections.People as Person[];
        var datas = loaded.collections.Datas as Data[];

        if (datas && datas.length > 0) {
          this.data = datas[0];
        } else {
          this.data = this.scope.session.create("Data") as Data;
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngAfterViewInit(): void {
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe(() => {
        this.data = undefined;
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
  }
}
