import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Context } from '@allors/angular/services/core';
import { Employment, Person, Party, Organisation } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, ids } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId, FetcherService } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './employment-edit.component.html',
  providers: [ContextService]
})
export class EmploymentEditComponent extends TestScope implements OnInit, OnDestroy {

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

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<EmploymentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
    private fetcher: FetcherService) {

    super();

    this.m = this.metaService.m;
  }

  static canCreate(createData: ObjectData, context: Context) {

    const organisationId = ids.Organisation;
    if (createData.associationObjectType.id === organisationId) {
      const organisation = context.session.get(createData.associationId) as Organisation;
      return organisation.IsInternalOrganisation;
    }

    return true;
  }

  public ngOnInit(): void {

    const { pull, x, m } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Person(),
          ];

          if (!isCreate) {
            pulls.push(
              pull.Employment({
                object: this.data.id,
                include: {
                  Employee: x,
                  Employer: x,
                  Parties: x
                }
              }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Party({
                object: this.data.associationId,
              }),
            );
          }

          return this.allors.context
            .load(new PullRequest({ pulls }))
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
          this.partyRelationship.FromDate = new Date().toISOString();
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
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
