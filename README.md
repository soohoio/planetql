# PlanetQL

Analyze C# project vunlerabilities

PlanetQL provides analyzing service for c# project.  
It uses [codeQL](https://github.com/github/codeql) to analyzing project with our custom queries.  
Currently provides `Non-deterministic iteration search` query and more queries will be provided later on.  
CodeQL environment is provided in docker image [soohoio/planetql](https://hub.docker.com/repository/docker/soohoio/planetql), image also provides CLI for easier use of codeQL.

**Table of Contents**

- [PlanetQL](#planetql)
  - [Usage](#usage)
    - [From scripts](#from-scripts)
      - [Example](#example)
    - [By scripts](#by-scripts)
      - [Example](#example-1)
  - [TroubleShooting](#troubleshooting)
  - [Contributing](#contributing)

## Usage

### From scripts

download planetQL container with command

```
docker pull soohoio/planetql
```

Run container with mounting volumes of `c# project` and `output` directory. After specifying volumes, execute planetql CLI with command `planetql` you can see manual with command `planetql help`.

You can find more information of PlanetQL CLI command in [PlanetQL-CLI help](planetql-cli/README.md)

```
docker run --rm --name planetql -v "${PROJECT_TO_ANALYZE}:/opt/src" -v "${OUTPUT_DIRECTORY}:/opt/results" soohoio/planetql planetql help
```

> Default path of `c# project` inside container is `/opt/src` and Default path of `output` directory is `/opt/results`.  
> Planetql CLI uses default path if you don't provide path when running command of planetQL CLI. Specify path to planetql cli with flags when you mounted your project at different path inside of container.

#### Example

```zsh
$ docker run --rm --name planetql -v "$(pwd)/sample_project:/opt/src" -v "$(pwd)/results:/opt/results" soohoio/planetql planetql setup
Initializing database ...


$ docker run --rm --name planetql -v "$(pwd)/sample_project:/opt/src" -v "$(pwd)/results:/opt/results" soohoio/planetql planetql analyze -f --format=csv
Compiling query plan ...
```

### By scripts

We provide shell script automates pulling docker image and running container with planetql cli commands

setup script

```
setup.sh <c# project path> <output directory>
```

analyze script

```
analyze.sh <output directory>
```

#### Example

```zsh
$ scripts/setup.sh sample_project results

$ scripts/analyze.sh results -f --format=csv
```

## TroubleShooting

Docker resource should be big enough to create codeql database. If you're stuck in `planetql set`(`codeql database create`) command, Try setting your docker environment to use more memory.

## Contributing

This project welcomes contributions and suggestions. Please open issues and Pull Requests for new features or bugs.
