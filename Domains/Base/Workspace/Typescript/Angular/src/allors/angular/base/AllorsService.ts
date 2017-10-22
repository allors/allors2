import { Workspace, } from "../../domain";
import { MetaDomain } from "../../meta";
import { Database } from "./Database";

export abstract class AllorsService {
  public abstract workspace: Workspace;
  public abstract database: Database;
  public abstract meta: MetaDomain;

  public abstract back();
}
