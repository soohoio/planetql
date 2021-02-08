import { Command, flags } from "@oclif/command";
import { ChildProcess, exec } from "child_process";
import { basename, dirname } from "path";

export default class Analyze extends Command {
  private qldir = "/opt/ql";

  private qlfile = "non_deterministic_iteration_local.ql";

  static description = `analyze vulnerabilities in c# project. must use after planetql setup, codeQL database has created.`;

  static usage = `analyze [--file | -f] [--database=] [--format=]`;

  static examples = [
    `$ planetql analyze -f --format=csv
Compiling query plan for /opt/ql/...ql
Evaluation Done; ...
Shutting down query evaluator.
`,
  ];

  static flags = {
    help: flags.help({ char: "h" }),
    file: flags.boolean({
      char: "f",
      description: "save result to file",
      default: false,
    }),
    format: flags.enum({
      options: ["csv", "text", "json"],
      default: "text",
      description: "output format",
    }),
    database: flags.string({
      char: "d",
      description: "path to datatabase created after setup",
      default: "/opt/results/source_db",
    }),
  };

  async run() {
    const { flags } = this.parse(Analyze);

    const printToFile = flags.file;
    const database = flags.database;
    const format = flags.format;

    if (!printToFile && format !== "text") {
      this.error("format flag can only used when file flag is set");
    }

    this.runQuery(printToFile, database, format);
  }

  private runQuery(printToFile: boolean, database: string, format: string) {
    if (printToFile) {
      this.printQueryResultToFile(database, format);
    } else {
      this.printQueryResultToConsole(database);
    }
  }

  private printQueryResultToFile(database: string, format: string) {
    const resultDir = dirname(database);
    const resultFile = basename(this.qlfile, ".ql");

    const queryRunProcess = exec(
      `codeql query run --database=${database} ${this.qldir}/${this.qlfile} --output=${resultDir}/${resultFile}.bqrs`
    );
    this.logAllConsoleOutput(queryRunProcess);

    queryRunProcess.on("exit", () => {
      const ext = format === "text" ? "txt" : format;
      const bqrsDecodeProcess = exec(
        `codeql bqrs decode --output=${resultDir}/${resultFile}.${ext} --format=${format} ${resultDir}/${resultFile}.bqrs`
      );
      this.logAllConsoleOutput(bqrsDecodeProcess);
    });
  }

  private printQueryResultToConsole(database: string) {
    const queryRunProcess = exec(
      `codeql query run --database=${database} ${this.qldir}/${this.qlfile}`
    );
    this.logAllConsoleOutput(queryRunProcess);
  }

  private logAllConsoleOutput(process: ChildProcess) {
    process.stdout.on("data", (data) => {
      this.log(data);
    });
    process.stderr.on("data", (data) => {
      this.log(data);
    });
  }
}
