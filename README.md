<div align="center">
  <img src="./assets/br-logo-ascii.png?raw=true" alt="ByReplace Logo">
</div>

#

[![Contributors](https://img.shields.io/github/contributors/Daniel-iel/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![Activity](https://img.shields.io/github/commit-activity/m/Daniel-iel/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![CI](https://github.com/Daniel-iel/ByReplace/actions/workflows/ci.yml/badge.svg)](https://github.com/Daniel-iel/ByReplace/actions/workflows/ci.yml/badge.svg/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![Downloads](https://img.shields.io/nuget/dt/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![Release](https://img.shields.io/nuget/v/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![Repo Size](https://img.shields.io/github/repo-size/Daniel-iel/ByReplace)](https://www.nuget.org/packages/ByReplace/)

# About

**ByReplace** is a powerful command-line interface (CLI) tool designed for efficiently executing find-and-replace operations within source files. This versatile utility enables users to seamlessly modify text within their codebase, enhancing productivity and streamlining development processes. Whether you're updating variable names, correcting typos, or implementing sweeping changes across multiple files, ByReplace has you covered.

# Get Started

To run ByReplace, you need [Dotnet 8](https://dotnet.microsoft.com/en-us/download). After installing the NuGet package, navigate to your terminal and execute the command `br -v`. If the installation was successful, the following text will be displayed in your terminal:

```shell
br

Commands:
  apply    Apply commands
  rule     Rule commands

Options:
  -h, --help    Show help message
  --version     Show version
```

# Installation

ByReplace is delivered via NuGet.

```shell
dotnet install -g ByReplace
```

# Creating the Configuration File

Create a configuration file with the following structure:

```shell
{
  "Path": "",
  "SkipDirectories": [""],
  "Rules": [
    {
      "Name": "",
      "Description": "",
      "Skip": ["", ""],
      "Extensions": ["", ""],
      "Replacement": {
        "Old": ["", ""],
        "New": ""
      }
    }
  ]
}
```

# JSON Configuration Documentation

This document provides a detailed explanation of the structure and purpose of each field within the provided JSON configuration file.

## Path
- Type: String
- Purpose: Specifies the path where the operation should be performed. This can be a directory path or a file path depending on the context of the operation.
- Example: "C:\Users\User\Documents"

## SkipDirectories
- Type: Array of Strings
- Purpose: Lists directories that should be skipped during the operation. Each entry in the array represents a directory path.
- Example: ["Temp", "Logs"]

## Rules
- Type: Array of Objects
- Purpose: Defines a set of rules to apply during the operation. Each rule object contains specific configurations for how files or directories should be handled.

## Name
- Type: String
- Purpose: A descriptive name for the rule. Used for identification purposes.
- Example: "BackupRule"

## Description
- Type: String
- Purpose: Provides a brief description of what the rule does.
- Example: "Moves all .txt files to the backup folder."

## Skip
- Type: Array of Strings
- Purpose: Lists specific files or patterns of files to skip when applying this rule. Useful for excluding certain types of files from being processed.
- Example: [".tmp", "~"]

## Extensions
- Type: Array of Strings
- Purpose: Specifies the file extensions that this rule applies to. Only files with these extensions will be considered for processing according to this rule.
- Example: [".docx", ".pdf"]

## Replacement
- Type: Object
- Purpose: Contains configurations for replacing parts of the file paths or names as per the rule's requirements.

### Old
- Type: Array of Strings
- Purpose: Lists the old strings or patterns that should be replaced in the file paths or names.
- Example: ["temp", "backup"]

### New
- Type: String
- Purpose: The new string or pattern that replaces the old ones specified in the Old array.
- Example: "archive"

# Commands

## Apply

`apply rule`: Executes a specific rule from the configuration file.

- `-r`: Rule's name.
- `-p`: Path of files to execute the apply.
- `-f`: Folder's path that contains the configuration file.

```bash
br apply rule -r "Rule" -p "C:/three_files" -f "C:/"
```

`apply rules`: Executes all rules from the configuration file.
- `-p`: Path of files to execute the apply.
- `-f`: Folder's path that contains the configuration file.

```bash
br apply rules -p "C:/three_files" -f "C:/"
```

## Rule

`rule list-rules`: Lists all rules from the configuration file.

- `-f`: Folder's path that contains the configuration file.

```bash
br rule list-rules -f "C:/"
```

`rule open-rule`: Gets a rule by its name.

- `-n`: Rule's name.
- `-f`: Folder's path that contains the configuration file.

```bash
br rule open-rule -n "rule name" -f "C:/"
```