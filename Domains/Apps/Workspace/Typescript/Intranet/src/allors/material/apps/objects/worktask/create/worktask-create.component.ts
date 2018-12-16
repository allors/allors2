import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, ContextService, NavigationService, MetaService } from '../../../../../angular';
import { InternalOrganisation, Locale, WorkTask, Organisation } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './worktask-create.component.html',
  providers: [ContextService]
})
export class WorkTaskCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  add: boolean;
  edit: boolean;

  internalOrganisation: InternalOrganisation;
  workTask: WorkTask;

  locales: Locale[];

  private subscription: Subscription;
  private readonly refresh$: BehaviorSubject<Date>;
  private readonly fetcher: Fetcher;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<WorkTaskCreateComponent>,
    public metaService: MetaService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(
      this.route.url,
      this.refresh$,
      this.stateService.internalOrganisationId$
    )
      .pipe(
        switchMap(([]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        this.workTask = this.allors.context.create('WorkTask') as WorkTask;
        this.workTask.TakenBy = this.internalOrganisation as Organisation;

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.workTask.id,
          objectType: this.workTask.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
