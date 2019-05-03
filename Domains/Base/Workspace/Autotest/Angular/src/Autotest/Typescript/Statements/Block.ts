import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class Block implements Statement {

    statements: Statement[];

    constructor(statement: ts.Block, program: Program) {
        this.statements = statement.statements.map((v) => program.createStatement(v));
    }

    public toJSON(): any {

        const { statements } = this;

        return {
            kind: 'block',
            statements
        };
    }
}

