# Password Generator

`pg` is a Windows command-line password generator written in .NET. It generates passwords locally, supports several character-set options, and now ships with a graphical MSI installer.

## What It Does

- Generate one or more passwords from the terminal
- Choose a password length from 4 to 128 characters
- Include uppercase letters, lowercase letters, numbers, and special characters
- Generate up to 10 passwords at once
- Show colorized terminal output and a simple banner

## Requirements

- Windows
- .NET SDK 10.0 or newer for development and local builds

## Run From Source

From the repository root:

```powershell
dotnet run --project .\password-generator -- -g
```

Show help:

```powershell
dotnet run --project .\password-generator -- -h
```

## CLI Usage

```text
pg [COMMAND] [OPTIONS]
```

### Commands

| Command | Description |
| --- | --- |
| `-g`, `--generate` | Generate passwords |
| `-h`, `--help`, `-help` | Show help |

### Options

| Option | Description |
| --- | --- |
| `-l`, `--length <num>` | Password length. Defaults to `12` and is clamped between `4` and `128`. |
| `-u`, `--uppercase` | Include uppercase letters `A-Z`. |
| `-lw`, `--lowercase` | Include lowercase letters `a-z`. |
| `-n`, `--numbers` | Include numbers `0-9`. |
| `-s`, `--special` | Include special characters. |
| `-a`, `--all` | Include all character types. This is also the default when no set is chosen. |
| `-c`, `--count <num>` | Generate multiple passwords. Defaults to `1` and is clamped between `1` and `10`. |

### Examples

```powershell
pg -g
pg -g -l 16
pg -g -u -n -l 20
pg -g --all --length 24 --count 3
```

## Windows Installer

Download the latest `pg-setup-*.msi` from GitHub Releases, run it, and follow the setup wizard. The installer places `pg.exe` in `C:\Program Files\Password Generator\` and adds that folder to `PATH`.

After installation, open a new PowerShell or Command Prompt window and run:

```powershell
pg -g
```

## Build The Installer

To build the MSI locally:

```powershell
.\scripts\build-installer.ps1
```

This publishes a self-contained `pg.exe` and creates:

```text
artifacts\installer\pg-setup-1.0.2.msi
```

## Release Flow

The repository includes a GitHub Actions workflow that builds the MSI when you push a `v*` tag.

```powershell
git tag v1.0.2
git push origin v1.0.2
```

The release artifact is attached to GitHub Releases for users to download.

## Project Layout

```text
password-generator/        App source
installer/                 WiX MSI project and license text
scripts/                   Build helper scripts
.github/workflows/         GitHub release build workflow
```

