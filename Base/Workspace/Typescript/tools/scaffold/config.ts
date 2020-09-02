import { PathResolver } from './src/Helpers';
import { Project } from './src/Project';

const pathResolver = new PathResolver('./apps/intranet');
const project = new Project(pathResolver, 'tsconfig.app.json');

export { project };
