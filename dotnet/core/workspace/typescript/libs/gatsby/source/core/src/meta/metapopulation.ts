import { MetaPopulation } from '@allors/meta/system';

declare module '@allors/meta/system' {
  interface MetaPopulation {
    gatsbyDerive(): void;
  }
}

MetaPopulation.prototype['gatsbyDerive'] = function (this: MetaPopulation) {
  this.classes.forEach((objectType) => {
    objectType.gatsbyRoleTypes = objectType.gatsbyRoleTypes || [];
    objectType.gatsbyAssociationTypes = objectType.gatsbyAssociationTypes || [];
  });

  this.classes.forEach((objectType) => {
    objectType.roleTypes.forEach((roleType) => {
      if (objectType.gatsbyRoleTypes.indexOf(roleType) > -1) {
        objectType._isGatsby = true;
        roleType.objectType._isGatsby = true;
      }
    });

    objectType.associationTypes.forEach((associationType) => {
      if (objectType.gatsbyAssociationTypes.indexOf(associationType) > -1) {
        objectType._isGatsby = true;
        associationType.objectType._isGatsby = true;
      }
    });

    if (objectType.gatsbyProperties && objectType.gatsbyProperties.length > 0) {
      objectType._isGatsby = true;
    }
  });

  this.objectTypes.forEach((v) => v.gatsbyDerive());
};
