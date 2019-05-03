import { isPropertyDeclaration, PropertyDeclaration, isMethodDeclaration, MethodDeclaration, EnumDeclaration, SyntaxKind } from 'typescript';
import * as tsutils from "tsutils";

import { Property } from './Property';
import { Member } from './Member';
import { Method } from './Method';
import { Program } from './Program';
import { Type } from './Type';
import { isEnumMember } from 'tsutils';

export class Enum implements Type {

    decorators: string[];

    members: string[];

    constructor(public name: string, private declaration: EnumDeclaration, private program: Program) {
        program.typeByName[name] = this;

        if (this.declaration.decorators) {
            this.decorators = this.declaration.decorators.map((v) => v.getText());
        }

        this.members = this.declaration.members.map((v) => v.name.getText());
    }

    public toJSON(): any {

        const { name, decorators, members } = this;

        return {
            kind: 'enum',
            name,
            decorators,
            members,
        };
    }
}

