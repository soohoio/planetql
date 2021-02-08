import { Command, flags } from "@oclif/command";
import { exec } from "child_process";
import { existsSync } from "fs";
import * as rimraf from "rimraf";

export default class Setup extends Command {
  static description = `setup c# project before analyzed. creates codeQL database`;

  static usage = `setup [--project=] [--database=]`;

  static examples = [
    `$ planetql setup
Cleared database directory /opt/results
Initializing database at /opt/results
...
Successfully created database at /opt/results
`,
  ];

  static flags = {
    help: flags.help({ char: "h", description: "show command help" }),
    project: flags.string({
      char: "p",
      description: "c# project path",
      default: "/opt/src",
    }),
    database: flags.string({
      char: "d",
      description: "result directory path to store created database",
      default: "/opt/results/source_db",
    }),
  };

  async run() {
    const { flags } = this.parse(Setup);

    const project = flags.project;
    const database = flags.database;

    this.clearDatabaseDirectory(database);
    this.createDatabase(project, database);
  }

  private clearDatabaseDirectory(database: string) {
    if (existsSync(database)) {
      rimraf(database, () => {
        this.log(`Cleared database directory ${database}`);
      });
    }
  }

  private createDatabase(project: string, database: string) {
    const createDb = exec(
      `codeql database create --language=csharp ${database} -s ${project} `
    );

    createDb.stdout.on("data", (data) => {
      this.log(data);
    });
    createDb.stderr.on("data", (data) => {
      this.log(data);
    });
  }
}
