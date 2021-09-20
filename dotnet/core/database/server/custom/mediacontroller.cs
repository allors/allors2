// <copyright file="MediaController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using Allors.Services;

    public class MediaController : CoreMediaController
    {
        public MediaController(ISessionService sessionService)
            : base(sessionService)
        {
        }
    }
}
