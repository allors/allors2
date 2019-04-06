import * as path from 'path';
import { Project } from './Compiler/Project';
import { data } from '../../../Workspace/Typescript/modules/Material/src/allors/meta';
import { domain } from '../../../Workspace/Typescript/modules/Material/src/allors/domain';
import { MetaPopulation, Workspace } from '../../../Workspace/Typescript/modules/Material/src/allors/framework';
import { menu, MenuItem } from '../../../Workspace/Typescript/modules/Material/src/app/main/main.menu';
import { routes } from '../../../Workspace/Typescript/modules/Material/src/app/app-routing.module';

const projectPath = path.resolve("../../Workspace/Typescript/modules/Material/src/tsconfig.app.json");
const project = new Project(projectPath);

const metaPopulation = new MetaPopulation(data);
const workspace = new Workspace(metaPopulation);
domain.apply(this.workspace);

export { project };
export { workspace, Workspace };
export { menu, MenuItem };
export { routes };
