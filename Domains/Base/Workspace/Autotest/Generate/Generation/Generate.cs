// ------------------------------------------------------------------------------------------------- 
// <copyright file="Generate.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Allors.Development.Repository.Tasks
{
    using System.IO;

    using Allors.Development.Repository.Generation;
    using Allors.Meta;

    using Autotest;

    public static class Generate
    {
        public static Log Execute(string template, string output)
        {
            var log = new GenerateLog();

            var templateFileInfo = new FileInfo(template);
            var stringTemplate = new StringTemplate(templateFileInfo);
            var outputDirectoryInfo = new DirectoryInfo(output);

            var model = new Model
            {
                MetaPopulation = MetaPopulation.Instance
            };

            model.LoadMetaExtensions(new FileInfo("./Workspace/Typescript/modules/Material/dist/autotest/meta.json"));
            model.LoadProject(new FileInfo("./Workspace/Autotest/Angular/dist/project.json"));
            model.LoadMenu(new FileInfo("./Workspace/Typescript/modules/Material/dist/autotest/menu.json"));

            stringTemplate.Generate(model, outputDirectoryInfo, log);

            return log;
        }
    }
}