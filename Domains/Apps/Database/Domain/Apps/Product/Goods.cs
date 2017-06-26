// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Goods.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public partial class Goods
    {
        public void ImportPhotos(DirectoryInfo directoryInfo)
        {
            var goodByArticleNumber = new Dictionary<string, Good>();

            foreach (Good good in new Goods(this.Session).Extent())
            {
                if (good.ExistArticleNumber)
                {
                    goodByArticleNumber.Add(good.ArticleNumber, good);
                }
            }

            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                var articleNumber = Path.GetFileNameWithoutExtension(fileInfo.Name);
                Good good;
                if (goodByArticleNumber.TryGetValue(articleNumber, out good))
                {
                    var fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                    var content = File.ReadAllBytes(fileInfo.FullName);
                    var image = new MediaBuilder(this.Session).WithFileName(fileName).WithInData(content).Build();
                    good.AddPhoto(image);
                }
            }
        }
    }
}