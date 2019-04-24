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

        it('should have local components', () => {

            const components = project.directives.filter((v) => v.isLocal && v.isComponent);
            assert.isNotEmpty(components);
        });

        it('should have routed components', () => {

            const components = project.directives.filter((v) => v.isLocal && v.isComponent);
            const rootComponents = components.filter((v) => !v.hasSelector);
            assert.isNotEmpty(rootComponents);
        });
    });
