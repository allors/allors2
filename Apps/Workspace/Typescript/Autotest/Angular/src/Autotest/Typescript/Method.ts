import { MethodDeclaration } from 'typescript';

import { Member } from './Member';
import { Parameter } from './Parameter';

export class Method implements Member {

    name: string;
    type: string;
    decorators: string[];
    parameters: Parameter[];

    constructor(declaration: MethodDeclaration) {

        this.name = declaration.name.getText();
        if (declaration.type) {
            this.type = declaration.type.getText();
        }
        this.decorators = declaration.decorators ? declaration.decorators.map(v=>v.getText()) : undefined;
        this.parameters = declaration.parameters ? declaration.parameters.map(v=> new Parameter(v)) : undefined;
    }


    public toJSON(): any {

        const { name, type, decorators, parameters } = this;

        return {
            kind: 'method',
            name,
            type,
            decorators,
            parameters
        };
    }

}

