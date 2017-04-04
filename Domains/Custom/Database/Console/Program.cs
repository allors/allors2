namespace Allors
{
    using System;

    public class Program
    {
        public static int Main(string[] args)
        {
            var interactive = args.Length == 0;

            if (interactive)
            {
                Console.WriteLine("Please select an option:\n");
                foreach (var option in Enum.GetValues(typeof(Options)))
                {
                    Console.WriteLine((int)option + ". " + Enum.GetName(typeof(Options), option));
                }

                Console.WriteLine();
            }

            try
            {
                var choice = interactive ? Console.ReadLine() : args[0];
                Options option;
                if (Enum.TryParse(choice, out option))
                {
                    if (interactive)
                    {
                        Console.WriteLine("-> " + (int)option + ". " + Enum.GetName(typeof(Options), option));
                        Console.WriteLine();
                    }

                    Command command;

                    switch (option)
                    {
                        case Options.Save:
                            command = new Commands.Save();
                            break;

                        case Options.Load:
                            command = new Commands.Load();
                            break;

                        case Options.Upgrade:
                            command = new Commands.Upgrade();
                            break;
                            
                        case Options.Populate:
                            command = new Commands.Populate();
                            break;

                        case Options.Demo:
                            command = new Commands.Demo();
                            break;

                        case Options.Report:
                            command = new Commands.Report();
                            break;

                        case Options.Investigate:
                            command = new Commands.Investigate();
                            break;

                        case Options.Exit:
                            return 0;

                        default:
                            throw new ArgumentException("Non supported option");
                    }

                    command.Execute();
                }
                else
                {
                    throw new ArgumentException("Unknown option");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (1);
            }
            finally
            {
                if (interactive)
                {
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey(false);
                }
            }

            return 0;
        }
    }
}
