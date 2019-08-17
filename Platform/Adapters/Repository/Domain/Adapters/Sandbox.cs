// <copyright file="Sandbox.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("73970b0f-1ff4-4d39-aad8-fdbfbaae472f")]
    #endregion
    public partial class Sandbox : System.Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("0e0ee030-8fb5-42fb-82b5-5daade2aca9d")]
        [AssociationId("209d7177-b6bc-4a13-ae35-ea14e37da038")]
        [RoleId("f0032ab2-d086-4d52-aa30-e295d640ed90")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public Sandbox[] InvisibleManies { get; set; }

        #region Allors
        [Id("122b0376-8d1a-4d46-b8a0-9f4ea94c9e96")]
        [AssociationId("0d1e81f3-9025-42f3-b7a6-e2da3268667c")]
        [RoleId("d4ee88a8-5404-43ae-87b4-8d8755ebe2d2")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public Sandbox InvisibleOne { get; set; }

        #region Allors
        [Id("5eec5096-d8ba-424e-988f-b50828fc7b51")]
        [AssociationId("3db5ada7-52b4-47fa-bf55-d7641b1e9202")]
        [RoleId("3758b6fd-ab8f-43d5-ad3c-906a69682976")]
        [Size(256)]
        #endregion
        public string InvisibleValue { get; set; }
        #region Allors
        [Id("856a0161-2a46-428a-bae5-95d6a86a89e8")]
        [AssociationId("0c22274b-c5c3-4b6e-883a-e375f25fd500")]
        [RoleId("2f108584-41f9-48df-93f5-d442ce92a2a2")]
        [Size(256)]
        #endregion
        public string Test { get; set; }

        #region Allors
        [Id("a0dac9fc-2d19-429b-a522-46425a01ab78")]
        [AssociationId("62ea1fd2-c269-4145-8bfa-d4edffc7c17d")]
        [RoleId("ab3b47b0-682e-4b0e-95af-30de1537e453")]
        #endregion
        public int AllorsInteger { get; set; }

        #region Allors
        [Id("c82d1693-7b88-4fab-8389-a43185c832ed")]
        [AssociationId("38d87065-0b21-42c2-92b7-a095b54b83be")]
        [RoleId("d11ad34e-9079-4005-b803-90511894d73f")]
        [Size(256)]
        #endregion
        public string AllorsString { get; set; }

        #region Allors

        [Id("E551BDCA-9532-4024-B127-E971A5C1CDB2")]

        #endregion
        public void DoIt()
        {
        }

        #region inherited methods

        #endregion
    }
}
