//------------------------------------------------------------------------------------------------- 
// <copyright file="ConfigurationBuilderExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
