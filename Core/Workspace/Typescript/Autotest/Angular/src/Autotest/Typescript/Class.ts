import { ClassDeclaration, isPropertyDeclaration, PropertyDeclaration, ClassElement, isMethodDeclaration, MethodDeclaration, Program, TypeChecker } from 'typescript';
import * as tsutils from "tsutils";

import { Property } from './Property';
import { Member } from './Member';
import { Method } from './Method';
import { PathResolver } from '../Helpers';

export class Class {

    name: string;
    bases: string[];
    decorators: string[];
    members: Member[];

    constructor(classDeclaration: ClassDeclaration, program: Program, pathResolver: PathResolver) {
        const typeChecker = program.getTypeChecker();

        this.name = classDeclaration.name.text;
        this.decorators = classDeclaration.decorators ? classDeclaration.decorators.map((v) => v.getText()) : [];

        const flattenedMembers = new Array<ClassElement>();
        this.flattenMembers(flattenedMembers, classDeclaration, typeChecker);

        this.members = flattenedMembers.map((v) => {

            if (isPropertyDeclaration(v)) {
                return new Property(v as PropertyDeclaration);
            }

            if (isMethodDeclaration(v)) {
                return new Method(v as MethodDeclaration);
            }

            return undefined;
        }).filter((v) => v);
    }

    public toJSON(): any {

        const { name, bases, members } = this;

        return {
            kind: 'class',
            name,
            bases,
            members,
        };
    }

    private flattenMembers(members: ClassElement[], classDeclaration: ClassDeclaration, typeChecker: TypeChecker) {

        if (classDeclaration.heritageClauses) {
            classDeclaration.heritageClauses.forEach((heritageClause) => {
                heritageClause.types.forEach((heritageClauseType) => {
                    let type = typeChecker.getTypeFromTypeNode(heritageClauseType)
                    if (tsutils.isTypeReference(type)) {
                        let declarations = type.target.symbol.declarations
                        for (const declaration of declarations) {
                            if (tsutils.isClassDeclaration(declaration)) {
                                const baseClass = declaration;

                                this.bases = this.bases || [];
                                this.bases.push(baseClass.name.text)

                                this.flattenMembers(members, baseClass, typeChecker);
                            }
                        }
                    }
                })
            });
        }

        members.push(...classDeclaration.members);
    }
}

