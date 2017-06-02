// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Console
{
    using System;
    using System.CommandLine;

    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                Commands? command = null;
                var file = "population.xml";

                ArgumentSyntax.Parse(
                    args,
                    syntax =>
                        {
                            syntax.DefineCommand("populate", ref command, Commands.Populate, "Populate the database");

                            syntax.DefineCommand("save", ref command, Commands.Save, "Load the database");
                            syntax.DefineOption("f|file", ref file, "The source xml file.");

                            syntax.DefineCommand("load", ref command, Commands.Load, "Save the database");
                            syntax.DefineOption("f|file", ref file, "The destination xml file.");

                            syntax.DefineCommand("upgrade", ref command, Commands.Upgrade, "Upgrade the database");
                            syntax.DefineOption("f|file", ref file, "The destination xml file.");

                            syntax.DefineCommand("import", ref command, Commands.Import, "Import from external sources");

                            syntax.DefineCommand("custom", ref command, Commands.Custom, "Custom code");
                        });


                switch (command)
                {
                    case Commands.Populate:
                        new Populate().Execute();
                        break;

                    case Commands.Save:
                        new Save(file).Execute();
                        break;

                    case Commands.Load:
                        new Load(file).Execute();
                        break;

                    case Commands.Upgrade:
                        new Upgrade(file).Execute();
                        break;

                    case Commands.Custom:
                        new Custom().Execute();
                        break;
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
        }
    }
}