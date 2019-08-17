// <copyright file="GenerateLog.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Allors.Development.Repository.Tasks
{
    using System;
    using Antlr4.StringTemplate.Misc;

    internal class GenerateLog : Log
    {
        public GenerateLog() => this.ErrorOccured = false;

        public override void Error(object sender, string message)
        {
            this.ErrorOccured = true;

            Console.WriteLine(message);

            if (sender is TemplateRuntimeMessage templateRuntimeMessage)
            {
                var frame = templateRuntimeMessage.Frame;
                var template = frame.Template;

                foreach (var attribute in template.GetAttributes())
                {
                    Console.WriteLine($"{attribute.Key}: {attribute.Value}");
                }
            }

            Console.WriteLine();
        }
    }
}
