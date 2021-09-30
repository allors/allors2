// <copyright file="FileReader.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.IO;

    public static class FileInfoExtensions
    {
        public static Media CreateMedia(this FileInfo fileInfo, ISession session)
        {
            fileInfo.Refresh();
            var content = File.ReadAllBytes(fileInfo.FullName);
            return new MediaBuilder(session).WithInFileName(fileInfo.FullName).WithInData(content).Build();
        }
    }
}
