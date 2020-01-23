import { MetaPopulation } from '../../framework/meta/MetaPopulation';
import { ObjectType } from '../../framework';

declare module '../../framework/meta/MetaPopulation' {
  interface MetaPopulation {

    gatsbyDerive(): void;
  }
}

MetaPopulation.prototype["gatsbyDerive"] = function (this: MetaPopulation) {

  // sync
  this.objectTypes.forEach((v => { v._isGatsby = false}))

  this.classes.forEach((v) => {
    v.roleTypes.forEach((w) => {
      if (w.isGatsby) {
        v._isGatsby = true;
        w.objectType._isGatsby = true;
        w.objectType.classes.forEach((x) => x._isGatsby = true)
      }
    })

    v.associationTypes.forEach((w) => {
      if (w.isGatsby) {
        v._isGatsby = true;
        w.objectType._isGatsby = true;
        w.objectType.classes.forEach((x) => x._isGatsby = true)
      }
    })

    if(v.gatsbyProperties && v.gatsbyProperties.length > 0){
      v._isGatsby = true;
    }
  })


  this.objectTypes.forEach((v) => v.gatsbyDerive());
}
