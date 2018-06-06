import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs/Observable";
import { WorkspaceService } from "../allors/angular";

import { Organisation, Singleton } from "../allors/domain";
import { Query, PullRequest, Equals } from "../allors/framework";
import { MetaDomain } from "../allors/meta";
import { Loaded } from "../allors/angular";
import { StateService } from "../allors/material/apps/services/StateService";

@Injectable()
export class ConfigService {

  constructor(private workspaceService: WorkspaceService, private stateService: StateService) { }

  public setup(): Observable<any> {

    const scope = this.workspaceService.createScope();

    const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;
    const queries = [
        new Query({
            name: "internalOrganisations",
            objectType: m.Organisation,
            predicate: new Equals({ roleType: m.Organisation.IsInternalOrganisation, value: true })}),
        new Query({
            name: "singletons",
            objectType: m.Singleton,
            }),
    ];

    return scope
        .load("Pull", new PullRequest({ queries }))
        .do((loaded: Loaded) => {
            const internalOrganisations = loaded.collections.internalOrganisations as Organisation[];

            if (internalOrganisations && internalOrganisations.length > 0) {
                var organisation = internalOrganisations.find(v=>v.id === this.stateService.internalOrganisationId);
                if(!organisation){
                    this.stateService.internalOrganisationId = internalOrganisations[0].id;
                }
            }

            const singletons = loaded.collections.singletons as Singleton[];
            this.stateService.singletonId = singletons[0].id;
        });
  }
}
