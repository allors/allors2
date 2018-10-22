import { Component, OnDestroy, OnInit, ViewChild, Self } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, Loaded, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { CommunicationEvent, CommunicationEventPurpose, CommunicationEventState, Person } from '../../../../../domain';
import { And, Contains, Equals, Like, Predicate, PullRequest, Sort, TreeNode } from '../../../../../framework';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

interface SearchData {
  subject: string;
  state: string;
  purpose: string;
  involved: string;
}

@Component({
  templateUrl: './communicationevents-overview.component.html',
  providers: [Allors]
})
export class CommunicationEventsOverviewComponent implements OnInit, OnDestroy {

  public total: number;

  public title = 'Communications';

  public searchForm: FormGroup; public advancedSearch: boolean;

  public data: CommunicationEvent[];

  public communicationEventStates: CommunicationEventState[];
  public selectedCommunicationEventState: CommunicationEventState;
  public communicationEventState: CommunicationEventState;

  public purposes: CommunicationEventPurpose[];
  public selectedPurpose: CommunicationEventPurpose;
  public purpose: CommunicationEventPurpose;

  public people: Person[];
  public selectedInvolved: Person;
  public involved: Person;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    titleService: Title,
  ) {

    titleService.setTitle(this.title);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      involved: [''],
      purpose: [''],
      state: [''],
      subject: [''],
    });
  }

  ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$: Observable<any> = combineLatest(search$, this.refresh$)
      .pipe(
        scan(([previousData, previousDate], [data, date]) => {
          return [data, date];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data, take]: [SearchData, number]) => {

          const pulls = [
            pull.CommunicationEventState(
              {
                sort: new Sort(m.CommunicationEventState.Name),
              }
            ),
            pull.CommunicationEventPurpose({
              predicate: new Equals({ propertyType: m.CommunicationEventPurpose.IsActive, value: true }),
              sort: new Sort(m.CommunicationEventPurpose.Name),
            }),
            pull.Person({
              sort: new Sort(m.Person.PartyName),
            })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                this.communicationEventStates = loaded.collections.communicationEventStates as CommunicationEventState[];
                this.communicationEventState = this.communicationEventStates.find((v: CommunicationEventState) => v.Name === data.state);

                this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
                this.purpose = this.purposes.find((v: CommunicationEventPurpose) => v.Name === data.purpose);

                this.people = loaded.collections.persons as Person[];
                this.involved = this.people.find((v: Person) => v.displayName === data.involved);

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                if (data.subject) {
                  const like: string = '%' + data.subject + '%';
                  predicates.push(new Like({ roleType: m.CommunicationEvent.Subject, value: like }));
                }

                if (data.state) {
                  predicates.push(new Equals({ propertyType: m.CommunicationEvent.CommunicationEventState, object: this.communicationEventState }));
                }

                if (data.purpose) {
                  predicates.push(new Contains({ propertyType: m.CommunicationEvent.EventPurposes, object: this.purpose }));
                }

                if (data.involved) {
                  predicates.push(new Contains({ propertyType: m.CommunicationEvent.InvolvedParties, object: this.involved }));
                }

                const pulls2 = [
                  pull.CommunicationEvent({
                    predicate,
                    include: {
                      CommunicationEventState: x,
                      FromParties: x,
                      ToParties: x,
                      InvolvedParties: x,
                    },
                    sort: new Sort(m.CommunicationEvent.ScheduledEnd),
                    skip: 0,
                    take,
                  })
                ];

                return scope
                  .load('Pull', new PullRequest({ pulls: pulls2 }));
              })
            );
        })
      )
      .subscribe((loaded) => {

        scope.session.reset();

        this.data = loaded.collections.communicationEvents as CommunicationEvent[];
        this.total = loaded.values.communicationEvents_total;
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

  public cancel(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to cancel this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Cancel)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public close(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to close this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Close)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public reopen(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to reopen this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Reopen)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public delete(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this communication event?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Delete)
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

  public checkType(obj: any): string {
    return obj.objectType.name;
  }

  public onView(person: Person): void {
    this.router.navigate(['/relations/person/' + person.id]);
  }
}
