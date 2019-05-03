import * as ts from 'typescript';

import { Type } from './Type';
import { PathResolver } from '../Helpers';
import { ProjectSymbols } from 'ngast';
import { Class } from './Class';
import { Interface } from './Interface';
import { Enum } from './Enum';

import { Statement } from './Statements/Statement';
import { VariableStatement } from './Statements/VariableStatement';
import { EmptyStatement } from './Statements/EmptyStatement';
import { ExpressionStatement } from './Statements/ExpressionStatement';
import { IfStatement } from './Statements/IfStatement';
import { DoStatement } from './Statements/DoStatement';
import { WhileStatement } from './Statements/WhileStatement';
import { ForStatement } from './Statements/ForStatement';
import { ForInStatement } from './Statements/ForInStatement';
import { ForOfStatement } from './Statements/ForOfStatement';
import { ContinueStatement } from './Statements/ContinueStatement';
import { BreakStatement } from './Statements/BreakStatement';
import { BreakOrContinueStatement } from './Statements/BreakOrContinueStatement';
import { ReturnStatement } from './Statements/ReturnStatement';
import { WithStatement } from './Statements/WithStatement';
import { SwitchStatement } from './Statements/SwitchStatement';
import { LabeledStatement } from './Statements/LabeledStatement';
import { ThrowStatement } from './Statements/ThrowStatement';
import { TryStatement } from './Statements/TryStatement';
import { DebuggerStatement } from './Statements/DebuggerStatement';

import { Expression } from './Expression/Expression';
import { ArrayLiteralExpression } from './Expression/ArrayLiteralExpression';
import { ObjectLiteralExpression } from './Expression/ObjectLiteralExpression';
import { PropertyAccessExpression } from './Expression/PropertyAccessExpression';
import { ElementAccessExpression } from './Expression/ElementAccessExpression';
import { CallExpression } from './Expression/CallExpression';
import { NewExpression } from './Expression/NewExpression';
import { TaggedTemplateExpression } from './Expression/TaggedTemplateExpression';
import { ParenthesizedExpression } from './Expression/ParenthesizedExpression';
import { FunctionExpression } from './Expression/FunctionExpression';
import { ArrowFunction } from './Expression/ArrowFunction';
import { DeleteExpression } from './Expression/DeleteExpression';
import { TypeOfExpression } from './Expression/TypeOfExpression';
import { VoidExpression } from './Expression/VoidExpression';
import { AwaitExpression } from './Expression/AwaitExpression';
import { PrefixUnaryExpression } from './Expression/PrefixUnaryExpression';
import { PostfixUnaryExpression } from './Expression/PostfixUnaryExpression';
import { BinaryExpression } from './Expression/BinaryExpression';
import { ConditionalExpression } from './Expression/ConditionalExpression';
import { TemplateExpression } from './Expression/TemplateExpression';
import { YieldExpression } from './Expression/YieldExpression';
import { SpreadElement } from './Expression/SpreadElement';
import { ClassExpression } from './Expression/ClassExpression';
import { OmittedExpression } from './Expression/OmittedExpression';
import { ExpressionWithTypeArguments } from './Expression/ExpressionWithTypeArguments';
import { AsExpression } from './Expression/AsExpression';
import { NonNullExpression } from './Expression/NonNullExpression';
import { Block } from './Statements/Block';
import { CaseBlock } from './Statements/CaseBlock';

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

    lookupOrMap(typeDeclaration: ts.ClassDeclaration | ts.InterfaceDeclaration | ts.EnumDeclaration): Type {
        const fileName = this.pathResolver.relative(typeDeclaration.getSourceFile().fileName);
        const typeName = typeDeclaration.name.getText();
        const name = `${typeName}, ${fileName}`;

        let type = this.typeByName[name];
        if (!type) {
            if (ts.isInterfaceDeclaration(typeDeclaration)) {
                type = new Interface(name, typeDeclaration, this);
            } else if (ts.isClassDeclaration(typeDeclaration)) {
                type = new Class(name, typeDeclaration, this);
            } else {
                type = new Enum(name, typeDeclaration, this);
            }
        }

        return type;
    }

    createExpression(expression: ts.Expression): Expression {
        if (ts.isArrayLiteralExpression(expression)) {
            return new ArrayLiteralExpression(expression, this);
        }

        if (ts.isObjectLiteralExpression(expression)) {
            return new ObjectLiteralExpression(expression, this);
        }

        if (ts.isPropertyAccessExpression(expression)) {
            return new PropertyAccessExpression(expression, this);
        }

        if (ts.isElementAccessExpression(expression)) {
            return new ElementAccessExpression(expression, this);
        }

        if (ts.isCallExpression(expression)) {
            return new CallExpression(expression, this);
        }

        if (ts.isNewExpression(expression)) {
            return new NewExpression(expression, this);
        }

        if (ts.isTaggedTemplateExpression(expression)) {
            return new TaggedTemplateExpression(expression, this);
        }

        if (ts.isParenthesizedExpression(expression)) {
            return new ParenthesizedExpression(expression, this);
        }

        if (ts.isFunctionExpression(expression)) {
            return new FunctionExpression(expression, this);
        }

        if (ts.isArrowFunction(expression)) {
            return new ArrowFunction(expression, this);
        }

        if (ts.isDeleteExpression(expression)) {
            return new DeleteExpression(expression, this);
        }

        if (ts.isTypeOfExpression(expression)) {
            return new TypeOfExpression(expression, this);
        }

        if (ts.isVoidExpression(expression)) {
            return new VoidExpression(expression, this);
        }

        if (ts.isAwaitExpression(expression)) {
            return new AwaitExpression(expression, this);
        }

        if (ts.isPrefixUnaryExpression(expression)) {
            return new PrefixUnaryExpression(expression, this);
        }

        if (ts.isPostfixUnaryExpression(expression)) {
            return new PostfixUnaryExpression(expression, this);
        }

        if (ts.isBinaryExpression(expression)) {
            return new BinaryExpression(expression, this);
        }

        if (ts.isConditionalExpression(expression)) {
            return new ConditionalExpression(expression, this);
        }

        if (ts.isTemplateExpression(expression)) {
            return new TemplateExpression(expression, this);
        }

        if (ts.isYieldExpression(expression)) {
            return new YieldExpression(expression, this);
        }

        if (ts.isSpreadElement(expression)) {
            return new SpreadElement(expression, this);
        }

        if (ts.isClassExpression(expression)) {
            return new ClassExpression(expression, this);
        }

        if (ts.isOmittedExpression(expression)) {
            return new OmittedExpression(expression, this);
        }

        if (ts.isExpressionWithTypeArguments(expression)) {
            return new ExpressionWithTypeArguments(expression, this);
        }

        if (ts.isAsExpression(expression)) {
            return new AsExpression(expression, this);
        }

        if (ts.isNonNullExpression(expression)) {
            return new NonNullExpression(expression, this);
        }

        const kind = ts.SyntaxKind[expression.kind];
        throw new Error("unknown expression " + kind);
    }

    createStatement(statement: ts.Statement): Statement {
        if (ts.isVariableStatement(statement)) {
            return new VariableStatement(statement, this);
        }

        if (ts.isEmptyStatement(statement)) {
            return new EmptyStatement(statement, this);
        }

        if (ts.isExpressionStatement(statement)) {
            return new ExpressionStatement(statement, this);
        }

        if (ts.isIfStatement(statement)) {
            return new IfStatement(statement, this);
        }

        if (ts.isDoStatement(statement)) {
            return new DoStatement(statement, this);
        }

        if (ts.isWhileStatement(statement)) {
            return new WhileStatement(statement, this);
        }

        if (ts.isForStatement(statement)) {
            return new ForStatement(statement, this);
        }

        if (ts.isForInStatement(statement)) {
            return new ForInStatement(statement, this);
        }

        if (ts.isForOfStatement(statement)) {
            return new ForOfStatement(statement, this);
        }

        if (ts.isContinueStatement(statement)) {
            return new ContinueStatement(statement, this);
        }

        if (ts.isBreakStatement(statement)) {
            return new BreakStatement(statement, this);
        }

        if (ts.isBreakOrContinueStatement(statement)) {
            return new BreakOrContinueStatement(statement, this);
        }

        if (ts.isReturnStatement(statement)) {
            return new ReturnStatement(statement, this);
        }

        if (ts.isWithStatement(statement)) {
            return new WithStatement(statement, this);
        }

        if (ts.isSwitchStatement(statement)) {
            return new SwitchStatement(statement, this);
        }

        if (ts.isLabeledStatement(statement)) {
            return new LabeledStatement(statement, this);
        }

        if (ts.isThrowStatement(statement)) {
            return new ThrowStatement(statement, this);
        }

        if (ts.isTryStatement(statement)) {
            return new TryStatement(statement, this);
        }

        if (ts.isDebuggerStatement(statement)) {
            return new DebuggerStatement(statement, this);
        }

        if (ts.isBlock(statement)){
            return new Block(statement, this);
        }

        if (ts.isCaseBlock(statement)){
            return new CaseBlock(statement, this);
        }

        const kind = ts.SyntaxKind[statement.kind];
        throw new Error("unknown statement " + kind);
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

