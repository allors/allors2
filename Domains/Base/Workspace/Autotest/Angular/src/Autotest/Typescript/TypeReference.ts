import * as ts from 'typescript';
import * as tsutils from "tsutils";

import { Program } from './Program';

export class TypeReference {
    name: string;

    arguments: TypeReference[];

    constructor(node: ts.TypeNode, program: Program) {
        if (ts.isTypeReferenceNode(node)) {
            const { typeChecker } = program;
            const tsType = typeChecker.getTypeFromTypeNode(node)
            const symbol = tsType.getSymbol() || tsType.aliasSymbol;
            if (symbol) {
                const typeDeclaration = symbol.valueDeclaration || symbol.declarations[0]; // TODO:
                if (typeDeclaration) {
                    if (ts.isInterfaceDeclaration(typeDeclaration) || ts.isClassDeclaration(typeDeclaration) || ts.isEnumDeclaration(typeDeclaration)) {
                        this.name = program.lookupOrMap(typeDeclaration).name;
                    } else if (ts.isTypeParameterDeclaration(typeDeclaration) || ts.isTypeAliasDeclaration(typeDeclaration) || ts.isVariableDeclaration(typeDeclaration)) {
                        this.name = symbol.name;
                    } else {
                        var kind = ts.SyntaxKind[typeDeclaration.kind];
                        this.name = symbol.name;
                    }
                }

                if (node.typeArguments) {
                    this.arguments = node.typeArguments.map((v) => new TypeReference(v, program));
                }
            } else {
                if (tsType.isLiteral) {
                    if (tsType.isNumberLiteral) {
                        this.name = "NumberLiteral";
                    } else if (tsType.isStringLiteral) {
                        this.name = "StringLiteral";
                    } else {
                        this.name = "Literal";
                    }
                }
            }
        }

        if (ts.isArrayTypeNode(node)) {
            this.name = "Array";
            this.arguments = [new TypeReference(node.elementType, program)];
        }

        if (!this.name) {
            var kind = ts.SyntaxKind[node.kind];
            this.name = node.getText();
            console.log(this.name);
        }
    }

    public toJSON(): any {

        const { name: type, arguments: typeArguments } = this;

        return {
            kind: 'typeReference',
            type,
            typeArguments,
        };
    }
}

