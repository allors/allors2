import { Meta } from '../allors/meta';
import { WorkspaceService } from '../allors/angular';

export function appInit(workspaceService: WorkspaceService) {

  const { metaPopulation } = workspaceService;
  const m = metaPopulation as Meta;

  m.Person.icon = 'people';
  m.Organisation.icon = 'business';
}

