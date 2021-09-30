import { PathResolver } from './src/Helpers';
import { Project } from './src/Project';

const pathResolver = new PathResolver('./apps/angular/material/app');
const project = new Project(pathResolver, 'tsconfig.app.json');

export { project };
