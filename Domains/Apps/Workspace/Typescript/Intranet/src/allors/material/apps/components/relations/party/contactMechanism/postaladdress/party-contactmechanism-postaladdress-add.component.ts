import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../../../angular';
import { Country, Enumeration, Party, PartyContactMechanism, PostalAddress, PostalBoundary } from '../../../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../../base/services/dialog';

@Component({
  templateUrl: './party-contactmechanism-postaladdress.html',
})
export class PartyContactMechanismPostalAddressAddComponent implements OnInit, OnDestroy {

  public title = 'Postal Address';
  public subTitle = 'add postal address';

  public m: MetaDomain;

  public party: Party;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: PostalAddress;
  public postalBoundary: PostalBoundary;
  public contactMechanismPurposes: Enumeration[];
  public countries: Country[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Party(
              {
                object: id,
                include: {
                  PartyContactMechanisms: {
                    ContactPurposes: x,
                    ContactMechanisms: {
                      PostalBoundary: {
                        Country: x,
                      }
                    }
                  }
                }
              }
            ),
            pull.ContactMechanismPurpose(
              {
                predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
                sort: new Sort(m.ContactMechanismPurpose.Name),
              }
            ),
            pull.Country({
              sort: new Sort(m.Country.Name),
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.party = loaded.objects.party as Party;

        if (!this.contactMechanism) {
          this.contactMechanism = this.scope.session.create('PostalAddress') as PostalAddress;
          this.postalBoundary = this.scope.session.create('PostalBoundary') as PostalBoundary;
          this.contactMechanism.PostalBoundary = this.postalBoundary;
        }

        this.partyContactMechanism = this.scope.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.partyContactMechanism.ContactMechanism = this.contactMechanism;
        this.partyContactMechanism.UseAsDefault = true;

        this.party.AddPartyContactMechanism(this.partyContactMechanism);

        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as Enumeration[];
        this.countries = loaded.collections.countries as Country[];
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
