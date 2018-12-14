import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, combineLatest, Observable, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Meta } from '../../../../meta';
import { Person, Data } from '../../../../domain';
import { PullRequest } from '../../../../framework';
import { ErrorService, SearchFactory, Loaded, WorkspaceService, ContextService, MetaService } from '../../../../angular';
import { RadioGroupOption } from '../../../base/components/radiogroup/radiogroup.component';
import { PullFactory } from '../../../../meta/generated/pull.g';

@Component({
  templateUrl: './form.component.html',
  providers: [ContextService]
})
export class FormComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: Meta;

  public data: Data;

  public people: Person[];

  public peopleFilter: SearchFactory;

  public radioGroupOptions: RadioGroupOption[] = [
    { label: 'One', value: 'one' },
    { label: 'Two', value: 'two' },
    { label: 'Three', value: 'three' },
  ];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
  ) {

    this.title = 'Form';
    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;

    this.peopleFilter = new SearchFactory({ objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName] });

    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = combineLatest(route$, this.refresh$);

    const x = {};
    const metaPopulation = this.workspaceService.metaPopulation;
    const pull = new PullFactory(metaPopulation);

    this.subscription = combined$
      .pipe(
        switchMap(([]: [UrlSegment[], Date]) => {

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

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        }))
      .subscribe((loaded: Loaded) => {

        this.allors.context.reset();

        this.people = loaded.collections.People as Person[];
        const datas = loaded.collections.Datas as Data[];

        if (datas && datas.length > 0) {
          this.data = datas[0];
        } else {
          this.data = this.allors.context.create(this.m.Data) as Data;
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

    this.allors.context
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
