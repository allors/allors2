import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs/Observable";
import { WorkspaceService } from "../allors/angular";
import { Loaded } from "../allors/angular/base/framework";
import { StateService } from "../allors/covalent/apps/services/StateService";
import { Organisation } from "../allors/domain";
import { Query } from "../allors/framework";
import { PullRequest } from "../allors/framework/database/pull/PullRequest";
import { Equals } from "../allors/framework/database/query/Equals";
import { MetaDomain } from "../allors/meta";

@Injectable()
export class ConfigService {

  constructor(private workspaceService: WorkspaceService, private stateService: StateService) { }

  public setup(): Observable<any> {

    const scope = this.workspaceService.createScope();

    const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;
    const query = [
        new Query({
        name: "internalOrganisations",
        objectType: m.Organisation,
        predicate: new Equals({ roleType: m.Organisation.IsInternalOrganisation, value: true })}),
    ];

    return scope
        .load("Pull", new PullRequest({ query }))
        .do((loaded) => {
            const organisations = loaded.collections.internalOrganisations as Organisation[];
            // TODO: Select a default
            const organisation = organisations[0];
            if (organisation) {
                this.stateService.internalOrganisationId = organisation.id;
            }
        });
  }
}
