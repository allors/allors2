import * as ts from 'typescript';
import * as tsutils from "tsutils";

import { Member } from './Member';
import { Program } from './Program';
import { TypeRef } from './TypeRef';

export class Property implements Member {

    name: string;
    typeRef: TypeRef;
    decorators: string[];
    initializer: string;

    constructor(declaration: ts.PropertyDeclaration, program: Program) {

        this.name = declaration.name.getText();
        if (declaration.type) {
            this.typeRef = new TypeRef(declaration.type, program);
        }

        this.decorators = declaration.decorators ? declaration.decorators.map(v => v.getText()) : undefined;
        this.initializer = declaration.initializer ? declaration.initializer.getText() : undefined;
    }

    public toJSON(): any {

        const { name, typeRef, decorators, initializer } = this;

        return {
            kind: 'property',
            name,
            typeRef,
            decorators,
            initializer
        };
    }
}

