namespace password_generator.Cli
{
    /// <summary>
    /// Raw ANSI escape codes used by the classic (non-interactive) CLI output.
    /// The interactive menu uses Spectre.Console markup instead — see
    /// Interactive/InteractiveTheme.cs.
    /// </summary>
    static class AnsiTheme
    {
        public const string Reset = "\u001b[0m";
        public const string Bold = "\u001b[1m";
        public const string Dim = "\u001b[2m";
        public const string Red = "\u001b[38;2;255;92;92m";
        public const string Green = "\u001b[38;2;77;255;166m";
        public const string Yellow = "\u001b[38;2;255;214;102m";
        public const string Cyan = "\u001b[38;2;64;224;255m";
        public const string Blue = "\u001b[38;2;112;168;255m";
        public const string Purple = "\u001b[38;2;190;132;255m";
        public const string Pink = "\u001b[38;2;255;92;184m";
    }
}