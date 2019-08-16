// <copyright file="MediaController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
