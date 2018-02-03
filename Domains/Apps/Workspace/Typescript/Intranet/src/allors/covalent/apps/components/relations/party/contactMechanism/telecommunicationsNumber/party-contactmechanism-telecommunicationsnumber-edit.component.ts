import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Subscription } from "rxjs/Subscription";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../../../angular";
import { Enumeration, PartyContactMechanism, TelecommunicationsNumber } from "../../../../../../../domain";
import { Fetch, PullRequest, Query, TreeNode } from "../../../../../../../framework";
import { MetaDomain } from "../../../../../../../meta";

@Component({
  templateUrl: "./party-contactmechanism-telecommunicationsnumber.html",
})
export class PartyContactMechanismTelecommunicationsNumberEditComponent implements OnInit, OnDestroy {

  public title: string = "Telecommunications Number";
  public subTitle: string = "edit telecommunications number";

  public m: MetaDomain;

  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: TelecommunicationsNumber;
  public contactMechanismPurposes: Enumeration[];
  public contactMechanismTypes: Enumeration[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const roleId: string = this.route.snapshot.paramMap.get("roleId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: "partyContactMechanism",
            id: roleId,
            include: [
              new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
              new TreeNode({
                roleType: m.PartyContactMechanism.ContactMechanism,
                nodes: [new TreeNode({ roleType: m.ContactMechanism.ContactMechanismType })],
              }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "contactMechanismPurposes",
              objectType: this.m.ContactMechanismPurpose,
            }),
          new Query(
            {
              name: "contactMechanismTypes",
              objectType: this.m.ContactMechanismType,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.partyContactMechanism = loaded.objects.partyContactMechanism as PartyContactMechanism;
        this.contactMechanism = this.partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as Enumeration[];
        this.contactMechanismTypes = loaded.collections.contactMechanismTypes as Enumeration[];
      },
      (error: any) => {
        this.errorService.message(error);
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
        this.errorService.dialog(error);
      });
  }

  public goBack(): void {
    window.history.back();
  }
}
