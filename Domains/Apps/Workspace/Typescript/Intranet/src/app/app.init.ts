import { MetaDomain } from '../allors/meta';
import { WorkspaceService } from '../allors/angular';

export function appInit(workspaceService: WorkspaceService) {

  const { metaPopulation } = workspaceService;
  const m = metaPopulation.metaDomain as MetaDomain;

  m.Person.objectType.icon = 'people';
  m.Organisation.objectType.icon = 'business';
}

