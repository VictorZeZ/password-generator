using System.Text;

namespace password_generator.Cli
{
    /// <summary>
    /// Banner art and small formatting helpers used by the classic,
    /// argument-driven CLI (help text, generate output, errors).
    /// </summary>
    static class ConsoleBanner
    {
        public static void Write()
        {
            Console.WriteLine();
            WriteGradientLine("██████╗   ██████╗ ");
            WriteGradientLine("██╔══██╗ ██╔════╝ ");
            WriteGradientLine("██████╔╝ ██║  ███╗");
            WriteGradientLine("██╔═══╝  ██║   ██║");
            WriteGradientLine("██║      ╚██████╔╝");
            WriteGradientLine("╚═╝       ╚═════╝ ");
            Console.WriteLine($"{AnsiTheme.Dim}Password Generator CLI{AnsiTheme.Reset} {AnsiTheme.Cyan}secure local password generation{AnsiTheme.Reset}");
            Console.WriteLine();
        }

        public static void WriteSection(string title)
        {
            Console.WriteLine($"{AnsiTheme.Pink}{AnsiTheme.Bold}{title}{AnsiTheme.Reset}");
        }

        public static void WriteOption(string option, string description)
        {
            Console.WriteLine($"  {AnsiTheme.Yellow}{option,-24}{AnsiTheme.Reset} {description}");
        }

        public static void WriteExample(string command, string description)
        {
            Console.WriteLine($"  {Command(command),-52} {AnsiTheme.Dim}{description}{AnsiTheme.Reset}");
        }

        public static string Command(string value) => $"{AnsiTheme.Green}{value}{AnsiTheme.Reset}";

        public static void WriteLineColor(string value, string color)
        {
            Console.WriteLine($"{color}{value}{AnsiTheme.Reset}");
        }

        /// <summary>
        /// Colors each character of a password by type (letters, numbers,
        /// symbols) for the classic CLI's single-shot terminal output.
        /// </summary>
        public static string HighlightPassword(string password)
        {
            StringBuilder output = new();

            foreach (char character in password)
            {
                string color = character switch
                {
                    >= 'A' and <= 'Z' => AnsiTheme.Cyan,
                    >= 'a' and <= 'z' => AnsiTheme.Green,
                    >= '0' and <= '9' => AnsiTheme.Yellow,
                    _ => AnsiTheme.Pink
                };

                output.Append(color).Append(character);
            }

            output.Append(AnsiTheme.Reset);
            return output.ToString();
        }

        static void WriteGradientLine(string text)
        {
            string[] colors = { AnsiTheme.Cyan, AnsiTheme.Blue, AnsiTheme.Purple, AnsiTheme.Pink };

            for (int i = 0; i < text.Length; i++)
            {
                Console.Write($"{colors[i * colors.Length / Math.Max(text.Length, 1)]}{text[i]}");
            }

            Console.WriteLine(AnsiTheme.Reset);
        }
    }
}