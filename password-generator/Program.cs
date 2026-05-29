using System.Text;

namespace password_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse command line arguments
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

            Console.WriteLine("Unknown command. Use 'pg -help' for usage information.");
        }

        static void ShowHelp()
        {
            Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║                   PASSWORD GENERATOR CLI                      ║
╚═══════════════════════════════════════════════════════════════╝

USAGE:
  pg [COMMAND] [OPTIONS]

COMMANDS:
  -g, --generate     Generate a new password
  -h, --help, -help  Show this help message

OPTIONS (for -g command):
  -l, --length <num>     Password length (default: 12, min: 4, max: 128)
  -u, --uppercase        Include uppercase letters (A-Z)
  -lw, --lowercase       Include lowercase letters (a-z)
  -n, --numbers          Include numbers (0-9)
  -s, --special          Include special characters (!@#$%^&*)
  -a, --all              Include all character types (default behavior)
  -c, --count <num>      Generate multiple passwords (default: 1, max: 10)

EXAMPLES:
  pg -g                           Generate password with default settings
  pg -g -l 16                     Generate 16-character password
  pg -g -u -n -l 20              Generate 20-char password with uppercase & numbers only
  pg -g --all --length 24        Generate 24-char password with all character types
  pg -g -c 5                     Generate 5 different passwords
  pg -g --uppercase --numbers    Generate password with only uppercase and numbers

CHARACTER SETS:
  Uppercase:  A-Z
  Lowercase:  a-z  
  Numbers:    0-9
  Special:    !@#$%^&*()_+-=[]{}|;:,.<>?

EXAMPLES IN TERMINAL:
  > pg -g
  > pg -g -l 16 -u -lw -n -s
  > pg -g --length 20 --count 3
");
        }

        static void GeneratePasswordFromArgs(string[] args)
        {
            // Default values
            int length = 12;
            bool includeUppercase = false;
            bool includeLowercase = false;
            bool includeNumbers = false;
            bool includeSpecial = false;
            int count = 1;

            // Parse arguments
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

            // If no character types specified, use all
            if (!includeUppercase && !includeLowercase && !includeNumbers && !includeSpecial)
            {
                includeUppercase = includeLowercase = includeNumbers = includeSpecial = true;
            }

            // Generate password(s)
            Console.WriteLine($"\nGenerating {count} password(s) (Length: {length}):\n");

            for (int i = 0; i < count; i++)
            {
                string password = GeneratePassword(length, includeUppercase, includeLowercase, includeNumbers, includeSpecial);

                if (count > 1)
                    Console.WriteLine($"{i + 1}. {password}");
                else
                    Console.WriteLine(password);
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
            Random random = new Random();

            // Build character pool
            if (includeUppercase) validChars.Append(uppercase);
            if (includeLowercase) validChars.Append(lowercase);
            if (includeNumbers) validChars.Append(numbers);
            if (includeSpecial) validChars.Append(special);

            // Ensure at least one character from each selected category
            if (includeUppercase)
                password.Append(uppercase[random.Next(uppercase.Length)]);
            if (includeLowercase)
                password.Append(lowercase[random.Next(lowercase.Length)]);
            if (includeNumbers)
                password.Append(numbers[random.Next(numbers.Length)]);
            if (includeSpecial)
                password.Append(special[random.Next(special.Length)]);

            // Fill the rest
            for (int i = password.Length; i < length; i++)
            {
                password.Append(validChars[random.Next(validChars.Length)]);
            }

            return ShuffleString(password.ToString());
        }

        static string ShuffleString(string input)
        {
            char[] array = input.ToCharArray();
            Random random = new Random();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return new string(array);
        }
    }
}