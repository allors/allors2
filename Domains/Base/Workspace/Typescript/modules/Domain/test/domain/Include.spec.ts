import { assert } from "chai";
import "mocha";

import { domain, Organisation, Person } from "../../src/allors/domain";
import { Fetch, MetaPopulation, Path, PullResponse, PushResponse, ResponseType, Session, Workspace } from "../../src/allors/framework";
import { data, IncludeOrganisation, MetaDomain } from "../../src/allors/meta";

describe("Fetch",
    () => {

        let metaPopulation: MetaPopulation;
        let workspace: Workspace;

        beforeEach(() => {
            metaPopulation = new MetaPopulation(data);
            workspace = new Workspace(metaPopulation);
            domain.apply(workspace);
        });

        it("should parse from json", () => {

           
        });
    },
);
