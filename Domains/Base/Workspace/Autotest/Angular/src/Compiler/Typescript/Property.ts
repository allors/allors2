import { PropertyDeclaration } from 'typescript';

import { Member } from './Member';

export class Property implements Member {

    name: string;
    type: string;

    constructor(declaration: PropertyDeclaration) {

        this.name = declaration.name.getText();

        if (declaration.type) {
            this.type = declaration.type.getText();
        }
    }


    public toJSON(): any {

        const { name, type } = this;

        return {
            kind: 'property',
            name,
            type,
        };
    }

}

