import { MetaDomain } from '../../meta';
import { Workspace, } from '../../domain';
import { Database } from './Database';

export abstract class AllorsService {
  abstract workspace: Workspace;
  abstract database: Database;
  abstract meta: MetaDomain;
}
