import * as signalR from "@microsoft/signalr";

const logLevel = import.meta.env.DEV
  ? signalR.LogLevel.Information
  : signalR.LogLevel.None;

const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_API_BASE_URL}/hubs/game`)
  .configureLogging(logLevel)
  .build();

export default connection;