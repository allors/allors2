import React from "react"
import { graphql } from 'gatsby'

import Layout from "../components/layout"

const OrganisationPage = ({ data: { allorsOrganisation: { name } } }) => (
  <Layout>
    <h1>{name}</h1>
  </Layout>
)

export const query = graphql`
  query($slug: String!) {
    allorsOrganisation(slug: { eq: $slug }) {
      slug,
      name
    },
  }
`

export default OrganisationPage
