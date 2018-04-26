import * as chai from "chai";
import { assert } from "chai";
import "mocha";

import { MetaObject, MetaObjectType, MetaPopulation } from "../../src/allors/framework";
import { data, MetaDomain } from "../../src/allors/meta";

describe("MetaDomain",
    () => {

        const metaPopulation = new MetaPopulation(data);
        const metaDomain: MetaDomain = metaPopulation.metaDomain;

        it("is defined",
            () => {
                assert.isDefined(metaDomain);
            });

        it("metaInterfaces should be defined",
            () => {
                data.interfaces.forEach((v) => {
                    assert.isDefined(metaDomain[v.name]);
                });
            });

        it("metaClasses should be defined",
            () => {
                data.classes.forEach((v) => {
                    assert.isDefined(metaDomain[v.name]);
                });
            });

        it("metaObject.objectType should be defined",
            () => {
                data.interfaces.concat(data.classes).forEach((v) => {
                    const metaObjectType: MetaObjectType = metaDomain[v.name];
                    assert.isDefined(metaObjectType.objectType);
                });
            });

        it("metaObject roleTypes should be defined",
            () => {
                data.interfaces.concat(data.classes).forEach((v) => {
                    const metaObjectType: MetaObjectType = metaDomain[v.name];
                    const objectType = metaObjectType.objectType;

                    const roleTypes = Object.keys(objectType.roleTypeByName).map((w) => objectType.roleTypeByName[w]);

                    roleTypes.forEach((w) => {
                        const metaRoleType = metaObjectType[w.name];
                        assert.isDefined(metaRoleType);
                    });
                });
            });

        it("metaObject associationTypes should be defined",
            () => {
                data.interfaces.concat(data.classes).forEach((v) => {
                    const metaObjectType: MetaObjectType = metaDomain[v.name];
                    const objectType = metaObjectType.objectType;

                    const associationTypes = Object.keys(objectType.associationTypeByName).map((w) => objectType.associationTypeByName[w]);

                    associationTypes.forEach((w) => {
                        const metaAssociationType = metaObjectType[w.name];
                        assert.isDefined(metaAssociationType);
                    });
                });
            });
    });
