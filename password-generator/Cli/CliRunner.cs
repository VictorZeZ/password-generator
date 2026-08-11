using password_generator.Core;

namespace password_generator.Cli
{
    /// <summary>
    /// The original flag-based CLI (`pg -g -l 16 ...`), kept for scripting
    /// and automation alongside the interactive menu.
    /// </summary>
    static class CliRunner
    {
        public static void ShowHelp()
        {
            ConsoleBanner.Write();

            ConsoleBanner.WriteSection("USAGE");
            Console.WriteLine($"  {ConsoleBanner.Command("pg")} {AnsiTheme.Dim}[COMMAND] [OPTIONS]{AnsiTheme.Reset}");
            Console.WriteLine();

            ConsoleBanner.WriteSection("COMMANDS");
            ConsoleBanner.WriteOption("(no arguments)", "Launch the interactive, menu-driven experience");
            ConsoleBanner.WriteOption("-i, --interactive", "Launch the interactive experience explicitly");
            ConsoleBanner.WriteOption("-g, --generate", "Generate a new password non-interactively");
            ConsoleBanner.WriteOption("-h, --help, -help", "Show this help message");
            Console.WriteLine();

            ConsoleBanner.WriteSection("OPTIONS (used with -g / --generate)");
            ConsoleBanner.WriteOption("-l, --length <num>", "Password length (default: 12, min: 4, max: 128)");
            ConsoleBanner.WriteOption("-u, --uppercase", "Include uppercase letters (A-Z)");
            ConsoleBanner.WriteOption("-lw, --lowercase", "Include lowercase letters (a-z)");
            ConsoleBanner.WriteOption("-n, --numbers", "Include numbers (0-9)");
            ConsoleBanner.WriteOption("-s, --special", "Include special characters (!@#$%^&*)");
            ConsoleBanner.WriteOption("-a, --all", "Include all character types (default behavior)");
            ConsoleBanner.WriteOption("-c, --count <num>", "Generate multiple passwords (default: 1, max: 32)");
            Console.WriteLine();

            ConsoleBanner.WriteSection("EXAMPLES");
            Console.WriteLine($"  {ConsoleBanner.Command("pg"),-52} {AnsiTheme.Dim}Launch the interactive experience{AnsiTheme.Reset}");
            ConsoleBanner.WriteExample("pg -g", "Generate password with default settings");
            ConsoleBanner.WriteExample("pg -g -l 16", "Generate a 16-character password");
            ConsoleBanner.WriteExample("pg -g -u -n -l 20", "Generate uppercase and numbers only");
            ConsoleBanner.WriteExample("pg -g --all --length 24 --count 3", "Generate three strong passwords");
            Console.WriteLine();

            ConsoleBanner.WriteSection("CHARACTER SETS");
            ConsoleBanner.WriteOption("Uppercase", "A-Z");
            ConsoleBanner.WriteOption("Lowercase", "a-z");
            ConsoleBanner.WriteOption("Numbers", "0-9");
            ConsoleBanner.WriteOption("Special", "!@#$%^&*()_+-=[]{}|;:,.<>?");
        }

        public static void GenerateFromArgs(string[] args)
        {
            int length = 12;
            bool includeUppercase = false;
            bool includeLowercase = false;
            bool includeNumbers = false;
            bool includeSpecial = false;
            int count = 1;
            bool countClamped = false;
            int requestedCount = 0;

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-l":
                    case "--length":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int len))
                        {
                            length = Math.Clamp(len, 4, 128);
                            i++;
                        }
                        break;
                    case "-u":
                    case "--uppercase":
                        includeUppercase = true;
                        break;
                    case "-lw":
                    case "--lowercase":
                        includeLowercase = true;
                        break;
                    case "-n":
                    case "--numbers":
                        includeNumbers = true;
                        break;
                    case "-s":
                    case "--special":
                        includeSpecial = true;
                        break;
                    case "-a":
                    case "--all":
                        includeUppercase = includeLowercase = includeNumbers = includeSpecial = true;
                        break;
                    case "-c":
                    case "--count":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int cnt))
                        {
                            requestedCount = cnt;
                            if (cnt > 32) countClamped = true;
                            count = Math.Clamp(cnt, 1, 32);
                            i++;
                        }
                        break;
                }
            }

            if (!includeUppercase && !includeLowercase && !includeNumbers && !includeSpecial)
            {
                includeUppercase = includeLowercase = includeNumbers = includeSpecial = true;
            }

            ConsoleBanner.Write();
            if (countClamped)
            {
                ConsoleBanner.WriteLineColor($"Maximum allowed count is 32. Generating 32 passwords instead of {requestedCount}.", AnsiTheme.Yellow);
            }
            Console.WriteLine($"{AnsiTheme.Dim}Generating {AnsiTheme.Reset}{AnsiTheme.Bold}{count}{AnsiTheme.Reset}{AnsiTheme.Dim} password(s), length {AnsiTheme.Reset}{AnsiTheme.Bold}{length}{AnsiTheme.Reset}{AnsiTheme.Dim}.{AnsiTheme.Reset}");
            Console.WriteLine();

            var passwords = PasswordGenerator.GenerateMany(count, length, includeUppercase, includeLowercase, includeNumbers, includeSpecial);

            for (int i = 0; i < passwords.Count; i++)
            {
                if (passwords.Count > 1)
                    Console.WriteLine($"{AnsiTheme.Dim}{i + 1,2}.{AnsiTheme.Reset} {ConsoleBanner.HighlightPassword(passwords[i])}");
                else
                    Console.WriteLine(ConsoleBanner.HighlightPassword(passwords[i]));
            }

            Console.WriteLine();
        }
    }
}