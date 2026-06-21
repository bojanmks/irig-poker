import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_API_BASE_URL}/hubs/game`)
  .configureLogging(signalR.LogLevel.Information)
  .build();

export default connection;