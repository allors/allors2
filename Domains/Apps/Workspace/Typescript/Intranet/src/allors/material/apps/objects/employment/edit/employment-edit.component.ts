import * as moment from 'moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import {  ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Employment, Party, Organisation, Person, InternalOrganisation } from '../../../../../domain';
import { PullRequest, Equals, IObject } from '../../../../../framework';
import { CreateData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './employment-edit.component.html',
  providers: [ContextService]
})
export class EmploymentEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  partyRelationship: Employment;
  people: Person[];
  party: Party;
  person: Person;
  organisation: Organisation;
  internalOrganisation: Organisation;
  internalOrganisations: Organisation[];
  title: string;
  addEmployee = false;

  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<EmploymentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { pull, x, m } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Employment({
              object: this.data.id,
              include: {
                Employee: x,
                Employer: x,
                Parties: x
              }
            }),
            pull.Party({
              object: this.data.associationId,
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
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        if (isCreate) {
          this.title = 'Add Employment';

          this.partyRelationship = this.allors.context.create('Employment') as Employment;
          this.partyRelationship.FromDate = moment.utc();
          this.partyRelationship.Employer = this.internalOrganisation;

          this.party = loaded.objects.Party as Party;

          if (this.party.objectType.name === m.Person.name) {
            this.person = this.party as Person;
            this.partyRelationship.Employee = this.person;
          }

          if (this.party.objectType.name === m.Organisation.name) {
            this.organisation = this.party as Organisation;

            if (!this.organisation.IsInternalOrganisation) {
              this.dialogRef.close();
            }
          }
        } else {
          this.partyRelationship = loaded.objects.Employment as Employment;
          this.person = this.partyRelationship.Employee;
          this.organisation = this.partyRelationship.Employer as Organisation;

          if (this.partyRelationship.CanWriteFromDate) {
            this.title = 'Edit Employment';
          } else {
            this.title = 'View Employment';
          }
        }
      });
  }

  public employeeAdded(employee: Person): void {
    this.partyRelationship.Employee = employee;
    this.people.push(employee);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.partyRelationship.id,
          objectType: this.partyRelationship.objectType,
        };

        this.dialogRef.close(data);
      },
      this.saveService.errorHandler
    );
  }
}
