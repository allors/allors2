import * as moment from 'moment/moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { Party, Organisation, Person, OrganisationContactRelationship, OrganisationContactKind } from '../../../../../domain';
import { PullRequest, Equals, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { ObjectData } from '../../../../../../allors/material/core/services/object';
import { SaveService, FiltersService } from '../../../../../../allors/material';

@Component({
  templateUrl: './organisationcontactrelationship-edit.component.html',
  providers: [ContextService]
})
export class OrganisationContactRelationshipEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  partyRelationship: OrganisationContactRelationship;
  title: string;
  addContact = false;

  private subscription: Subscription;
  party: Party;
  person: Person;
  organisation: Organisation;
  organisations: Organisation[];
  contactKinds: OrganisationContactKind[];
  generalContact: OrganisationContactKind;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<OrganisationContactRelationshipEditComponent>,
    public metaService: MetaService,
    public filtersService: FiltersService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x, m } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            pull.OrganisationContactRelationship({
              object: this.data.id,
              include: {
                Organisation: x,
                Contact: x,
                Parties: x
              }
            }),
            pull.Party({
              object: this.data.associationId,
            }),
            pull.Organisation({
            }),
            pull.OrganisationContactKind({
              sort: new Sort(this.m.OrganisationContactKind.Description)
            }),
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.organisations = loaded.collections.Organisations as Organisation[];

        this.contactKinds = loaded.collections.OrganisationContactKinds as OrganisationContactKind[];
        this.generalContact = this.contactKinds.find(v => v.UniqueId === 'eebe4d65-c452-49c9-a583-c0ffec385e98');

        if (isCreate) {
          this.title = 'Add Organisation Contact';

          this.partyRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
          this.partyRelationship.FromDate = moment.utc().toISOString();
          this.partyRelationship.AddContactKind(this.generalContact);

          this.party = loaded.objects.Party as Party;

          if (this.party.objectType.name === m.Person.name) {
            this.person = this.party as Person;
            this.partyRelationship.Contact = this.person;
          }

          if (this.party.objectType.name === m.Organisation.name) {
            this.organisation = this.party as Organisation;
            this.partyRelationship.Organisation = this.organisation;
          }
        } else {
          this.partyRelationship = loaded.objects.OrganisationContactRelationship as OrganisationContactRelationship;
          this.person = this.partyRelationship.Contact;
          this.organisation = this.partyRelationship.Organisation as Organisation;

          if (this.partyRelationship.CanWriteFromDate) {
            this.title = 'Edit Organisation Contact';
          } else {
            this.title = 'View Organisation Contact';
          }
        }
      });
  }

  public contactAdded(contact: Person): void {
    this.partyRelationship.Contact = contact;
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
