import { assert } from 'chai';
import 'mocha';
import * as path from 'path';

import { Program } from '../src/Program';

describe('Program',
    () => {
        describe('with sample project',
            () => {
                var project = path.resolve("./sample/src/tsconfig.app.json");
                const program = new Program(project);

                it('should construct', () => {

                    assert.isDefined(program);
                });

                it('should have local components', () => {
                    const { application } = program;

                    const components = application.directives.filter((v) => v.isLocal && v.isComponent);
                    assert.equal(5, components.length);
                });

                it('should have routed components', () => {
                    const { application } = program;

                    const components = application.directives.filter((v) => v.isLocal && v.isComponent);
                    const rootComponents = components.filter((v) => !v.hasSelector);
                    assert.equal(2, rootComponents.length);
                });

            });

    });
