import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class CaseBlock implements Statement {
    constructor(statement: ts.CaseBlock, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'caseBlock',
        };
    }
}

