import { Loaded } from "../promise";

import { NodePluginArgs } from "gatsby"
import { ISessionObject } from "../framework";
import createName from "./utils/createName";

export class NodeMapper {

  constructor(private args: NodePluginArgs, private options) {
  }

  public map(loaded: Loaded) {

    const { actions: { createNode }, createNodeId, createContentDigest } = this.args;

    const session = loaded.session;
    const ids = loaded.response.objects.map(v => v[0]);

    const contentDigestById = loaded.response.objects
      .reduce((acc, value) => {
        const id = value[0];
        const version = value[1];
        const accessControls = value.length > 2 ? value[2] : undefined;
        const deniedPermissions = value.length > 3 ? value[3] : undefined;

        let contentDigest = `allors-${id}-${version}`;
        if (accessControls) {
          contentDigest += `${accessControls}`
        }

        if (deniedPermissions) {
          contentDigest += `${deniedPermissions}`
        }

        acc[id] = createContentDigest(contentDigest);

        return acc;
      }, {});

    const objects = ids.map((v) => session.get(v)).filter((v) => v.objectType._isGatsby);
    const objectById = objects.reduce((map, value) => { map[value.id] = value; return map; }, {})

    objects.forEach((v) => {

      const sessionObject = v;
      const workspaceObject = sessionObject.workspaceObject;
      const type = workspaceObject.objectType;

      type.interfaces.filter((w) => w._isGatsby).forEach((w) => {

        const interfaceNode = {
          id: createNodeId(`allors-${w.name.toLowerCase()}-${workspaceObject.id}`),
          parent: null,
          children: [],
          internal: {
            type: w._name,
            contentDigest: contentDigestById[sessionObject.id],
          }
        }

        w.classes.filter((x) => x._isGatsby).forEach((x) => {
          interfaceNode[x.name] = createNodeId(`allors-${sessionObject.id}`);
        })

        createNode(interfaceNode);
      });

      const classNode = {
        id: createNodeId(`allors-${workspaceObject.id}`),
        parent: null,
        children: [],
        internal: {
          type: type._name,
          contentDigest: contentDigestById[sessionObject.id],
        }
      }

      // Allors
      classNode["allorsId"] = workspaceObject.id;
      classNode["allorsVersion"] = workspaceObject.version;
      classNode["allorsType"] = workspaceObject.objectType.name;

      // Roles
      type.gatsbyRoleTypes.forEach((roleType => {
        const role = workspaceObject.roleByRoleTypeId.get(roleType.id);
        let propertyName = createName(roleType.name);

        if (!!role) {
          if (roleType.objectType.isUnit) {
            if (roleType.mediaType === "text/markdown") {
              classNode[propertyName] = createNodeId(`allors-${workspaceObject.id}-${roleType.id}`);;
            } else {
              classNode[propertyName] = role;
            }
          } else {
            if (roleType.objectType.isClass) {
              if (roleType.isOne) {
                if (ids.indexOf(role) > -1) {
                  classNode[propertyName] = createNodeId(`allors-${role}`);
                }
              } else {
                const roleIds = (role as string[]).filter(w => ids.indexOf(w) > -1);
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
                const roleIds = (role as string[]).filter(w => objectById[w]);
                if (roleIds.length > 0) {
                  classNode[propertyName] = roleIds.map((w) => createNodeId(`allors-${roleType.objectType.name.toLowerCase()}-${w}`));
                }
              }
            }
          }
        }
      }))

      // Associations
      type.gatsbyAssociationTypes.forEach((associationType => {
        const association = sessionObject.getAssociation(associationType);
        const propertyName = `${createName(associationType.name)}`;

        if (associationType.isOne) {
          if (!!association) {
            classNode[propertyName] = createNodeId(`allors-${association.id}`);
          }
        } else {
          const associationArray = (association as ISessionObject[]);
          if (associationArray.length > 0) {
            const associationIds = associationArray.map(w => w.id);
            classNode[propertyName] = associationIds.map((w) => createNodeId(`allors-${w}`));
          }
        }
      }))

      // Properties
      if (type.gatsbyProperties) {
        type.gatsbyProperties.forEach((property => {
          const value = sessionObject[property.name];

          if (!!value) {
            classNode[property.name] = value;
          }

        }))
      }

      createNode(classNode);

      // Markdown Roles
      type.gatsbyRoleTypes.forEach((roleType => {
        const role = workspaceObject.roleByRoleTypeId.get(roleType.id);
        let propertyName = createName(roleType.name);

        if (!!role) {
          if (roleType.objectType.isUnit) {
            if (roleType.mediaType) {
              const gatsbyChildId = createNodeId(`allors-${workspaceObject.id}-${roleType.id}`);
              createNode({
                id: gatsbyChildId,
                parent: createNodeId(`allors-${workspaceObject.id}`),
                internal: {
                  type: `AllorsMarkdown`,
                  mediaType: roleType.mediaType,
                  content: role,
                  contentDigest: createContentDigest(role),
                }
              });
              classNode[propertyName] = gatsbyChildId;
            }
          }
        }
      }))
    })
  }
}
