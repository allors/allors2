namespace Allors {
  import MethodType = Meta.MethodType;

  export class Method {
        constructor(
            public object: SessionObject,
            public methodType: MethodType) {
        }
    }
}
