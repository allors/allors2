import * as ts from 'typescript';

import { Program } from './Program';

export class TypeRef {
    name: string;

    arguments: TypeRef[];

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
                    this.arguments = node.typeArguments.map((v) => new TypeRef(v, program));
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
        } else if (ts.isArrayTypeNode(node)) {
            this.name = "Array";
            this.arguments = [new TypeRef(node.elementType, program)];
        } else if (ts.isTypeLiteralNode(node)) {
            this.name = node.getText();
        } else if (ts.isTypeQueryNode(node)) {
            this.name = node.getText();
        } else if (ts.isUnionTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isIntersectionTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isConditionalTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isTypeReferenceNode(node)) {
            this.name = node.getText();
        } else if (ts.isTypePredicateNode(node)) {
            this.name = node.getText();
        } else if (ts.isTypeOperatorNode(node)) {
            this.name = node.getText();
        } else if (ts.isFunctionTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isTupleTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isImportTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isMappedTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isInferTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isThisTypeNode(node)) {
            this.name = node.getText();
        } else if (ts.isParenthesizedTypeNode(node)) {
            this.name = node.getText();
        } else {
            switch (node.kind) {
                case ts.SyntaxKind.AnyKeyword:
                case ts.SyntaxKind.UnknownKeyword:
                case ts.SyntaxKind.NumberKeyword:
                case ts.SyntaxKind.BigIntKeyword:
                case ts.SyntaxKind.ObjectKeyword:
                case ts.SyntaxKind.BooleanKeyword:
                case ts.SyntaxKind.StringKeyword:
                case ts.SyntaxKind.SymbolKeyword:
                case ts.SyntaxKind.ThisKeyword:
                case ts.SyntaxKind.VoidKeyword:
                case ts.SyntaxKind.UndefinedKeyword:
                case ts.SyntaxKind.NullKeyword:
                case ts.SyntaxKind.NeverKeyword:
                    this.name = node.getText();
                    break;
            }
        }

        if (!this.name) {
            this.name = node.getText();

            var kind = ts.SyntaxKind[node.kind];
            console.log(`Fall through for ${this.name} (${kind})`)
        }
    }

    public toJSON(): any {

        const { name: type, arguments: typeArguments } = this;

        return {
            kind: 'typeRef',
            type,
            typeArguments,
        };
    }
}

