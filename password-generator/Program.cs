using System.Security.Cryptography;
using System.Text;

namespace password_generator
{
    class Program
    {
        const string Reset = "\u001b[0m";
        const string Bold = "\u001b[1m";
        const string Dim = "\u001b[2m";
        const string Red = "\u001b[38;2;255;92;92m";
        const string Green = "\u001b[38;2;77;255;166m";
        const string Yellow = "\u001b[38;2;255;214;102m";
        const string Cyan = "\u001b[38;2;64;224;255m";
        const string Blue = "\u001b[38;2;112;168;255m";
        const string Purple = "\u001b[38;2;190;132;255m";
        const string Pink = "\u001b[38;2;255;92;184m";

        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-help" || args[0] == "--help" || args[0] == "-h")
            {
                ShowHelp();
                return;
            }

            if (args[0] == "-g" || args[0] == "--generate")
            {
                GeneratePasswordFromArgs(args);
                return;
            }

            WriteBanner();
            WriteLineColor("Unknown command.", Red);
            Console.WriteLine($"Use {Command("pg -help")} for usage information.");
        }

        static void ShowHelp()
        {
            WriteBanner();

            WriteSection("USAGE");
            Console.WriteLine($"  {Command("pg")} {Dim}[COMMAND] [OPTIONS]{Reset}");
            Console.WriteLine();

            WriteSection("COMMANDS");
            WriteOption("-g, --generate", "Generate a new password");
            WriteOption("-h, --help, -help", "Show this help message");
            Console.WriteLine();

            WriteSection("OPTIONS");
            WriteOption("-l, --length <num>", "Password length (default: 12, min: 4, max: 128)");
            WriteOption("-u, --uppercase", "Include uppercase letters (A-Z)");
            WriteOption("-lw, --lowercase", "Include lowercase letters (a-z)");
            WriteOption("-n, --numbers", "Include numbers (0-9)");
            WriteOption("-s, --special", "Include special characters (!@#$%^&*)");
            WriteOption("-a, --all", "Include all character types (default behavior)");
            WriteOption("-c, --count <num>", "Generate multiple passwords (default: 1, max: 10)");
            Console.WriteLine();

            WriteSection("EXAMPLES");
            WriteExample("pg -g", "Generate password with default settings");
            WriteExample("pg -g -l 16", "Generate a 16-character password");
            WriteExample("pg -g -u -n -l 20", "Generate uppercase and numbers only");
            WriteExample("pg -g --all --length 24 --count 3", "Generate three strong passwords");
            Console.WriteLine();

            WriteSection("CHARACTER SETS");
            WriteOption("Uppercase", "A-Z");
            WriteOption("Lowercase", "a-z");
            WriteOption("Numbers", "0-9");
            WriteOption("Special", "!@#$%^&*()_+-=[]{}|;:,.<>?");
        }

        static void GeneratePasswordFromArgs(string[] args)
        {
            int length = 12;
            bool includeUppercase = false;
            bool includeLowercase = false;
            bool includeNumbers = false;
            bool includeSpecial = false;
            int count = 1;

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
                            count = Math.Clamp(cnt, 1, 10);
                            i++;
                        }
                        break;
                }
            }

            if (!includeUppercase && !includeLowercase && !includeNumbers && !includeSpecial)
            {
                includeUppercase = includeLowercase = includeNumbers = includeSpecial = true;
            }

            WriteBanner();
            Console.WriteLine($"{Dim}Generating {Reset}{Bold}{count}{Reset}{Dim} password(s), length {Reset}{Bold}{length}{Reset}{Dim}.{Reset}");
            Console.WriteLine();

            for (int i = 0; i < count; i++)
            {
                string password = GeneratePassword(length, includeUppercase, includeLowercase, includeNumbers, includeSpecial);

                if (count > 1)
                    Console.WriteLine($"{Dim}{i + 1,2}.{Reset} {HighlightPassword(password)}");
                else
                    Console.WriteLine(HighlightPassword(password));
            }

            Console.WriteLine();
        }

        static string GeneratePassword(int length, bool includeUppercase, bool includeLowercase,
                                      bool includeNumbers, bool includeSpecial)
        {
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            StringBuilder validChars = new StringBuilder();
            StringBuilder password = new StringBuilder();

            if (includeUppercase) validChars.Append(uppercase);
            if (includeLowercase) validChars.Append(lowercase);
            if (includeNumbers) validChars.Append(numbers);
            if (includeSpecial) validChars.Append(special);

            if (includeUppercase)
                password.Append(uppercase[NextIndex(uppercase.Length)]);
            if (includeLowercase)
                password.Append(lowercase[NextIndex(lowercase.Length)]);
            if (includeNumbers)
                password.Append(numbers[NextIndex(numbers.Length)]);
            if (includeSpecial)
                password.Append(special[NextIndex(special.Length)]);

            for (int i = password.Length; i < length; i++)
            {
                password.Append(validChars[NextIndex(validChars.Length)]);
            }

            return ShuffleString(password.ToString());
        }

        static string ShuffleString(string input)
        {
            char[] array = input.ToCharArray();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = NextIndex(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return new string(array);
        }

        static int NextIndex(int maxExclusive)
        {
            return RandomNumberGenerator.GetInt32(maxExclusive);
        }

        static void WriteBanner()
        {
            Console.WriteLine();
            WriteGradientLine("██████╗   ██████╗ ");
            WriteGradientLine("██╔══██╗ ██╔════╝ ");
            WriteGradientLine("██████╔╝ ██║  ███╗");
            WriteGradientLine("██╔═══╝  ██║   ██║");
            WriteGradientLine("██║      ╚██████╔╝");
            WriteGradientLine("╚═╝       ╚═════╝ ");
            Console.WriteLine($"{Dim}Password Generator CLI{Reset} {Cyan}secure local password generation{Reset}");
            Console.WriteLine();
        }

        static void WriteGradientLine(string text)
        {
            string[] colors = { Cyan, Blue, Purple, Pink };

            for (int i = 0; i < text.Length; i++)
            {
                Console.Write($"{colors[i * colors.Length / Math.Max(text.Length, 1)]}{text[i]}");
            }

            Console.WriteLine(Reset);
        }

        static void WriteSection(string title)
        {
            Console.WriteLine($"{Pink}{Bold}{title}{Reset}");
        }

        static void WriteOption(string option, string description)
        {
            Console.WriteLine($"  {Yellow}{option,-24}{Reset} {description}");
        }

        static void WriteExample(string command, string description)
        {
            Console.WriteLine($"  {Command(command),-52} {Dim}{description}{Reset}");
        }

        static string Command(string value)
        {
            return $"{Green}{value}{Reset}";
        }

        static void WriteLineColor(string value, string color)
        {
            Console.WriteLine($"{color}{value}{Reset}");
        }

        static string HighlightPassword(string password)
        {
            StringBuilder output = new StringBuilder();

            foreach (char character in password)
            {
                string color = character switch
                {
                    >= 'A' and <= 'Z' => Cyan,
                    >= 'a' and <= 'z' => Green,
                    >= '0' and <= '9' => Yellow,
                    _ => Pink
                };

                output.Append(color);
                output.Append(character);
            }

            output.Append(Reset);
            return output.ToString();
        }
    }
}
