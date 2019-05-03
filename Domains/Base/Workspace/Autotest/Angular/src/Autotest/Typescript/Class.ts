import { ClassDeclaration, isPropertyDeclaration, PropertyDeclaration, ClassElement, isMethodDeclaration, MethodDeclaration, TypeChecker, InterfaceDeclaration } from 'typescript';
import * as tsutils from "tsutils";

import { Property } from './Property';
import { Member } from './Member';
import { Method } from './Method';
import { Program } from './Program';
import { Type } from './Type';

export class Class implements Type {

    base: Class;

    decorators: string[];

    members: Member[];

    constructor(public name: string, private declaration: ClassDeclaration, private program: Program) {
        program.typeByName[name] = this;

        if(this.declaration.decorators){
            this.decorators = this.declaration.decorators.map((v) => v.getText());
        }

        const { typeChecker } = program;
        if (this.declaration.heritageClauses) {
            this.declaration.heritageClauses.forEach((heritageClause) => {
                heritageClause.types.forEach((heritageClauseType) => {
                    let type = typeChecker.getTypeFromTypeNode(heritageClauseType)
                    if (tsutils.isTypeReference(type)) {
                        let declarations = type.target.getSymbol().declarations
                        for (const declaration of declarations) {
                            if (tsutils.isClassDeclaration(declaration)) {
                                this.base = this.program.lookupOrMap(declaration) as Class;
                            }
                        }
                    }
                })
            });
        }

        this.members = this.declaration.members.map((v) => {

            if (isPropertyDeclaration(v)) {
                return new Property(v, this.program);
            }

            if (isMethodDeclaration(v)) {
                return new Method(v);
            }

            return undefined;
        }).filter((v) => v);
    }

    public toJSON(): any {

        const { name, decorators, base, members } = this;

        return {
            kind: 'class',
            name,
            base,
            decorators,
            members,
        };
    }
}

