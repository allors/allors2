import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class ForInStatement implements Statement {
    constructor(statement: ts.ForInStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'forInStatement',
        };
    }
}

