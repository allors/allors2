import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Subscription } from "rxjs/Subscription";

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, PartyContactMechanism, WebAddress, Enumeration } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest } from "@allors/framework";

import { template } from "./party-contactmechanism-webaddress.component";

@Component({
  template: template,
})
export class PartyContactMechanismEditWebAddressComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  title: string = "Web Address";
  subTitle: string = "edit web address";

  m: MetaDomain;

  partyContactMechanism: PartyContactMechanism;
  contactMechanism: WebAddress;
  contactMechanismPurposes: Enumeration[];

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
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "contactMechanismPurposes",
              objectType: this.m.ContactMechanismPurpose,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.partyContactMechanism = loaded.objects.partyContactMechanism as PartyContactMechanism;
        this.contactMechanism = this.partyContactMechanism.ContactMechanism as WebAddress;
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as Enumeration[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
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
