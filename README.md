<div align="center">
  <img  src="./assets/br-logo-ascii.png?raw=true">
</div>

#

[![Contributors](https://img.shields.io/github/contributors/Daniel-iel/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![Activity](https://img.shields.io/github/commit-activity/m/Daniel-iel/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![ci](https://github.com/Daniel-iel/ByReplace/actions/workflows/ci.yml/badge.svg)](https://github.com/Daniel-iel/ByReplace/actions/workflows/ci.yml/badge.svg/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![Downloads](https://img.shields.io/nuget/dt/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![Release](https://img.shields.io/nuget/v/ByReplace)](https://www.nuget.org/packages/ByReplace/)
[![Repo Size](https://img.shields.io/github/repo-size/Daniel-iel/ByReplace)](https://www.nuget.org/packages/ByReplace/)

# About

ByReplace is a powerful command-line interface (CLI) tool designed for efficiently executing find-and-replace operations within source files. This versatile utility enables users to seamlessly modify text within their codebase, enhancing productivity and streamlining development processes. Whether you're updating variable names, correcting typos, or implementing sweeping changes across multiple files.

# Get Started

[Dotnet 8](https://dotnet.microsoft.com/en-us/download) is required to run ByReplace.
After installing the NuGet package, navigate to your terminal and execute the command `br -v`. If the installation was successful, the following text will be displayed in your terminal.

```shell
br

Commands:
  apply    apply commands
  rule     rule commands

Options:
  -h, --help    Show help message
  --version     Show version
```

## Install

ByReplace tool is delivery by [nuget](https://www.nuget.org/packages/ByReplace).

```shell
dotnet install -g ByReplace
```

## How Create the Configuration File

```shell
{
  "Path": "",
  "SkipDirectories": [ "" ],
  "Rules": [
    {
      "Name": "",
      "Description": "",
      "Skip": [ "", "" ],
      "Extensions": [ "", "" ],
      "Replacement": {
        "Old": [ "", "" ],
        "New": ""
      }
    }
  ]
}
```

### Path

### SkipDirectories

### Rules

## Commands

### Apply

`br apply rule`: Is used to execute only one rule from brConfiguration file. 

- `-r`: rule's name.
- `-p`: path of files to execute the apply.
- `-f`: folder's path that constains the brConfiguration file.

```shell
br apply rule -r "Rule" -p "C:/{three files" -f "C:"
```

`br apply rules`: Is used to execute all rules rule from brConfiguration file. 

- `-p`: path of files to execute the apply.
- `-f`: folder's path that constains the brConfiguration file.

```shell
br apply rules -p "C:/{three files" -f "C:"
```

### Rule

`br rule list-rules` 

```shell
```

`br rule open-rule` 

```shell
```
