module.exports = {
  siteMetadata: {
    siteName: `Allors Gatsby`,
  },
  plugins: [
    `gatsby-plugin-typescript`,
    `gatsby-plugin-graphql-codegen`,
    {
      resolve: "gatsby-source-allors",
      options: {
        url: "http://localhost:5000/",
        login: "TestAuthentication/Token",
        user: "administrator",
        password: undefined
      },
    },
    `gatsby-transformer-sharp`,
    `gatsby-plugin-sharp`,
  ],
}
