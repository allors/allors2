import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  InternalOrganisation,
  Person,
  OrganisationContactRelationship,
  Party,
  PartyContactMechanism,
  ContactMechanism,
  WorkTask,
} from '@allors/domain/generated';
import { Sort } from '@allors/data/system';
import { FetcherService, InternalOrganisationId, Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  templateUrl: './worktask-create.component.html',
  providers: [ContextService]
})
export class WorkTaskCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  public title = 'Add Work Task';

  add: boolean;
  edit: boolean;

  internalOrganisation: InternalOrganisation;
  workTask: WorkTask;
  contactMechanisms: ContactMechanism[];
  contacts: Person[];
  addContactPerson = false;
  addContactMechanism: boolean;

  locales: Locale[];

  private subscription: Subscription;
  private readonly refresh$: BehaviorSubject<Date>;
  organisations: Organisation[];
  organisationsFilter: SearchFactory;
  subContractorsFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<WorkTaskCreateComponent>,
    public metaService: MetaService,
    public navigationService: NavigationService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private route: ActivatedRoute,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest([this.route.url, this.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(() => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
            pull.Organisation({
              sort: new Sort(m.Organisation.PartyName)
            })
          ];

          this.organisationsFilter = Filters.organisationsFilter(m);
          this.subContractorsFilter = Filters.subContractorsFilter(m, this.internalOrganisationId.value);

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.organisations = loaded.collections.Organisations as Organisation[];

        this.workTask = this.allors.context.create('WorkTask') as WorkTask;
        this.workTask.TakenBy = this.internalOrganisation as Organisation;

      });
  }

  public customerSelected(customer: ISessionObject) {
    this.updateCustomer(customer as Party);
  }

  private updateCustomer(party: Party) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_Country: x
              }
            }
          }
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      })
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe(
      (loaded) => {
        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms
          .map((v: PartyContactMechanism) => v.ContactMechanism);

        this.contacts = loaded.collections.CurrentContacts as Person[];
      });
  }

  public contactPersonAdded(contact: Person): void {

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.workTask.Customer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.workTask.ContactPerson = contact;
  }

  public contactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.workTask.Customer.AddPartyContactMechanism(partyContactMechanism);
    this.workTask.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
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
        const data: IObject = {
          id: this.workTask.id,
          objectType: this.workTask.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
