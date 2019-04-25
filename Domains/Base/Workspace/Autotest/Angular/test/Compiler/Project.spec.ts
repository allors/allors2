import { assert } from 'chai';
import 'mocha';

import { project } from '../../src/config';

describe('Project',
    () => {
        it('should be constructed', () => {

            assert.isDefined(project);
        });

        it('should have no parse error', () => {

            assert.isEmpty(project.parseErrors);
        });
    });
