// <copyright file="FileReader.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.IO;

    public static class FileReader
    {
        public static Media CreateMedia(ISession session, string fileName)
        {
            if (File.Exists(fileName))
            {
                var fileInfo = new FileInfo(fileName);

                var name = Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                return new MediaBuilder(session).WithFileName(name).WithInData(content).Build();
            }

            return null;
        }
    }
}
