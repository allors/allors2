// <copyright file="TestCollection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests
{
    using Xunit;

    [CollectionDefinition("Test collection")]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}
