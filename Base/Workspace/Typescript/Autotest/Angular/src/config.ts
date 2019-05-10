import { PathResolver } from './Autotest/Helpers';
import { Project } from './Autotest/Project';

const pathResolver = new PathResolver("../../Material");
const project = new Project(pathResolver, "src/tsconfig.app.json");

export { project };