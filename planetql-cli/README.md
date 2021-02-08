# planetql

analyze vulnerabilities in c# project

[![oclif](https://img.shields.io/badge/cli-oclif-brightgreen.svg)](https://oclif.io)
[![Version](https://img.shields.io/npm/v/planetql.svg)](https://npmjs.org/package/planetql)
[![Downloads/week](https://img.shields.io/npm/dw/planetql.svg)](https://npmjs.org/package/planetql)
[![License](https://img.shields.io/npm/l/planetql.svg)](https://github.com/soohoio/planetql/blob/master/package.json)

<!-- toc -->
* [planetql](#planetql)
* [Prerequisites](#prerequisites)
* [Usage](#usage)
* [Commands](#commands)
<!-- tocstop -->

# Prerequisites

<!-- prerequisites -->

[codeql-cli](https://github.com/github/codeql-cli-binaries) (>= 2.4.3)

<!-- prerequisitesstop -->

# Usage

<!-- usage -->
```sh-session
$ npm install -g planetql
$ planetql COMMAND
running command...
$ planetql (-v|--version|version)
planetql/0.9.5 darwin-x64 node-v15.7.0
$ planetql --help [COMMAND]
USAGE
  $ planetql COMMAND
...
```
<!-- usagestop -->

# Commands

<!-- commands -->
* [`planetql analyze [--file | -f] [--database=] [--format=]`](#planetql-analyze---file---f---database---format)
* [`planetql help [COMMAND]`](#planetql-help-command)
* [`planetql setup [--project=] [--database=]`](#planetql-setup---project---database)

## `planetql analyze [--file | -f] [--database=] [--format=]`

analyze vulnerabilities in c# project. must use after planetql setup, codeQL database has created.

```
USAGE
  $ planetql analyze [--file | -f] [--database=] [--format=]

OPTIONS
  -d, --database=database   [default: /opt/results/source_db] path to datatabase created after setup
  -f, --file                save result to file
  -h, --help                show CLI help
  --format=(csv|text|json)  [default: text] output format

EXAMPLE
  $ planetql analyze -f --format=csv
  Compiling query plan for /opt/ql/...ql
  Evaluation Done; ...
  Shutting down query evaluator.
```

_See code: [src/commands/analyze.ts](https://github.com/soohoio/planetql/blob/v0.9.5/src/commands/analyze.ts)_

## `planetql help [COMMAND]`

display help for planetql

```
USAGE
  $ planetql help [COMMAND]

ARGUMENTS
  COMMAND  command to show help for

OPTIONS
  --all  see all commands in CLI
```

_See code: [@oclif/plugin-help](https://github.com/oclif/plugin-help/blob/v3.2.2/src/commands/help.ts)_

## `planetql setup [--project=] [--database=]`

setup c# project before analyzed. creates codeQL database

```
USAGE
  $ planetql setup [--project=] [--database=]

OPTIONS
  -d, --database=database  [default: /opt/results/source_db] result directory path to store created database
  -h, --help               show command help
  -p, --project=project    [default: /opt/src] c# project path

EXAMPLE
  $ planetql setup
  Cleared database directory /opt/results
  Initializing database at /opt/results
  ...
  Successfully created database at /opt/results
```

_See code: [src/commands/setup.ts](https://github.com/soohoio/planetql/blob/v0.9.5/src/commands/setup.ts)_
<!-- commandsstop -->
