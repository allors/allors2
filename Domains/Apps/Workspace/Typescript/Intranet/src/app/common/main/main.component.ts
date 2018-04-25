import { Component } from "@angular/core";
import { Subscription } from "rxjs";
import { Observable } from "rxjs/Observable";

import { Loaded, MenuItem, MenuService, Scope, WorkspaceService } from "../../../allors/angular";
import { StateService } from "../../../allors/covalent/apps/services/StateService";
import { Organisation } from "../../../allors/domain";
import { Equals, Fetch, PullRequest, Query } from "../../../allors/framework";
import { MetaDomain } from "../../../allors/meta";

@Component({
  templateUrl: "./main.component.html",
})
export class MainComponent {
  public selectedInternalOrganisation: Organisation;
  public internalOriganisations: Organisation[];

  public usermenu: any[] = [
    { icon: "tune", route: ".", title: "Account settings" },
    { icon: "exit_to_app", route: ".", title: "Sign out" },
  ];

  public modules: MenuItem[] = [];
  public m: MetaDomain;

  private scope: Scope;
  private subscription: Subscription;

  constructor(
    public menu: MenuService,
    private stateService: StateService,
    private workspaceService: WorkspaceService) {

      this.modules = this.menu.modules;
      this.scope = this.workspaceService.createScope();
      this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    this.subscription = this.stateService.internalOrganisationId$
     .switchMap((internalOrganisationId) => {

      const fetches = [
        new Fetch({
          id: internalOrganisationId,
          name: "internalOrganisation",
        }),
      ];

      const queries = [
        new Query(
          {
            name: "internalOrganisations",
            objectType: this.m.Organisation,
            predicate: new Equals({ roleType: this.m.Organisation.IsInternalOrganisation, value: true }),
          }),
      ];

      return this.scope
        .load("Pull", new PullRequest({ fetches, queries }));
    })
    .subscribe((loaded: Loaded) => {
      this.scope.session.reset();
      this.internalOriganisations = loaded.collections.internalOrganisations as Organisation[];
      this.selectedInternalOrganisation = loaded.objects.internalOrganisation as Organisation;
    });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public selectInternalOrganisation(internalOrganisation: Organisation) {
    this.stateService.internalOrganisationId = internalOrganisation.id;
  }

}
