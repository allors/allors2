import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Invoked, Loaded, Saved, Scope } from "../../../../angular";
import { Contains, Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "../../../../domain";
import {
  ContactMechanism, Currency, Organisation, OrganisationRole, Party, PartyContactMechanism,
  Person, PersonRole, RequestForQuote, WebAddress} from "../../../../domain";
import { MetaDomain } from "../../../../meta";

@Component({
  templateUrl: "./edit.component.html",
})
export class RequestEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public request: RequestForQuote;
  public people: Person[];
  public organisations: Organisation[];
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];

  public addEmailAddress: boolean = false;
  public addPostalAddress: boolean = false;
  public addTeleCommunicationsNumber: boolean = false;
  public addWebAddress: boolean = false;

  public peopleFilter: Filter;
  public organisationsFilter: Filter;
  public currenciesFilter: Filter;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  get showOrganisations(): boolean {
    return !this.request.Originator || this.request.Originator instanceof (Organisation);
  }
  get showPeople(): boolean {
    return !this.request.Originator || this.request.Originator instanceof (Person);
  }

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.peopleFilter = new Filter({scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName]});
    this.organisationsFilter = new Filter({scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
    this.currenciesFilter = new Filter({scope: this.scope, objectType: this.m.Currency, roleTypes: [this.m.Currency.Name]});
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const rolesQuery: Query[] = [
          new Query(
            {
              name: "organisationRoles",
              objectType: m.OrganisationRole,
            }),
          new Query(
            {
              name: "personRoles",
              objectType: m.PersonRole,
            }),
          new Query(
            {
              name: "currencies",
              objectType: this.m.Currency,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ query: rolesQuery }))
          .switchMap((loaded: Loaded) => {
            this.currencies = loaded.collections.currencies as Currency[];

            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const oCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Customer");

            const personRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const pCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Customer");

            const fetch: Fetch[] = [
              new Fetch({
                id,
                include: [
                  new TreeNode({ roleType: m.Request.Currency }),
                  new TreeNode({ roleType: m.Request.Originator }),
                  new TreeNode({ roleType: m.Request.RequestState }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                        roleType: m.PostalAddress.PostalBoundary,
                      }),
                    ],
                    roleType: m.Request.FullfillContactMechanism,
                  })],
                name: "requestForQuote",
              }),
            ];

            const query: Query[] = [
              new Query(
                {
                  name: "organisations",
                  objectType: this.m.Organisation,
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: oCustomerRole }),
                }),
              new Query(
                {
                  name: "persons",
                  objectType: this.m.Person,
                  predicate: new Contains({ roleType: m.Person.PersonRoles, object: pCustomerRole }),
                }),
            ];

            return this.scope.load("Pull", new PullRequest({ fetch, query }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.request = loaded.objects.requestForQuote as RequestForQuote;
        if (!this.request) {
          this.request = this.scope.session.create("RequestForQuote") as RequestForQuote;
        }

        this.originatorSelected(this.request.Originator);

        this.organisations = loaded.collections.organisations as Organisation[];
        this.people = loaded.collections.parties as Person[];
        this.title = "Request " + this.request.RequestNumber + " from: " + this.request.Originator.PartyName;
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public submit(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.request.Submit)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully submitted.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                submitFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            submitFn();
          }
        });
    } else {
      submitFn();
    }
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.request.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public hold(): void {
    const holdFn: () => void = () => {
      this.scope.invoke(this.request.Hold)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully held.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                holdFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            holdFn();
          }
        });
    } else {
      holdFn();
    }
  }

  public reject(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.request.Reject)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully rejected.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                rejectFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            rejectFn();
          }
        });
    } else {
      rejectFn();
    }
  }

  public webAddressCancelled(): void {
    this.addWebAddress = false;
  }

  public webAddressAdded(id: string): void {
    this.addWebAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
  }

  public emailAddressCancelled(): void {
    this.addEmailAddress = false;
  }

  public emailAddressAdded(id: string): void {
    this.addEmailAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
  }

  public postalAddressCancelled(): void {
    this.addPostalAddress = false;
  }

  public postalAddressAdded(id: string): void {
    this.addPostalAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
  }

  public teleCommunicationsNumberCancelled(): void {
    this.addTeleCommunicationsNumber = false;
  }

  public teleCommunicationsNumberAdded(id: string): void {
    this.addTeleCommunicationsNumber = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
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
        this.router.navigate(["/orders/request/" + this.request.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public originatorSelected(party: Party): void {

    const fetch: Fetch[] = [
      new Fetch({
        id: party.id,
        include: [
          new TreeNode({
            nodes: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: this.m.PostalBoundary.Country }),
                ],
                roleType: this.m.PostalAddress.PostalBoundary,
              }),
            ],
            roleType: this.m.PartyContactMechanism.ContactMechanism,
          }),
        ],
        name: "partyContactMechanisms",
        path: new Path({ step: this.m.Party.CurrentPartyContactMechanisms }),
      }),
    ];

    this.scope
      .load("Pull", new PullRequest({ fetch }))
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
