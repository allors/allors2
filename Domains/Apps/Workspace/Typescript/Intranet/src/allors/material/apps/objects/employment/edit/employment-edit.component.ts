import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { PartyRelationship, Employment, Person } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { EditData, CreateData, ObjectData } from 'src/allors/material/base/services/object';
import { Sorter } from 'src/allors/material';

@Component({
  templateUrl: './employment-edit.component.html',
  providers: [ContextService]
})
export class EmploymentEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  partyRelationship: PartyRelationship;
  title: string;
  addFromParty = false;

  private subscription: Subscription;
  people: Person[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<EmploymentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x, m } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          const pulls = [
            pull.Employment({
              object: this.data.id,
              include: {
                Employee: x,
                Employer: x,
                Parties: x
              }
            }),
            pull.Person({
            }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.people = loaded.collections.People as Person[];

        if (isCreate) {
          this.title = 'Add Employment';

          this.partyRelationship = this.allors.context.create('Employment') as Employment;
        } else {
          this.partyRelationship = loaded.objects.CustomerRelationship as Employment;

          if (this.partyRelationship.CanWriteFromDate) {
            this.title = 'Edit Employment';
          } else {
            this.title = 'View Employment';
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.partyRelationship.id,
          objectType: this.partyRelationship.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
