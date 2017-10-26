import { AfterViewInit, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { Title } from "@angular/platform-browser";
import { ActivatedRoute } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { And, Equals, Fetch, Like, Or, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "@allors";
import { AllorsService, ErrorService, Filter, Loaded, Saved, Scope } from "@allors";
import { Enumeration, Locale, Organisation, Person } from "@allors";
import { RoleType } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./organisation.component.html",
})
export class OrganisationComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;
  public people: Person[];

  public organisation: Organisation;

  public peopleFilter: Filter;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    private media: TdMediaService) {

    this.title = "Organisation";
    this.titleService.setTitle(this.title);
    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;

    this.peopleFilter = new Filter({scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName]});
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");

        const m: MetaDomain = this.allorsService.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: "organisation",
            id,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "people",
              objectType: this.m.Person,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.organisation = loaded.objects.organisation as Organisation;
        if (!this.organisation) {
          this.organisation = this.scope.session.create("Organisation") as Organisation;
        }

        this.people = loaded.collections.people as Person[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
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

  public ownerSelected(person: Person): void {
    console.log(person.displayName);
  }
}
