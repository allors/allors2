import * as ts from 'typescript';
import * as tsutils from "tsutils";

import { Member } from './Member';
import { Program } from './Program';
import { TypeReference } from './TypeReference';

export class Property implements Member {

    name: string;
    typeReference: TypeReference;
    decorators: string[];
    initializer: string;

    constructor(declaration: ts.PropertyDeclaration, program: Program) {

        const { typeChecker } = program;

        this.name = declaration.name.getText();
        if (declaration.type) {
            this.typeReference = new TypeReference(declaration.type, program);
        }

        this.decorators = declaration.decorators ? declaration.decorators.map(v => v.getText()) : undefined;
        this.initializer = declaration.initializer ? declaration.initializer.getText() : undefined;
    }

    public toJSON(): any {

        const { name, typeReference, decorators, initializer } = this;

        return {
            kind: 'property',
            name,
            typeReference,
            decorators,
            initializer
        };
    }
}

