module.exports = {
  siteMetadata: {
    siteName: `Allors Gatsby`,
  },
  plugins: [
    {
      resolve: "gatsby-source-allors",
      options: {},
    },
    `gatsby-plugin-typescript`
  ],
}
