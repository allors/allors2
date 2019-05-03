import { ClassDeclaration, isPropertyDeclaration, PropertyDeclaration, ClassElement, isMethodDeclaration, MethodDeclaration, TypeChecker, InterfaceDeclaration } from 'typescript';
import * as tsutils from "tsutils";

import { Property } from './Property';
import { Member } from './Member';
import { Method } from './Method';
import { Program } from './Program';
import { Type } from './Type';

export class Interface implements Type {

    decorators: string[];

    members: Member[];

    constructor(public name: string, private declaration: InterfaceDeclaration, private program: Program) {
        program.typeByName[name] = this;

        if(this.declaration.decorators){
            this.decorators = this.declaration.decorators.map((v) => v.getText());
        }

        this.members = this.declaration.members.map((v) => {

            if (isPropertyDeclaration(v)) {
                return new Property(v as PropertyDeclaration, this.program);
            }

            if (isMethodDeclaration(v)) {
                return new Method(v as MethodDeclaration, this.program);
            }

            return undefined;
        }).filter((v) => v);
    }

    public toJSON(): any {

        const { name, decorators, members } = this;

        return {
            kind: 'interface',
            name,
            decorators,
            members,
        };
    }
}

