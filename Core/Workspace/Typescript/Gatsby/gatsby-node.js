const path = require(`path`)
const catalogueTemplate = path.resolve(`./src/templates/catalogue.js`)

exports.createPages = async ({ graphql, actions }) => {
    // const { createPage } = actions
    // const result = await graphql(
    //     `
    //     {
    //         allProductCategoryAllors {
    //             edges {
    //                 node {
    //                     slug,
    //                     name,
    //                 }
    //             }
    //         },
    //     }
    //     `
    // );

    // if (result.errors) {
    //     throw result.errors
    // }

    // const productCategoryEdges = result.data.allProductCategoryAllors.edges
    // productCategoryEdges.forEach((edge) => {
    //     createPage({
    //         path: edge.node.slug,
    //         component: catalogueTemplate,
    //     })
    // })
}
