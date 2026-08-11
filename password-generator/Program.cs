using password_generator.Cli;
using password_generator.Interactive;

namespace password_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Typing "pg" with no arguments launches the fully interactive experience.
            if (args.Length == 0)
            {
                InteractiveRunner.Run();
                return;
            }

            if (args[0] == "-help" || args[0] == "--help" || args[0] == "-h")
            {
                CliRunner.ShowHelp();
                return;
            }

            if (args[0] == "-i" || args[0] == "--interactive")
            {
                InteractiveRunner.Run();
                return;
            }

            if (args[0] == "-g" || args[0] == "--generate")
            {
                CliRunner.GenerateFromArgs(args);
                return;
            }

            ConsoleBanner.Write();
            ConsoleBanner.WriteLineColor("Unknown command.", AnsiTheme.Red);
            Console.WriteLine($"Use {ConsoleBanner.Command("pg -help")} for usage information.");
        }
    }
}