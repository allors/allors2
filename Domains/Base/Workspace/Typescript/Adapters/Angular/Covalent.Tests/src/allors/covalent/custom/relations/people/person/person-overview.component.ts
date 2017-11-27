import { AfterViewInit, Component, OnDestroy, OnInit } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { ErrorService, Loaded, Scope } from "@allors";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "@allors";
import { Locale, Organisation, Person } from "@allors";
import { MetaDomain } from "@allors";

import { AllorsService } from "../../../../../../app/allors.service";

@Component({
  templateUrl: "./person-overview.component.html",
})
export class PersonOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public person: Person;
  public locales: Locale[];

  public title: string;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    public media: TdMediaService) {

    this.title = "Person";
    this.titleService.setTitle(this.title);
    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;
  }

  public ngOnInit(): void {

    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Person.Locale }),
            ],
            name: "person",
          }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch }));
      })
      .subscribe((loaded: Loaded) => {
        this.person = loaded.objects.person as Person;
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

  public goBack(): void {
    window.history.back();
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }
}