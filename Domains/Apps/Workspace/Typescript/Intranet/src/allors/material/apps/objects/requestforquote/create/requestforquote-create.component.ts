import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Organisation, RequestForQuote, Currency, ContactMechanism, Person, Party, PartyContactMechanism, OrganisationContactRelationship, CustomerRelationship } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { switchMap } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ObjectData, CreateData } from '../../../../../../allors/material/base/services/object';

@Component({
  templateUrl: './requestforquote-create.component.html',
  providers: [ContextService]
})
export class RequestForQuoteCreateComponent implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Add Request for Quote';

  public request: RequestForQuote;
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[] = [];
  public contacts: Person[] = [];
  public scope: ContextService;

  public addContactPerson = false;
  public addContactMechanism = false;
  public addOriginator = false;
  private previousOriginator: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  private fetcher: Fetcher;
  internalOrganisation: Organisation;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<RequestForQuoteCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private errorService: ErrorService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([ ]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Currency({ sort: new Sort(m.Currency.Name) })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        this.currencies = loaded.collections.Currencies as Currency[];

        this.request = this.allors.context.create('RequestForQuote') as RequestForQuote;
        this.request.Recipient = this.internalOrganisation;
        this.request.RequestDate = new Date();

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
          id: this.request.id,
          objectType: this.request.objectType,
        };

        this.dialogRef.close(data);      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public originatorSelected(party: Party) {
    if (party) {
      this.updateOriginator(party);
    }
  }

  public originatorCancelled(): void {
    this.addOriginator = false;
  }

  public originatorAdded(party: Party): void {

    this.addOriginator = false;

    const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
    customerRelationship.Customer = party as Party;
    customerRelationship.InternalOrganisation = this.internalOrganisation;

    this.request.Originator = party;
  }

  public partyContactMechanismCancelled() {
    this.addContactMechanism = false;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addContactMechanism = false;

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
    this.request.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public personCancelled(): void {
    this.addContactPerson = false;
  }

  public personAdded(person: Person): void {

    this.addContactPerson = false;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.request.Originator as Organisation;
    organisationContactRelationship.Contact = person;

    this.contacts.push(person);
    this.request.ContactPerson = person;
  }

  private updateOriginator(party: Party) {

    const { pull, tree, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x
                }
              }
            }
          }
        },
      }),
      pull.Party({
        object: party.id,
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.request.Originator !== this.previousOriginator) {
          this.request.FullfillContactMechanism = null;
          this.request.ContactPerson = null;
          this.previousOriginator = this.request.Originator;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.CurrentContacts as Person[];
      }, this.errorService.handler);
  }
}
