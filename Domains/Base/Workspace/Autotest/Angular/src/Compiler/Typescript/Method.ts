import { MethodDeclaration } from 'typescript';

import { Member } from './Member';

export class Method implements Member {

    name: string;
    type: string;

    constructor(declaration: MethodDeclaration) {

        this.name = declaration.name.getText();

        if (declaration.type) {
            this.type = declaration.type.getText();
        }
    }


    public toJSON(): any {

        const { name, type } = this;

        return {
            kind: 'method',
            name,
            type,
        };
    }

}

