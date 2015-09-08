// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Goods.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
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
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        public void ImportPhotos(DirectoryInfo directoryInfo)
        {
            var goodBySku = new Dictionary<string, Good>();

            foreach (Good good in new Goods(this.Session).Extent())
            {
                if (good.ExistSku)
                {
                    goodBySku.Add(good.Sku, good);
                }
            }

            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                var sku = System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name);
                Good good;
                if (goodBySku.TryGetValue(sku, out good))
                {
                    if (!good.ExistPhoto)
                    {
                        good.Photo = new MediaBuilder(this.Session).Build();
                    }

                    try
                    {
                        good.Photo.Content = File.ReadAllBytes(fileInfo.FullName);
                    }
                    catch (Exception e)
                    {
                        Console.Write(fileInfo.FullName + @" " + e.Message);
                        good.Photo.Delete();
                    }
                }
            }
        }
    }
}