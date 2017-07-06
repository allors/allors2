import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, PushResponse, Contains, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import {
  Currency, Organisation, Party, Person, PartyContactMechanism, ContactMechanism,
  OrganisationRole, PersonRole, RequestForQuote,
} from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Filter } from '../../../../../angular';

@Component({
  templateUrl: './request.component.html',
})
export class RequestFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  flex: string = '100%';
  flex2: string = `calc(50%-25px)`;

  m: MetaDomain;

  request: RequestForQuote;
  people: Person[];
  organisations: Organisation[];
  currencies: Currency[];
  contactMechanisms: ContactMechanism[];

  peopleFilter: Filter;
  organisationsFilter: Filter;
  currenciesFilter: Filter;

  get showOrganisations(): boolean {
    return !this.request.Originator || this.request.Originator instanceof (Organisation);
  }
  get showPeople(): boolean {
    return !this.request.Originator || this.request.Originator instanceof (Person);
  }

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;

    this.peopleFilter = new Filter(this.scope, this.m.Person, [this.m.Person.FirstName, this.m.Person.LastName]);
    this.organisationsFilter = new Filter(this.scope, this.m.Organisation, [this.m.Organisation.Name]);
    this.currenciesFilter = new Filter(this.scope, this.m.Currency, [this.m.Currency.Name]);
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'requestForQuote',
            id: id,
            include: [
              new TreeNode({ roleType: m.Request.Originator }),
            ],
          }),
        ];

        const rolesQuery: Query[] = [
          new Query(
            {
              name: 'organisationRoles',
              objectType: m.OrganisationRole,
            }),
          new Query(
            {
              name: 'personRoles',
              objectType: m.PersonRole,
            }),
          new Query(
            {
              name: 'currencies',
              objectType: this.m.Currency,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ query: rolesQuery }))
          .mergeMap((loaded: Loaded) => {
            this.currencies = loaded.collections.currencies as Currency[];

            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const oCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === 'Customer');

            const personRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const pCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === 'Customer');

            const query: Query[] = [
              new Query(
                {
                  name: 'organisations',
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: oCustomerRole }),
                  objectType: this.m.Organisation,
                }),
              new Query(
                {
                  name: 'persons',
                  predicate: new Contains({ roleType: m.Person.PersonRoles, object: pCustomerRole }),
                  objectType: this.m.Person,
                }),
            ];

            return this.scope.load('Pull', new PullRequest({ fetch: fetch, query: query }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.request = loaded.objects.requestForQuote as RequestForQuote;
        if (!this.request) {
          this.request = this.scope.session.create('RequestForQuote') as RequestForQuote;
        }

        this.organisations = loaded.collections.organisations as Organisation[];
        this.people = loaded.collections.parties as Person[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  originatorSelected(party: Party): void {

    const fetch: Fetch[] = [
      new Fetch({
        name: 'partyContactMechanisms',
        id: party.id,
        path: new Path({ step: this.m.Party.CurrentPartyContactMechanisms }),
        include: [
          new TreeNode({
            roleType: this.m.PartyContactMechanism.ContactMechanism,
            nodes: [
              new TreeNode({
                roleType: this.m.PostalAddress.PostalBoundary,
                nodes: [
                  new TreeNode({ roleType: this.m.PostalBoundary.Country }),
                ],
              }),
            ],
          }),
        ],
      }),
    ];

    this.scope
      .load('Pull', new PullRequest({ fetch: fetch }))
      .subscribe((loaded: Loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  goBack(): void {
    window.history.back();
  }
}
