import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { Country, Enumeration, Party, PartyContactMechanism, PostalAddress, PostalBoundary } from "@allors"
import { Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "@allors";
import { MetaDomain } from "@allors";
import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";

@Component({
  templateUrl: "./form.component.html",
})
export class PartyContactMechanismAddPostalAddressComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  title: string = "Postal Address";
  subTitle: string = "add postal address";

  m: MetaDomain;

  party: Party;
  partyContactMechanism: PartyContactMechanism;
  contactMechanism: PostalAddress;
  postalBoundary: PostalBoundary;
  contactMechanismPurposes: Enumeration[];
  countries: Country[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: "party",
            id,
            include: [
              new TreeNode({
                roleType: m.Party.PartyContactMechanisms,
                nodes: [
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
              name: "countries",
              objectType: this.m.Country,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.party = loaded.objects.party as Party;

        if (!this.contactMechanism) {
          this.contactMechanism = this.scope.session.create("PostalAddress") as PostalAddress;
          this.postalBoundary = this.scope.session.create("PostalBoundary") as PostalBoundary;
          this.contactMechanism.PostalBoundary = this.postalBoundary;
        }

        this.partyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
        this.partyContactMechanism.ContactMechanism = this.contactMechanism;
        this.partyContactMechanism.UseAsDefault = true;

        this.party.AddPartyContactMechanism(this.partyContactMechanism);

        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as Enumeration[];
        this.countries = loaded.collections.countries as Country[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
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

  goBack(): void {
    window.history.back();
  }
}
