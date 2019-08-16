//------------------------------------------------------------------------------------------------- 
// <copyright file="ContentTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>Defines the ContentTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Tests
{
    public abstract class ContentTests : DomainTest
    {
        protected static byte[] GetByteArray() => GetByteArray("Some string");

        protected static byte[] GetByteArray(string v) => System.Text.Encoding.UTF8.GetBytes(v);
    }
}
