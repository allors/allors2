import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../../../angular';
import { EmailAddress, Enumeration, Party, PartyContactMechanism } from '../../../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../../base/services/dialog';

@Component({
  templateUrl: './party-contactmechanism-emailaddress.html',
})
export class PartyContactMechanismEmailAddressAddComponent implements OnInit, OnDestroy {

  public title = 'Email Address';
  public subTitle = 'add email address';

  public m: MetaDomain;

  public party: Party;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: EmailAddress;
  public contactMechanismPurposes: Enumeration[];

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

          const id = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Party(
              {
                object: id,
                include: {
                  PartyContactMechanisms: {
                    ContactPurposes: x,
                    ContactMechanism: x,
                  }
                }
              }
            ),
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(m.ContactMechanismPurpose.Name)
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.party = loaded.objects.party as Party;

        if (!this.contactMechanism) {
          this.contactMechanism = this.scope.session.create('EmailAddress') as EmailAddress;
        }

        this.partyContactMechanism = this.scope.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.partyContactMechanism.ContactMechanism = this.contactMechanism;
        this.partyContactMechanism.UseAsDefault = true;

        this.party.AddPartyContactMechanism(this.partyContactMechanism);

        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as Enumeration[];
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
