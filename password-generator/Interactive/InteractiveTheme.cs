using Spectre.Console;

namespace password_generator.Interactive
{
    /// <summary>
    /// Colors and labels used by the interactive, menu-driven experience.
    /// Kept separate from Cli/AnsiTheme.cs, which styles the classic CLI.
    /// </summary>
    static class InteractiveTheme
    {
        // Idle (not currently selected) passwords in the browse list.
        public const string PasswordColor = "#8FD6FF"; // light blue

        // The currently selected password in the browse list.
        public static readonly Style SelectedStyle = new(foreground: Color.Green, decoration: Decoration.Bold);

        public const string UppercaseLabel = "Uppercase letters (A-Z)";
        public const string LowercaseLabel = "Lowercase letters (a-z)";
        public const string NumbersLabel = "Numbers (0-9)";
        public const string SpecialLabel = "Special characters (!@#$%^&*)";
        public const string ExitLabel = "Exit";
    }
}