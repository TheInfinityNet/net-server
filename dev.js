const concurrently = require("concurrently");

const services = [
  { command: "npm run post:dev", name: "POST", prefixColor: "blue" },
  { command: "npm run comment:dev", name: "COMMENT", prefixColor: "green" },
  { command: "npm run file:dev", name: "FILE", prefixColor: "yellow" },
  { command: "npm run group:dev", name: "GROUP", prefixColor: "magenta" },
  { command: "npm run identity:dev", name: "IDENTITY", prefixColor: "cyan" },
  { command: "npm run mail:dev", name: "MAIL", prefixColor: "red" },
  {
    command: "npm run notification:dev",
    name: "NOTIFICATION",
    prefixColor: "blueBright",
  },
  { command: "npm run ocelot:dev", name: "OCELOT", prefixColor: "greenBright" },
  {
    command: "npm run profile:dev",
    name: "PROFILE",
    prefixColor: "magentaBright",
  },
  {
    command: "npm run reaction:dev",
    name: "REACTION",
    prefixColor: "cyanBright",
  },
  {
    command: "npm run relationship:dev",
    name: "RELATIONSHIP",
    prefixColor: "redBright",
  },
].map((service) => ({
  ...service,
  name: service.name.padEnd(15),
}));

const processManager = concurrently(services, {
  prefix: "name",
  restartTries: 3,
  killOthers: ["failure", "success"],
  output: "always",
});

processManager.result
  .then(() => {
    console.log("All services started successfully.");
  })
  .catch((error) => {
    console.error("Some service failed:", error);
  });
