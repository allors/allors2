const path = require('path')

const personTemplate = path.resolve(`./src/templates/person.tsx`)

/** @param {import("gatsby").CreatePagesArgs } args*/
exports.createPages = async ({ graphql, actions }) => {
  const { createPage } = actions
  const result = await graphql(
    `
        {
            allAllorsPerson {
                edges {
                    node {
                        slug,
                    }
                }
            },
        }
        `
  );

  if (result.errors) {
    throw result.errors
  }

  /** @type {import("./graphql-types").AllorsPersonConnection } */
  const allAllorsPerson = result.data.allAllorsPerson;
  allAllorsPerson.edges.forEach((edge) => {
    const slug = edge.node.slug;
    createPage({
      path: slug,
      component: personTemplate,
      context: {
        slug: slug,
      }
    })
  })
}
