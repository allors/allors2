import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs/Subscription';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from '../../../../../../../angular';
import { Country, Enumeration, PartyContactMechanism, PostalAddress } from '../../../../../../../domain';
import { Fetch, PullRequest, Query, Sort, TreeNode } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { DialogService } from '../../../../../../base/services/dialog';

@Component({
  templateUrl: './party-contactmechanism-postaladdress.html',
})
export class PartyContactMechanismPostalAddressEditComponent implements OnInit, OnDestroy {

  public title = 'Postal Address';
  public subTitle = 'edit postal address';

  public m: MetaDomain;

  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: PostalAddress;
  public contactMechanismPurposes: Enumeration[];
  public countries: Country[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: DialogService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const roleId: string = this.route.snapshot.paramMap.get('roleId');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            name: 'partyContactMechanism',
            id: roleId,
            include: [
              new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
              new TreeNode({
                roleType: m.PartyContactMechanism.ContactMechanism,
                nodes: [
                  new TreeNode({
                    roleType: m.PostalAddress.PostalBoundary,
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                  }),
                ],
              }),
            ],
          }),
        ];

        const queries: Query[] = [
          new Query(
            {
              name: 'contactMechanismPurposes',
              objectType: this.m.ContactMechanismPurpose,
            }),
          new Query(
            {
              name: 'countries',
              objectType: this.m.Country,
              sort: [new Sort({ roleType: m.Country.Name, direction: 'Asc' })],
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {

        this.partyContactMechanism = loaded.objects.partyContactMechanism as PartyContactMechanism;
        this.contactMechanism = this.partyContactMechanism.ContactMechanism as PostalAddress;
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
