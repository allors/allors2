import { Component, EventEmitter, OnInit , Output } from "@angular/core";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { Enumeration, Locale, Person, PersonRole } from "../../../../../domain";
import { PullRequest, Query } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  selector: "person-inline",
  templateUrl: "./person-inline.component.html",
})
export class PersonInlineComponent implements OnInit {

  @Output()
  public saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public person: Person;

  public m: MetaDomain;

  public locales: Locale[];
  public genders: Enumeration[];
  public salutations: Enumeration[];
  public roles: PersonRole[];

  private scope: Scope;

  constructor(private workspaceService: WorkspaceService, private errorService: ErrorService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    const query: Query[] = [
      new Query(
        {
          name: "locales",
          objectType: this.m.Locale,
        }),
      new Query(
        {
          name: "genders",
          objectType: this.m.GenderType,
        }),
      new Query(
        {
          name: "salutations",
          objectType: this.m.Salutation,
        }),
      new Query(
        {
          name: "roles",
          objectType: this.m.PersonRole,
        }),
    ];

    this.scope
      .load("Pull", new PullRequest({ query }))
      .subscribe((loaded) => {
        this.locales = loaded.collections.locales as Locale[];
        this.genders = loaded.collections.genders as Enumeration[];
        this.salutations = loaded.collections.salutations as Enumeration[];
        this.roles = loaded.collections.roles as PersonRole[];

        this.person = this.scope.session.create("Person") as Person;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.person.id);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }
}
