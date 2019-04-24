import { Project } from '../Compiler/Project';
import { Route } from '../Compiler/Route';

export class Angular {

    routes: Route[];

    public Load(project: Project) {

        const mainModule = project.mainModule;
        this.routes = mainModule.routes;

        console.log(project.directives.length);
    }
}