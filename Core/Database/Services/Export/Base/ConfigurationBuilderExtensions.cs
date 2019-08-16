//------------------------------------------------------------------------------------------------- 
// <copyright file="ConfigurationBuilderExtensions.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the DomainTest type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.Services
{
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationBuilderExtensions
    {
        public static void AddCrossPlatform(this IConfigurationBuilder configurationBuilder, string path, string environmentName = null, bool skipDefault = false)
        {
            var platform = "other";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "osx";
            }

            if (!skipDefault)
            {
                configurationBuilder.AddJsonFile(Path.Combine(path, "appSettings.json"), true);
            }

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                configurationBuilder.AddJsonFile(Path.Combine(path, $"appSettings.{environmentName}.json"), true);
            }

            configurationBuilder.AddJsonFile(Path.Combine(path, $"appSettings.{platform}.json"), true);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                configurationBuilder.AddJsonFile(Path.Combine(path, $"appSettings.{environmentName}.{platform}.json"), true);
            }
        }
    }
}
