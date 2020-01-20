const path = require(`path`)

const organisationTemplate = path.resolve(`./src/templates/organisation.tsx`)

/** @param {import("gatsby").CreatePagesArgs } args*/
exports.createPages = async ({ graphql, actions }) => {
  const { createPage } = actions
  const result = await graphql(
    `
        {
            allAllorsOrganisation {
                edges {
                    node {
                        slug,
                        name,
                    }
                }
            },
        }
        `
  );

  if (result.errors) {
    throw result.errors
  }

  /** @type {import("./graphql-types").AllorsOrganisationConnection } */
  const allAllorsOrganisation = result.data.allAllorsOrganisation
  allAllorsOrganisation.edges.forEach((edge) => {
    const slug = edge.node.slug;
    createPage({
      path: slug,
      component: organisationTemplate,
      context: {
        slug: slug,
      }
    })
  })
}
