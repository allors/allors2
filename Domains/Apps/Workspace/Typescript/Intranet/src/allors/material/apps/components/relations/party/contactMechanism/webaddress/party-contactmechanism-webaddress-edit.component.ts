import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../../../angular';
import { Enumeration, PartyContactMechanism, WebAddress } from '../../../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../../base/services/dialog';

@Component({
  templateUrl: './party-contactmechanism-webaddress.html',
})
export class PartyContactMechanismEditWebAddressComponent implements OnInit, OnDestroy {

  public title = 'Web Address';
  public subTitle = 'edit web address';

  public m: MetaDomain;

  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: WebAddress;
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

          const roleId = this.route.snapshot.paramMap.get('roleId');

          const pulls = [
            pull.PartyContactMechanism({
              object: roleId,
              include: {
                ContactPurposes: x,
              }
            }),
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(m.ContactMechanismPurpose.Name),
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.partyContactMechanism = loaded.objects.partyContactMechanism as PartyContactMechanism;
        this.contactMechanism = this.partyContactMechanism.ContactMechanism as WebAddress;
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
