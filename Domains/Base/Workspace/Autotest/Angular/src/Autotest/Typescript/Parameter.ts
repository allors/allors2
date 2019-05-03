import { ParameterDeclaration } from 'typescript';
import { TypeRef } from './TypeRef';
import { Program } from './Program';

export class Parameter {

    name: string;
    typeRef: TypeRef;

    constructor(declaration: ParameterDeclaration, program: Program) {

        this.name = declaration.name.getText();
        if (declaration.type) {
            this.typeRef = new TypeRef(declaration.type, program);
        }
    }

    public toJSON(): any {

        const { name, typeRef } = this;

        return {
            kind: 'parameter',
            name,
            typeRef,
        };
    }
}

