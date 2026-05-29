# Password Generator CLI

A simple .NET command-line password generator. The app builds to a `pg` executable and can generate one or more passwords with configurable length and character sets.

## Features

- Generate passwords from the terminal
- Choose password length from 4 to 128 characters
- Include uppercase letters, lowercase letters, numbers, and special characters
- Generate up to 10 passwords at once
- Defaults to all character types when no character set is specified

## Requirements

- .NET SDK 10.0 or newer

## Build

From the repository root:

```powershell
dotnet build .\password-generator.slnx
```

## Run

Run the project directly with `dotnet run`:

```powershell
dotnet run --project .\password-generator -- -g
```

After building, you can also run the generated executable:

```powershell
.\password-generator\bin\Debug\net10.0\pg.exe -g
```

## Usage

```text
pg [COMMAND] [OPTIONS]
```

### Commands

| Command | Description |
| --- | --- |
| `-g`, `--generate` | Generate a new password |
| `-h`, `--help`, `-help` | Show help |

### Generate Options

| Option | Description |
| --- | --- |
| `-l`, `--length <num>` | Password length. Defaults to `12`. Values are clamped between `4` and `128`. |
| `-u`, `--uppercase` | Include uppercase letters, `A-Z`. |
| `-lw`, `--lowercase` | Include lowercase letters, `a-z`. |
| `-n`, `--numbers` | Include numbers, `0-9`. |
| `-s`, `--special` | Include special characters. |
| `-a`, `--all` | Include all character types. This is also the default when no character set is selected. |
| `-c`, `--count <num>` | Generate multiple passwords. Defaults to `1`. Values are clamped between `1` and `10`. |

## Examples

Generate a password with default settings:

```powershell
pg -g
```

Generate a 16-character password:

```powershell
pg -g -l 16
```

Generate a 20-character password with uppercase letters and numbers only:

```powershell
pg -g -u -n -l 20
```

Generate three 24-character passwords using all character types:

```powershell
pg -g --all --length 24 --count 3
```

## Publish

Create a release build:

```powershell
dotnet publish .\password-generator\password-generator.csproj -c Release
```

The executable is published under:

```text
password-generator\bin\Release\net10.0\publish\
```

To use `pg` from any terminal, add the publish directory to your `PATH`.
