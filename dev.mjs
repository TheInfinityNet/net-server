import concurrently from "concurrently";
import inquirer from "inquirer";
import chalk from "chalk";

const services = [
  { name: "COMMENT", profile: "Comment.Presentation", prefixColor: "green" },
  { name: "FILE", profile: "File.Presentation", prefixColor: "yellow" },
  { name: "GROUP", profile: "Group.Presentation", prefixColor: "magenta" },
  { name: "IDENTITY", profile: "Identity.Presentation", prefixColor: "cyan" },
  { name: "MAIL", profile: "Mail.Presentation", prefixColor: "red" },
  {
    name: "NOTIFICATION",
    profile: "Notification.Presentation",
    prefixColor: "blueBright",
  },
  {
    name: "OCELOT",
    profile: "Ocelot.Presentaition",
    prefixColor: "greenBright",
  },
  { name: "POST", profile: "Post.Presentation", prefixColor: "blue" },
  {
    name: "PROFILE",
    profile: "Profile.Presentation",
    prefixColor: "magentaBright",
  },
  {
    name: "REACTION",
    profile: "Reaction.Presentation",
    prefixColor: "cyanBright",
  },
  {
    name: "RELATIONSHIP",
    profile: "Relationship.Presentation",
    prefixColor: "redBright",
  },
];

async function runMenu() {
  const { servicesToRun } = await inquirer.prompt([
    {
      type: "checkbox",
      name: "servicesToRun",
      message: "Select the services to run:",
      choices: services.map((service) => ({
        name: service.name,
        value: service.profile,
      })),
    },
  ]);

  if (servicesToRun.length === 0) {
    console.log(chalk.red("No services selected. Exiting..."));
    return;
  }

  const { watchModeServices } = await inquirer.prompt([
    {
      type: "checkbox",
      name: "watchModeServices",
      message: "Select the services to run in watch mode:",
      choices: servicesToRun.map((profile) => {
        const service = services.find((s) => s.profile === profile);
        return { name: service.name, value: profile };
      }),
    },
  ]);

  const commands = servicesToRun.map((profile) => {
    const service = services.find((s) => s.profile === profile);
    return {
      command: watchModeServices.includes(profile)
        ? `dotnet watch run --launch-profile https --project ${profile}`
        : `dotnet run --launch-profile https --project ${profile}`,
      name: service.name,
      prefixColor: watchModeServices.includes(profile)
        ? service.prefixColor
        : "gray",
    };
  });

  const processManager = concurrently(commands, {
    prefix: "name",
    restartTries: 3,
    killOthers: ["failure", "success"],
    output: "always",
  });

  processManager.result
    .then(() => {
      console.log(chalk.green("All selected services started successfully."));
    })
    .catch((error) => {
      console.error(chalk.red("Some service failed:"), error);
    });
}

runMenu();
