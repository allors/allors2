import { PathResolver } from './Autotest/Helpers';
import { Project } from './Autotest/Project';

const pathResolver = new PathResolver("../../Intranet");
const project = new Project(pathResolver, "tsconfig.json");

export { project };