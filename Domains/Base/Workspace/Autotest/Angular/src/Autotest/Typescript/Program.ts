import * as ts from 'typescript';

import { Type } from './Type';
import { PathResolver } from '../Helpers';
import { ProjectSymbols } from 'ngast';
import { Class } from './Class';
import { Interface } from './Interface';
import { Enum } from './Enum';

export class Program {
    tsProgram: ts.Program;
    typeChecker: ts.TypeChecker;

    typeByName: { [name: string]: Type; } = {};

    constructor(public projectSymbols: ProjectSymbols, public pathResolver: PathResolver) {
        this.tsProgram = (projectSymbols as any).program;
        this.typeChecker = this.tsProgram.getTypeChecker();

        this.projectSymbols.getDirectives().forEach((v) => {

            const classDeclaration = v.getNode();
            if (classDeclaration) {
                this.lookupOrMap(classDeclaration);
            }
        })
    }

    public lookupOrMap(typeDeclaration: ts.ClassDeclaration | ts.InterfaceDeclaration | ts.EnumDeclaration): Type {
        const fileName = this.pathResolver.relative(typeDeclaration.getSourceFile().fileName);
        const typeName = typeDeclaration.name.getText();
        const name = `${typeName}, ${fileName}`;

        let type = this.typeByName[name];
        if (!type) {
            if(ts.isInterfaceDeclaration(typeDeclaration))
            {
                type = new Interface(name, typeDeclaration, this);
            } else if(ts.isClassDeclaration(typeDeclaration)) {
                type = new Class(name, typeDeclaration, this);
            } else{
                type = new Enum(name, typeDeclaration, this);
            }
        }

        return type;
    }

    public toJSON(): any {
        const { typeByName } = this;
        const types = Object.values(typeByName)

        return {
            kind: 'program',
            types,
        };
    }
}

