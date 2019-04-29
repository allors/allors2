import { PropertyDeclaration } from 'typescript';

import { Member } from './Member';

export class Property implements Member {

    name: string;
    type: string;
    decorators: string[];
    initializer: string;

    constructor(declaration: PropertyDeclaration) {

        this.name = declaration.name.getText();
        if (declaration.type) {
            this.type = declaration.type.getText();
        }

        this.decorators = declaration.decorators ? declaration.decorators.map(v=>v.getText()) : undefined;
        this.initializer = declaration.initializer ? declaration.initializer.getText() : undefined;
    }

    public toJSON(): any {

        const { name, type, decorators, initializer } = this;

        return {
            kind: 'property',
            name,
            type,
            decorators,
            initializer
        };
    }
}

