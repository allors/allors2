import { Project } from './Compiler/Project';
import { PathResolver } from './Compiler/Helpers';

const pathResolver = new PathResolver("../../../Workspace/Typescript/modules/Material");
const project = new Project(pathResolver, "src/tsconfig.app.json");

export { project };