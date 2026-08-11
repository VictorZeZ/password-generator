using System.Security.Cryptography;
using System.Text;

namespace password_generator.Core
{
    /// <summary>
    /// Core, UI-agnostic password generation logic shared by both the
    /// classic argument-driven CLI and the interactive menu.
    /// </summary>
    static class PasswordGenerator
    {
        const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string Numbers = "0123456789";
        const string Special = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        public static string Generate(int length, bool includeUppercase, bool includeLowercase,
                                       bool includeNumbers, bool includeSpecial)
        {
            StringBuilder validChars = new();
            StringBuilder password = new();

            if (includeUppercase) validChars.Append(Uppercase);
            if (includeLowercase) validChars.Append(Lowercase);
            if (includeNumbers) validChars.Append(Numbers);
            if (includeSpecial) validChars.Append(Special);

            if (includeUppercase)
                password.Append(Uppercase[NextIndex(Uppercase.Length)]);
            if (includeLowercase)
                password.Append(Lowercase[NextIndex(Lowercase.Length)]);
            if (includeNumbers)
                password.Append(Numbers[NextIndex(Numbers.Length)]);
            if (includeSpecial)
                password.Append(Special[NextIndex(Special.Length)]);

            for (int i = password.Length; i < length; i++)
            {
                password.Append(validChars[NextIndex(validChars.Length)]);
            }

            return Shuffle(password.ToString());
        }

        public static List<string> GenerateMany(int count, int length, bool includeUppercase,
                                                  bool includeLowercase, bool includeNumbers, bool includeSpecial)
        {
            var passwords = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                passwords.Add(Generate(length, includeUppercase, includeLowercase, includeNumbers, includeSpecial));
            }

            return passwords;
        }

        static string Shuffle(string input)
        {
            char[] array = input.ToCharArray();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = NextIndex(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return new string(array);
        }

        static int NextIndex(int maxExclusive) => RandomNumberGenerator.GetInt32(maxExclusive);
    }
}