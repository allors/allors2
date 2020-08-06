import { NodePluginArgs, NodeInput } from 'gatsby';

import { ISessionObject } from '@allors/domain/system';

import { ContentDigests } from './ContentDigests';
import { createName } from './utils/createName';

export class NodeMapper {
  constructor(private args: NodePluginArgs) {}

  public map(objects: ISessionObject[], contentDigests: ContentDigests): void {
    const ids = objects.map((v) => v.id);

    const {
      actions: { createNode },
      createNodeId,
      createContentDigest,
    } = this.args;
    const objectById = objects.reduce((map, value) => {
      map[value.id] = value;
      return map;
    }, {} as { [key: string]: ISessionObject });

    objects.forEach((v) => {
      const sessionObject = v;
      const workspaceObject = sessionObject.workspaceObject;
      const type = workspaceObject.objectType;

      type.interfaces
        .filter((w) => w._isGatsby)
        .forEach((w) => {
          const interfaceNode: NodeInput = {
            id: createNodeId(`allors-${w.name.toLowerCase()}-${workspaceObject.id}`),
            parent: null,
            children: [],
            internal: {
              type: w._name,
              contentDigest: contentDigests.byId[sessionObject.id],
            },
          };

          w.classes
            .filter((x) => x._isGatsby)
            .forEach((x) => {
              interfaceNode[x.name] = createNodeId(`allors-${sessionObject.id}`);
            });

          createNode(interfaceNode);
        });

      const classNode: NodeInput = {
        id: createNodeId(`allors-${workspaceObject.id}`),
        parent: null,
        children: [],
        internal: {
          type: type._name,
          contentDigest: contentDigests.byId[sessionObject.id],
        },
      };

      // Allors
      classNode['allorsId'] = workspaceObject.id;
      classNode['allorsVersion'] = workspaceObject.version;
      classNode['allorsType'] = workspaceObject.objectType.name;

      // Roles
      type.gatsbyRoleTypes.forEach((roleType) => {
        const role = workspaceObject.roleByRoleTypeId.get(roleType.id);
        const propertyName = createName(roleType.name);

        if (role != null) {
          if (roleType.objectType.isUnit) {
            classNode[propertyName] = role;
          } else {
            if (roleType.objectType.isClass) {
              if (roleType.isOne) {
                if (ids.indexOf(role) > -1) {
                  classNode[propertyName] = createNodeId(`allors-${role}`);
                }
              } else {
                const roleIds = (role as string[]).filter((w) => ids.indexOf(w) > -1);
                if (roleIds.length > 0) {
                  classNode[propertyName] = roleIds.map((w) => createNodeId(`allors-${w}`));
                }
              }
            } else {
              if (roleType.isOne) {
                if (objectById[role]) {
                  classNode[propertyName] = createNodeId(`allors-${roleType.objectType.name.toLowerCase()}-${role}`);
                }
              } else {
                const roleIds = (role as string[]).filter((w) => objectById[w]);
                if (roleIds.length > 0) {
                  classNode[propertyName] = roleIds.map((w) => createNodeId(`allors-${roleType.objectType.name.toLowerCase()}-${w}`));
                }
              }
            }
          }
        }
      });

      // Associations
      type.gatsbyAssociationTypes.forEach((associationType) => {
        const association = sessionObject.getAssociation(associationType);
        const propertyName = `${createName(associationType.name)}`;

        if (associationType.objectType.isClass) {
          if (associationType.isOne) {
            if (association) {
              classNode[propertyName] = createNodeId(`allors-${association.id}`);
            }
          } else {
            const associationArray = association as ISessionObject[];
            if (associationArray.length > 0) {
              const associationIds = associationArray.map((w) => w.id);
              classNode[propertyName] = associationIds.map((w) => createNodeId(`allors-${w}`));
            }
          }
        } else {
          if (associationType.isOne) {
            if (association) {
              classNode[propertyName] = createNodeId(`allors-${associationType.objectType.name.toLowerCase()}-${association.id}`);
            }
          } else {
            const associationArray = association as ISessionObject[];
            if (associationArray.length > 0) {
              const associationIds = associationArray.map((w) => w.id);
              classNode[propertyName] = associationIds.map((w) =>
                createNodeId(`allors-${associationType.objectType.name.toLowerCase()}-${w}`)
              );
            }
          }
        }
      });

      // Properties
      if (type.gatsbyProperties) {
        type.gatsbyProperties.forEach((property) => {
          const value = (sessionObject as any)[property.name];

          if (value) {
            classNode[property.name] = value;
          }
        });
      }

      createNode(classNode);
    });
  }
}
