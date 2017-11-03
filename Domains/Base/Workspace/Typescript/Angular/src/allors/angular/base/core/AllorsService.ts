import { Workspace } from "@baseDomain";
import { MetaDomain } from "@generatedMeta/meta.g";
import { Database } from "./Database";

export abstract class AllorsService {
  public abstract workspace: Workspace;
  public abstract database: Database;
  public abstract m: MetaDomain;

  public abstract back();
}
