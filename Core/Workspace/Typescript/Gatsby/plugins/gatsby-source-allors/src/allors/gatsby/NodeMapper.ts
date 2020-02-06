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
    const workspace = session.workspace;
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

    ids.forEach((id) => {

      const sessionObject = session.get(id);
      const workspaceObject = workspace.get(id);
      const type = workspaceObject.objectType;

      if (type._isGatsby && type.isClass) {
        const node = {
          id: createNodeId(`allors-${workspaceObject.id}`),
          parent: null,
          children: [],
          internal: {
            type: `Allors${type.name}`,
            contentDigest: contentDigestById[id],
          }
        }

        // Allors
        node["allorsId"] = workspaceObject.id;
        node["allorsVersion"] = workspaceObject.version;
        node["allorsType"] = workspaceObject.objectType.name;

        // Roles
        type.gatsbyRoleTypes.forEach((roleType => {
          const role = workspaceObject.roleByRoleTypeId.get(roleType.id);
          let propertyName = createName(roleType.name);

          if (!!role) {
            if (roleType.objectType.isUnit) {
              if (roleType.mediaType === "text/markdown") {
                node[propertyName] = createNodeId(`allors-${workspaceObject.id}-${roleType.id}`);;
              } else {
                node[propertyName] = role;
              }
            } else {
              if (roleType.isOne) {
                if (ids.indexOf(role) > -1) {
                  node[propertyName] = createNodeId(`allors-${role}`);
                }
              } else {
                const roleIds = (role as string[]).filter(w => ids.indexOf(w) > -1);
                if (roleIds.length > 0) {
                  node[propertyName] = roleIds.map((w) => createNodeId(`allors-${w}`));
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
              node[propertyName] = createNodeId(`allors-${association.id}`);
            }
          } else {
            const associationArray = (association as ISessionObject[]);
            if (associationArray.length > 0) {
              const associationIds = associationArray.map(w => w.id);
              node[propertyName] = associationIds.map((w) => createNodeId(`allors-${w}`));
            }
          }
        }))

        // Properties
        if (type.gatsbyProperties) {
          type.gatsbyProperties.forEach((property => {
            const value = sessionObject[property.name];

            if (!!value) {
              node[property.name] = value;
            }

          }))
        }

        createNode(node);

        // Child Roles
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
                node[propertyName] = gatsbyChildId;
              }
            }
          }
        }))
      }
    })
  }
}
