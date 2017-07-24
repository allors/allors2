import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { PullRequest, Query, Predicate, And, Or, Not, Equals, Like, Contains, ContainedIn, TreeNode, Sort, Page } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta/index';
import { Person, WorkEffortAssignment, WorkTask } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../../angular';

interface SearchData {
  description: string;
}

@Component({
  templateUrl: './workTasksOverview.component.html',
})
export class WorkTasksOverviewComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;
  total: number;

  title: string = 'Work Tasks';

  searchForm: FormGroup;

  data: WorkTask[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MdSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    private snackBarService: MdSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    titleService.setTitle(this.title);
    this.scope = new Scope(allors.database, allors.workspace);

    this.searchForm = this.formBuilder.group({
      description: [''],
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

        if (data.description) {
          const like: string = '%' + data.description + '%';
          predicates.push(new Like({ roleType: m.WorkTask.Description, value: like }));
        }

        const query: Query[] = [new Query(
          {
            name: 'worktasks',
            objectType: m.WorkTask,
            predicate: predicate,
            page: new Page({ skip: 0, take: take }),
            // include: [
            //   new TreeNode({ roleType: m.Person.Picture }),
            // ],
          })];

        return this.scope.load('Pull', new PullRequest({ query: query }));

      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.worktasks as WorkTask[];
        this.total = loaded.values.worktasks_total;
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
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  delete(worktask: WorkTask): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this work task?' })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(worktask.Delete)
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

  onView(person: Person): void {
    this.router.navigate(['/relations/people/' + person.id + '/overview']);
  }
}
