using password_generator.Core;
using Spectre.Console;
using TextCopy;

namespace password_generator.Interactive
{
    /// <summary>
    /// The interactive experience shown when `pg` is run with no arguments:
    /// prompts for length, character types, and count, then lets the user
    /// browse the generated passwords and copy one to the clipboard.
    /// </summary>
    static class InteractiveRunner
    {
        public static void Run()
        {
            AnsiConsole.Clear();
            ShowWelcome();

            int length = PromptForLength();
            var (upper, lower, numbers, special) = PromptForCharacterTypes();
            int count = PromptForCount();

            var passwords = GenerateWithSpinner(count, length, upper, lower, numbers, special);

            AnsiConsole.WriteLine();
            BrowsePasswords(passwords);
        }

        static void ShowWelcome()
        {
            AnsiConsole.Write(
                new FigletText("pg")
                    .Centered()
                    .Color(Color.Cyan1));

            AnsiConsole.Write(new Rule("[grey]Password Generator[/]").RuleStyle("grey").Centered());
            AnsiConsole.MarkupLine("[dim]Secure, local password generation. Press[/] [green]Enter[/] [dim]at any prompt to accept the default value.[/]");
            AnsiConsole.WriteLine();
        }

        static int PromptForLength()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>("[bold]Password length[/] [grey](4-32, default 12, press Enter to accept):[/]")
                    .DefaultValue(12)
                    .PromptStyle("cyan")
                    .ValidationErrorMessage("[red]Enter a whole number between 4 and 32.[/]")
                    .Validate(len => len is >= 4 and <= 32
                        ? ValidationResult.Success()
                        : ValidationResult.Error()));
        }

        static (bool Upper, bool Lower, bool Numbers, bool Special) PromptForCharacterTypes()
        {
            var selected = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("[bold]Character types[/] [grey](space to toggle, enter to confirm, default: all)[/]")
                    .NotRequired()
                    .PageSize(10)
                    .HighlightStyle("cyan")
                    .InstructionsText("[grey](Press [blue]<space>[/] to toggle a type, [green]<enter>[/] to accept)[/]")
                    .AddChoices(
                        InteractiveTheme.UppercaseLabel,
                        InteractiveTheme.LowercaseLabel,
                        InteractiveTheme.NumbersLabel,
                        InteractiveTheme.SpecialLabel)
                    .Select(InteractiveTheme.UppercaseLabel)
                    .Select(InteractiveTheme.LowercaseLabel)
                    .Select(InteractiveTheme.NumbersLabel)
                    .Select(InteractiveTheme.SpecialLabel));

            bool upper = selected.Contains(InteractiveTheme.UppercaseLabel);
            bool lower = selected.Contains(InteractiveTheme.LowercaseLabel);
            bool numbers = selected.Contains(InteractiveTheme.NumbersLabel);
            bool special = selected.Contains(InteractiveTheme.SpecialLabel);

            if (!upper && !lower && !numbers && !special)
            {
                AnsiConsole.MarkupLine("[yellow]No character types selected. Using all character types.[/]");
                upper = lower = numbers = special = true;
            }

            return (upper, lower, numbers, special);
        }

        static int PromptForCount()
        {
            int requested = AnsiConsole.Prompt(
                new TextPrompt<int>("[bold]How many passwords?[/] [grey](1-32, default 1, press Enter to accept):[/]")
                    .DefaultValue(1)
                    .PromptStyle("cyan")
                    .ValidationErrorMessage("[red]Enter a whole number of at least 1.[/]")
                    .Validate(value => value >= 1
                        ? ValidationResult.Success()
                        : ValidationResult.Error()));

            if (requested > 32)
            {
                AnsiConsole.MarkupLine($"[yellow]Maximum allowed count is 32. Generating 32 passwords instead of {requested}.[/]");
            }

            return Math.Clamp(requested, 1, 32);
        }

        static List<string> GenerateWithSpinner(int count, int length, bool upper, bool lower, bool numbers, bool special)
        {
            List<string> passwords = new(count);

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse("cyan"))
                .Start("Generating passwords...", _ =>
                {
                    passwords.AddRange(PasswordGenerator.GenerateMany(count, length, upper, lower, numbers, special));
                });

            return passwords;
        }

        /// <summary>
        /// Shows the generated passwords in a keyboard-navigable list.
        /// Passwords render in a single idle color; the item currently
        /// under the cursor is highlighted in green. Pressing Enter on a
        /// password copies it to the clipboard.
        /// </summary>
        static void BrowsePasswords(List<string> passwords)
        {
            var displayToPassword = new Dictionary<string, string>();
            var choices = new List<string>();

            for (int i = 0; i < passwords.Count; i++)
            {
                string label = passwords.Count > 1
                    ? $"{i + 1,2}. {FormatPasswordForList(passwords[i])}"
                    : FormatPasswordForList(passwords[i]);

                // Guard against duplicate labels (Spectre choices must be unique strings).
                while (displayToPassword.ContainsKey(label))
                {
                    label += " ";
                }

                displayToPassword[label] = passwords[i];
                choices.Add(label);
            }

            string exitChoice = $"[grey]{InteractiveTheme.ExitLabel}[/]";
            choices.Add(exitChoice);

            while (true)
            {
                AnsiConsole.WriteLine();

                string selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold]Generated passwords[/] [grey](Up/Down to browse, Enter to copy to clipboard)[/]")
                        .PageSize(15)
                        .HighlightStyle(InteractiveTheme.SelectedStyle)
                        .MoreChoicesText("[grey](Move up/down to see more passwords)[/]")
                        .AddChoices(choices));

                if (selected == exitChoice)
                {
                    AnsiConsole.MarkupLine("[grey]Goodbye![/]");
                    break;
                }

                string rawPassword = displayToPassword[selected];
                ClipboardService.SetText(rawPassword);
                AnsiConsole.MarkupLine("[green]✓ Copied to clipboard.[/]");
            }
        }

        static string FormatPasswordForList(string password) =>
            $"[{InteractiveTheme.PasswordColor}]{Markup.Escape(password)}[/]";
    }
}