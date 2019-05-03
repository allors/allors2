import { MethodDeclaration } from 'typescript';

import { Member } from './Member';
import { Parameter } from './Parameter';
import { TypeRef } from './TypeRef';
import { Program } from './Program';
import { Statement } from './Statements/Statement';

export class Method implements Member {

    name: string;
    typeRef: TypeRef;
    decorators: string[];
    parameters: Parameter[];
    body: Statement[];

    constructor(declaration: MethodDeclaration, program: Program) {

        this.name = declaration.name.getText();
        if (declaration.type) {
            this.typeRef = new TypeRef(declaration.type, program);
        }

        this.decorators = declaration.decorators ? declaration.decorators.map(v => v.getText()) : undefined;
        this.parameters = declaration.parameters ? declaration.parameters.map(v => new Parameter(v, program)) : undefined;

        const body = declaration.body;
        if (body && body.statements) {
            this.body = body.statements.map((v) => program.createStatement(v));
        }
    }

    public toJSON(): any {

        const { name, typeRef, decorators, parameters, body } = this;

        return {
            kind: 'method',
            name,
            typeRef,
            decorators,
            parameters,
            body
        };
    }

}

