import { Extent } from './Extent';
import { ISessionObject } from '../../workspace';
import { Result } from './Result';
import { FlatPull } from './FlatPull';
import { ObjectType } from '../../meta/ObjectType';
import { MetaObjectType } from '../../meta';
import { Filter } from './Filter';
import { Fetch } from './Fetch';
import { Sort } from './Sort';
import { Tree } from './Tree';

export class Pull {

  public extentRef: string;

  public extent: Extent;

  public object: ISessionObject | string;

  public results: Result[];

  constructor(fields?: Partial<Pull> | ObjectType | MetaObjectType, flat?: FlatPull) {
    if (fields instanceof ObjectType || (fields as MetaObjectType)._objectType) {
      const objectType = (fields as MetaObjectType)._objectType ? (fields as MetaObjectType)._objectType : fields as ObjectType;

      if (!flat) {
        this.extent = new Filter({ objectType: objectType });
      } else {
        this.object = flat.object;
        this.extent = flat.extent;
        this.extentRef = flat.extentRef;

        const sort = flat.sort instanceof Sort ? [flat.sort] : flat.sort;

        if (flat.predicate) {
          if (this.object || this.extent || this.extentRef) {
            throw new Error('predicate conflicts with object/extent/extentRef');
          }

          this.extent = new Filter({ objectType: objectType, predicate: flat.predicate, sort });
        }

        if (!this.object && !this.extent && !this.extentRef) {
          this.extent = new Filter({ objectType: objectType, sort });
        }

        if (flat.fetchRef || flat.fetch || flat.include || flat.name || flat.skip || flat.take) {
          const result = new Result({
            fetchRef: flat.fetchRef,
            fetch: flat.fetch ? flat.fetch instanceof Fetch ? flat.fetch : new Fetch(objectType, flat.fetch) : undefined,
            name: flat.name,
            skip: flat.skip,
            take: flat.take
          });

          if (flat.include) {
            const include = flat.include instanceof Tree ? flat.include : new Tree(objectType, flat.include);

            if (result.fetch) {
              if (result.fetch.step) {
                throw new Error('include conflicts with fetch step');
              }

              result.fetch.include = include;
            } else {
              result.fetch = new Fetch({ include });
            }
          }

          this.results = this.results || [];
          this.results.push(result);
        }
      }
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    const sessionObject = this.object as ISessionObject;

    return {
      extentRef: this.extentRef,
      extent: this.extent,
      object: sessionObject && sessionObject.id ? sessionObject.id : this.object,
      results: this.results,
    };
  }
}
